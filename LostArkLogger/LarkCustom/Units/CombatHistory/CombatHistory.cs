using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LostArkLogger;

using LostArkLogger.LarkCustom;
using LostArkLogger.LarkCustom.UI;
using LostArkLogger.LarkCustom.Extensions;

namespace LostArkLogger.LarkCustom.Units
{
    public class CombatHistory
    {

        public LogQueue<LogInfo> PlayerCombatLog; // stuff player does
        public LogQueue<LogInfo> EnemyCombatLog; // stuff done to player
        public CombatHistory(Parser parser, int logLength)
        {
            PlayerCombatLog = new LogQueue<LogInfo>(logLength);
            parser.onCombatEvent += onDamageEvent;
        }

        public void onDamageEvent(LogInfo log)
        {
            if (log.SourceEntity.Type == Entity.EntityType.Player)
            {
                PlayerCombatLog.Add(log);
            }
            else if (log.DestinationEntity.Type == Entity.EntityType.Player && log.SourceEntity.Type != Entity.EntityType.Player)
            {
                EnemyCombatLog.Add(log);
            }
        }

        public LogInfo[] GetPlayerCombatLog()
        {
            return PlayerCombatLog.GetQueueContent();
        }

        public LogInfo[] GetEnemyCombatLog()
        {
            return EnemyCombatLog.GetQueueContent();
        }

        public LogInfo[] GetAllCombatLog()
        {
            LogInfo[] playerAct = PlayerCombatLog.GetQueueContent();
            LogInfo[] enemyAct = EnemyCombatLog.GetQueueContent();

            LogInfo[] totalLog = new LogInfo[playerAct.Length + enemyAct.Length];

            int pcl = 0, ecl = 0, tcl = 0;

            while (pcl < playerAct.Length || ecl < enemyAct.Length)
            {
                if (pcl == playerAct.Length)
                {
                    totalLog[tcl++] = enemyAct[ecl++];
                }
                else if (ecl == enemyAct.Length)
                {
                    totalLog[tcl++] = playerAct[pcl++];
                }
                else if (playerAct[pcl].Time < enemyAct[ecl].Time)
                {
                    totalLog[tcl++] = playerAct[pcl++];
                }
                else
                {
                    totalLog[tcl++] = enemyAct[ecl++];
                }
            }

            return totalLog;
        }
    }
}
