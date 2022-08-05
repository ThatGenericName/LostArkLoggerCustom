using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostArkLogger.LarkCustom.Units
{
    public struct DamageDataStruct
    {
        public UInt64 Damage;
        public UInt32 Hits;
        public UInt32 Crits;
        public UInt64 TimeAlive;
        public UInt64 CounterAttacks;
        public UInt64 FrontAttacks;
        public UInt64 BackAttacks;
        public UInt64 Stagger;


        public double CritRate { get => Crits / (double)Hits; }
        public double FrontAttackRate { get => FrontAttacks / (double)Hits; }
        public double BackAttackRate { get => BackAttacks / (double)Hits; }
    }
}
