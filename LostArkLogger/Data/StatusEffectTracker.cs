﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostArkLogger
{
    public class StatusEffectTracker
    {
        private readonly ConcurrentDictionary<UInt64, ConcurrentDictionary<UInt64, StatusEffect>> StatusEffectRegistry;
        public Parser parser;
        public event Action? OnChange;
        public event Action<StatusEffect, TimeSpan>? OnStatusEffectEnded;
        public StatusEffectTracker(Parser p)
        {
            StatusEffectRegistry = new ConcurrentDictionary<UInt64, ConcurrentDictionary<UInt64, StatusEffect>>();
            parser = p;
            parser.beforeNewZone += BeforeNewZone;
        }

        public void BeforeNewZone()
        {
            // cancel remaining statuseffects so they get added to the old encounter
            foreach(var statusEffectList in StatusEffectRegistry)
            {
                foreach(var statusEffect in statusEffectList.Value)
                {
                    var duration = (DateTime.UtcNow - statusEffect.Value.Started);
                    OnStatusEffectEnded?.Invoke(statusEffect.Value, duration);
                }
            }
            StatusEffectRegistry.Clear();
        }

        public void Process(PKTInitPC packet)
        {
            var statusEffectList = GetStatusEffectList(packet.PlayerId);
            foreach (var statusEffect in packet.statusEffectDatas)
            {
                ProcessStatusEffectData(statusEffect, packet.PlayerId, statusEffect.SourceId, statusEffectList, StatusEffect.StatusEffectType.Local);
            }
            OnChange?.Invoke();
        }

        public void Process(PKTNewNpc packet)
        {
            var statusEffectList = GetStatusEffectList(packet.npcStruct.NpcId);
            foreach (var statusEffect in packet.npcStruct.statusEffectDatas)
            {
                ProcessStatusEffectData(statusEffect, packet.npcStruct.NpcId, statusEffect.SourceId, statusEffectList, StatusEffect.StatusEffectType.Local);
            }
            OnChange?.Invoke();
        }

        public void Process(PKTNewPC packet)
        {
            var statusEffectList = GetStatusEffectList(packet.pCStruct.PartyId);
            foreach (var statusEffect in packet.pCStruct.statusEffectDatas)
            {
                ProcessStatusEffectData(statusEffect, packet.pCStruct.PartyId, statusEffect.SourceId, statusEffectList, StatusEffect.StatusEffectType.Party);
            }
            OnChange?.Invoke();
        }

        private void ProcessStatusEffectData(StatusEffectData effectData, UInt64 targetId, UInt64 sourceId, ConcurrentDictionary<UInt64, StatusEffect> effectList, StatusEffect.StatusEffectType effectType)
        {
            Entity sourceEntity = parser.GetSourceEntity(sourceId);
            var statusEffect = new StatusEffect { Started = DateTime.UtcNow, StatusEffectId = effectData.StatusEffectId, InstanceId = effectData.EffectInstanceId, SourceId = sourceEntity.EntityId, TargetId = targetId, Type = effectType };
            // end this buf now, it got refreshed
            if (effectList.Remove(statusEffect.InstanceId, out var oldStatusEffect))
            {
                var duration = DateTime.UtcNow - oldStatusEffect.Started;
                OnStatusEffectEnded?.Invoke(oldStatusEffect, duration);
            }
            effectList.TryAdd(statusEffect.InstanceId, statusEffect);
        }

        public void Process(PKTStatusEffectAddNotify effect)
        {
            var statusEffectList = GetStatusEffectList(effect.ObjectId);//get by targetId
            ProcessStatusEffectData(effect.statusEffectData, effect.ObjectId, effect.statusEffectData.SourceId, statusEffectList, StatusEffect.StatusEffectType.Local);
            OnChange?.Invoke();
        }

        public void Process(PKTPartyStatusEffectAddNotify effect)
        {
            foreach (var statusEffect in effect.statusEffectDatas)
            {
                var applierId = statusEffect.SourceId;
                if (effect.PlayerIdOnRefresh != 0x0)
                    applierId = effect.PlayerIdOnRefresh;
                var statusEffectList = GetStatusEffectList(applierId);
                ProcessStatusEffectData(statusEffect, effect.PartyId, applierId, statusEffectList, StatusEffect.StatusEffectType.Party);
            }
            OnChange?.Invoke();
        }

        public void Process(PKTPartyStatusEffectRemoveNotify effect)
        {
            var statusEffectList = GetStatusEffectList(effect.PartyId);
            foreach (var effectInstanceId in effect.StatusEffectIds)
            {
                if (statusEffectList.Remove(effectInstanceId, out var oldStatusEffect))
                {
                    var duration = DateTime.UtcNow - oldStatusEffect.Started;
                    OnStatusEffectEnded?.Invoke(oldStatusEffect, duration);
                }
            }
            OnChange?.Invoke();
        }

        public void Process(PKTStatusEffectRemoveNotify effect)
        {
            var statusEffectList = GetStatusEffectList(effect.ObjectId);
            foreach (var effectInstanceId in effect.InstanceIds)
            {
                if (statusEffectList.Remove(effectInstanceId, out var oldStatusEffect))
                {
                    var duration = DateTime.UtcNow - oldStatusEffect.Started;
                    OnStatusEffectEnded?.Invoke(oldStatusEffect, duration);
                }
            }
            OnChange?.Invoke();
        }

        public void Process(PKTDeathNotify packet)
        {
            if(StatusEffectRegistry.Remove(packet.TargetId, out var statusEffectList))
            {
                foreach (var statusEffect in statusEffectList)
                {
                    var oldStatusEffect = statusEffect.Value;
                    var duration = DateTime.UtcNow - oldStatusEffect.Started;
                    OnStatusEffectEnded?.Invoke(oldStatusEffect, duration);
                }
            }
            OnChange?.Invoke();
        }

        public int GetStatusEffectCountFor(UInt64 PlayerId)
        {
            return GetStatusEffectList(PlayerId).Count;
        }

        public ConcurrentDictionary<UInt64, ConcurrentDictionary<UInt64, StatusEffect>> GetStatusEffectRegistry()
        {
            return StatusEffectRegistry;
        }

        private ConcurrentDictionary<UInt64, StatusEffect> GetStatusEffectList(UInt64 targetId)
        {
            if (!StatusEffectRegistry.TryGetValue(targetId, out var statusEffectList))
            {
                statusEffectList = new ConcurrentDictionary<UInt64, StatusEffect>();
                StatusEffectRegistry.TryAdd(targetId, statusEffectList);
            }
            return statusEffectList;
        }

    }
}
