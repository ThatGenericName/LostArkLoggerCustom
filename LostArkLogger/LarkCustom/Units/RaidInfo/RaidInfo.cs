using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostArkLogger.LarkCustom.Units
{
    public class RaidInfo : IDisposable
    {
        public enum RaidInfoEnum
        {
            Default,
            ArgosP1,
            ArgosP2,
            ArgosP3,
            ValtanG1,
            ValtanG2,
            VykasG1,
            VykasG2,
            Deskaluda,
            Kungelanium,
            NightFoxYoho,
            Igrexion,
            AirasOculus,
            OrehasPreveza,
            SeaOfIndolence,
            TranquilKarkosa,
            AlericsSanctuary,
            RoadOfLament,
            ForgeOfFallenPride,
            HallOfTheTwistedWarlod,
            HildebrandtPalace,
            DemonBeastCanyon,
            NecromancersOrigin
        }

        public enum DifficultyModifier
        {
            Normal,
            Hard,
            Inferno
        }

        public Parser Parser { get; set; }

        private BossInfo[][] bosses;

        private int currentBoss;

        public RaidInfo(Parser parser)
        {
            if (BossEntityInfos == null)
            {
                StaticInit();
            }
            Parser = parser;
            parser.onCombatEvent += onDamageEvent;
            parser.onNewZone += onNewZone;
            parser.onRaidReset += onRaidReset;
        }

        public Dictionary<string, ulong> GetDamageDealt(int bossSetInd)
        {
            Dictionary<string, ulong> damageDealt = new Dictionary<string, ulong>();
            foreach (BossInfo bossInfo in bosses[currentBoss])
            {
                damageDealt[bossInfo.Title] = bossInfo.DamageDealt;

            }
            return damageDealt;
        }

        public void onDamageEvent(LogInfo logInfo)
        {
            foreach (BossInfo bi in bosses[currentBoss])
            {
                if (bi.BossIDs.Contains(logInfo.DestinationEntity.EntityId))
                {
                    bi.ApplyDamage(logInfo);
                    return;
                }
            }
        }

        public void onNewZone()
        {

        }

        public void onRaidReset(bool success)
        {
            if (success)
            {
                currentBoss++;
            }
            else
            {
                Reset();
            }
        }
        public void Reset()
        {
            foreach (BossInfo bossInfo in bosses[currentBoss])
            {
                bossInfo.DamageDealt = 0;
            }
        }


        // Static RaidInfo stuff here;

        public struct BossEntityInfo
        {
            public string Name;
            public int HP;
            public int[] BossPhases;
            public int BarCount;
            public UInt64 BossID;
        }

        public static void StaticInit()
        {
            BossEntityInfos = new Dictionary<ulong, BossEntityInfo>();
            // Deserialize file with boss info here.
        }

        public void Dispose()
        {
            Parser.onRaidReset -= onRaidReset;
            Parser.onNewZone -= onNewZone;
            Parser.onCombatEvent -= onDamageEvent;
        }

        public static Dictionary<UInt64, BossEntityInfo> BossEntityInfos { get; private set; }

    }
}
