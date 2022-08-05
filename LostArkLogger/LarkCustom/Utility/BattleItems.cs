using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostArkLogger.LarkCustom.Utility
{
    public static class BattleItems
    {
        private static readonly Dictionary<Int32, (string, Int32)> Items = new();

        public static string GetItemNameFromID(int id)
        {
            return Items[id].Item1;
        }

        public static Int32 GetItemIconIndexFromID(int id)
        {
            return Items[id].Item2;
        }

        public static bool Contains(Int32 id)
        {
            return Items.ContainsKey(id);
        }

        public static void init()
        {
            Items.Add(32001, ("[Battle Item] Flash Grenade", 4));
            Items.Add(32006, ("[Battle Item] Splended Flash Grenade", 4));
            Items.Add(32011, ("[Battle Item] Flame Grenade", 3));
            Items.Add(32019, ("[Battle Item] Splended Flame Grenade", 3));
            Items.Add(32021, ("[Battle Item] Frost Grenade", 5));
            Items.Add(32024, ("[Battle Item] Splended Frost Grenade", 5));
            Items.Add(32031, ("[Battle Item] Electric Grenade", 2));
            Items.Add(32036, ("[Battle Item] Splended Electric Grenade", 2));
            Items.Add(32131, ("[Battle Item] Huge Crescent Star", -1)); //? what is this?
            Items.Add(32141, ("[Battle Item] Destruction Bomb", 10));
            Items.Add(32143, ("[Battle Item] Splended Destruction Bomb", 10));
            Items.Add(32241, ("[Battle Item] Dark Grenade", 1));
            Items.Add(32245, ("[Battle Item] Splended Dark Grenade", 1));
            Items.Add(32251, ("[Battle Item] Vibration Bomb", -1)); // uwu whats this?
            Items.Add(32311, ("[Battle Item] Whirlwind Grenade", 6));
            Items.Add(32314, ("[Battle Item] Splended Whirlwind Grenade", 6));
            Items.Add(32321, ("[Battle Item] Clay Grenade", 0));
            Items.Add(32326, ("[Battle Item] Splended Clay Grenade", 0));
            Items.Add(33011, ("[Minor Battle Item] Lesser Flame Grenade", 3)); // when do we see lesser and rusty battle items?
            Items.Add(33031, ("[Minor Battle Item] Lesser Electric Grenade", 2));
            Items.Add(33131, ("[Minor Battle Item] Rusty Giant Crescent Star", -1));
            Items.Add(33141, ("[Minor Battle Item] Rusty Destruction Bomb", 10));
        }
    }
}
