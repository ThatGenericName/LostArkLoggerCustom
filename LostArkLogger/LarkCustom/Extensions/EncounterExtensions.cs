using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LostArkLogger;

using LostArkLogger.LarkCustom;
using LostArkLogger.LarkCustom.Units;

namespace LostArkLogger.LarkCustom.Extensions
{
    public static class EncounterExtensions
    {

        public static Dictionary<uint, DamageDataStruct> GetEntityDamageInPeriod(this Encounter encounter, int period, Entity entity)
        {
            var baseSearch = encounter.Infos.Where(i => i.SourceEntity.Type == Entity.EntityType.Player);
            // Filters out anything older than specified period
            var timeFilter = encounter.Infos.Where(i => i.Time > DateTime.Now - new TimeSpan(0, 0, period));

            IEnumerable<IGrouping<uint, LogInfo>> groupedByEntityName
                = timeFilter.Where(i => i.SourceEntity == entity).GroupBy(i => i.SkillId);

            Dictionary<uint, DamageDataStruct> result = new Dictionary<uint, DamageDataStruct>();

            foreach (IGrouping<uint, LogInfo> groupedInfo in groupedByEntityName)
            {
                DamageDataStruct dds = new DamageDataStruct();

                foreach (LogInfo logInfo in groupedInfo)
                {
                    dds.Hits++;
                    dds.Damage += logInfo.Damage;
                    dds.Stagger += logInfo.Stagger;
                    if (logInfo.Crit) dds.Crits++;
                    if (logInfo.FrontAttack) dds.FrontAttacks++;
                    if (logInfo.BackAttack) dds.BackAttacks++;
                    if (logInfo.Counter) dds.CounterAttacks++;
                }

                result.Add(groupedInfo.Key, dds);
            }

            return result;
        }

        /// <summary>
        /// Gets the player damage information from the last <c>period</c> seconds.
        /// </summary>
        /// <param name="encounter"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static Dictionary<Entity, DamageDataStruct[]> GetRaidDamageInPeriod(this Encounter encounter, int period)
        {
            var baseSearch = encounter.Infos.Where(i => i.SourceEntity.Type == Entity.EntityType.Player);

            IEnumerable<IGrouping<Entity, LogInfo>> groupedByEntityNoTime = baseSearch.GroupBy(i => i.SourceEntity);

            Dictionary<Entity, DamageDataStruct[]> result = new Dictionary<Entity, DamageDataStruct[]>();


            foreach (IGrouping<Entity, LogInfo> groupedInfo in groupedByEntityNoTime)
            {
                DamageDataStruct dds = new DamageDataStruct();

                foreach (LogInfo logInfo in groupedInfo)
                {
                    dds.Hits++;
                    dds.Damage += logInfo.Damage;
                    dds.Stagger += logInfo.Stagger;
                    if (logInfo.Crit) dds.Crits++;
                    if (logInfo.FrontAttack) dds.FrontAttacks++;
                    if (logInfo.BackAttack) dds.BackAttacks++;
                    if (logInfo.Counter) dds.CounterAttacks++;
                }
                DamageDataStruct[] ddsArr = new DamageDataStruct[2];
                ddsArr[0] = dds;
                result.Add(groupedInfo.Key, ddsArr);
            }

            // Filters out anything older than specified period
            var timeFilter = encounter.Infos.Where(i => i.Time > DateTime.Now - new TimeSpan(0, 0, period));

            IEnumerable<IGrouping<Entity, LogInfo>> groupedByEntity = timeFilter.GroupBy(i => i.SourceEntity);
            

            foreach (IGrouping<Entity, LogInfo> groupedInfo in groupedByEntity)
            {
                DamageDataStruct dds = new DamageDataStruct();
                
                foreach (LogInfo logInfo in groupedInfo)
                {
                    dds.Hits++;
                    dds.Damage += logInfo.Damage;
                    dds.Stagger += logInfo.Stagger;
                    if (logInfo.Crit) dds.Crits++;
                    if (logInfo.FrontAttack) dds.FrontAttacks++;
                    if (logInfo.BackAttack) dds.BackAttacks++;
                    if (logInfo.Counter) dds.CounterAttacks++;
                }

                if (result.ContainsKey(groupedInfo.Key))
                {
                    result[groupedInfo.Key][1] = dds;
                }
                else
                {
                    DamageDataStruct[] ddsArr = new DamageDataStruct[2];
                    ddsArr[1] = dds;
                    result.Add(groupedInfo.Key, ddsArr);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the player damage information across the entire raid attempt.
        /// </summary>
        /// <param name="encounter"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static Dictionary<Entity, DamageDataStruct> GetRaidDamage(this Encounter encounter)
        {
            var baseSearch = encounter.Infos.Where(i => i.SourceEntity.Type == Entity.EntityType.Player);

            IEnumerable<IGrouping<Entity, LogInfo>> groupedByEntity = baseSearch.GroupBy(i => i.SourceEntity);

            Dictionary<Entity, DamageDataStruct> result = new Dictionary<Entity, DamageDataStruct>();

            foreach (IGrouping<Entity, LogInfo> groupedInfo in groupedByEntity)
            {
                DamageDataStruct dds = new DamageDataStruct();

                foreach (LogInfo logInfo in groupedInfo)
                {
                    dds.Hits++;
                    dds.Damage += logInfo.Damage;
                    dds.Stagger += logInfo.Stagger;
                    if (logInfo.Crit) dds.Crits++;
                    if (logInfo.FrontAttack) dds.FrontAttacks++;
                    if (logInfo.BackAttack) dds.BackAttacks++;
                    if (logInfo.Counter) dds.CounterAttacks++;
                }

                result.Add(groupedInfo.Key, dds);
            }

            return result;
        }
    }
}
