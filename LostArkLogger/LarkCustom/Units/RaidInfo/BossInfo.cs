using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace LostArkLogger.LarkCustom.Units
{
    public class BossInfo
    {
        public int[] MechanicPoints { get; set; }

        public ulong TotalHP { get; set; }

        public HashSet<ulong> BossIDs;
        public string Title { get; set; }
        public ulong DamageDealt { get; set; } = 0;

        public void ApplyDamage(LogInfo li)
        {
            DamageDealt += li.Damage;
        }

        // static methods here

        private static Dictionary<ulong, string> _npcItems =
            JsonSerializer.Deserialize<Dictionary<ulong, string>>(Encoding.Default.GetString(Properties.Resources.npcNames));

        private static string GetNPCNameById(ulong id)
        {
            return _npcItems[id];
        }

    }
}
