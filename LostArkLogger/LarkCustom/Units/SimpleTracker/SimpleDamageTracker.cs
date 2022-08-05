using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LostArkLogger;

using LostArkLogger.LarkCustom;
using LostArkLogger.LarkCustom.UI;
using LostArkLogger.LarkCustom.Extensions;
using LostArkLogger.LarkCustom.Config;

namespace LostArkLogger.LarkCustom.Units
{
    public class SimpleDamageTracker
    {
        enum Scope
        {
            Raid,
            Player
        }

        private Scope scope;

        private Parser parser;

        private Encounter currentEncounter;

        private List<Brush> brushList;

        private int encounterPlayerCount;

        private Dictionary<UInt64, Entity> playerEntities;
        private Dictionary<string, Entity> playerEntitiesByName;

        private UInt64 targetEntityID;

        public int Period { get; set; }

        private void Init(Parser parserT)
        {
            this.parser = parserT;
            currentEncounter = parser.currentEncounter;
            int count = 0;
            foreach (var kvp in currentEncounter.Entities) // might need to replace currentEncounter.Entities with currentEncouter.PartyEntities
            {
                var entity = kvp.Value;
                if (entity.Type == Entity.EntityType.Player)
                {
                    playerEntities[entity.EntityId] = entity;
                    playerEntitiesByName[entity.Name] = entity;
                    count++;
                }
            }
            encounterPlayerCount = count;
        }

        public Content CreateContentWindow()
        {
            Content content = new SimpleDamageTrackerContent(this);

            return content;
        }

        public Dictionary<Entity, DamageDataStruct[]> GetRaidPeriodDamage()
        {
            return currentEncounter.GetRaidDamageInPeriod(UserConfig.CurrentUserConfig.DPSPeriod);
        }

        public Dictionary<Entity, DamageDataStruct> GetRaidDamage()
        {
            return currentEncounter.GetRaidDamage();
        }

        public Dictionary<uint, DamageDataStruct> GetEntityDamageData(Entity entity)
        {
            return currentEncounter.GetEntityDamageInPeriod(UserConfig.CurrentUserConfig.DPSPeriod, entity);
        }

        public TimeSpan GetEncounterTime()
        {
            DateTime startTime = parser.currentEncounter.Start;
            DateTime endTime = parser.currentEncounter.End == default(DateTime) ? DateTime.Now : parser.currentEncounter.End;

            return endTime - startTime;
        }
    }

}
