using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace LostArkLogger.LarkCustom.Utility
{
    public enum PlayerClassEnum
    {
        Arcanist = 2,
        Artillerist = 15,
        Assassin = 19,
        Bard = 17,
        Berserker = 3,
        Deadeye = 5,
        Destroyer = 0,
        Deathblade = 22,
        FemaleGunner = 25,
        Glavier = 18,
        Gunlancer = 9,
        Gunner = 8,
        Gunslinger = 26,
        Machinist = 23,
        Mage = 10,
        MaleMartialArtist = 27,
        MartialArtist = 6,
        Paladin = 22,
        Reaper = 24,
        Scrapper = 9,
        ShadowHunter = 21,
        Sharpshooter = 14,
        Sorceress = 29,
        Soulfist = 13,
        Striker = 28,
        Summoner = 11,
        Wardancer = 4,
        Warrior = 12,
        Invalid = 16
    }

    public static class PlayerClassMethods
    {
        private static Dictionary<string, PlayerClassEnum> playerClasses = new Dictionary<string, PlayerClassEnum>();
        private static Dictionary<UInt32, PlayerClassEnum> playerClassesById = new Dictionary<UInt32, PlayerClassEnum>();
        public static PlayerClassEnum GetPlayerClassEnumFromString(string playerClass)
        {
            PlayerClassEnum returnVal = playerClasses.TryGetValue(playerClass, out returnVal) ? returnVal : PlayerClassEnum.Invalid;
            return returnVal;
        }

        public static PlayerClassEnum GetPlayerClassEnumFromID(UInt32 id)
        {
            PlayerClassEnum returnVal = playerClassesById.TryGetValue(id, out returnVal) ? returnVal : PlayerClassEnum.Invalid;
            return returnVal;
        }

        public static Rectangle GetSpriteLocation(PlayerClassEnum pce)
        {
            var imageSize = 64;
            var x = (int)pce % 17;
            var y = (int)pce / 17;
            return new Rectangle(x * imageSize, y * imageSize, imageSize, imageSize);
        }
        private static void Init()
        {
            foreach (PlayerClassEnum plrClass in Enum.GetValues(typeof(PlayerClassEnum)))
            {
                string enumName = plrClass.ToString();
                playerClasses.Add(enumName, plrClass);
            }

            playerClassesById = new Dictionary<uint, PlayerClassEnum>
            {
                [101] = playerClasses["Warrior"],
                [201] = playerClasses["Mage"],
                [301] = playerClasses["MartialArtist"],
                [401] = playerClasses["Assassin"],
                [501] = playerClasses["Gunner"],
                [601] = playerClasses["Specialist"],
                [102] = playerClasses["Berserker"],
                [103] = playerClasses["Destroyer"],
                [104] = playerClasses["Gunlancer"],
                [105] = playerClasses["Paladin"],
                [202] = playerClasses["Arcanist"],
                [203] = playerClasses["Summoner"],
                [204] = playerClasses["Bard"],
                [205] = playerClasses["Sorceress"],
                [302] = playerClasses["Wardancer"],
                [303] = playerClasses["Scrapper"],
                [304] = playerClasses["Soulfist"],
                [305] = playerClasses["Glavier"],
                [402] = playerClasses["Deathblade"],
                [403] = playerClasses["Shadowhunter"],
                [404] = playerClasses["Reaper"],
                [502] = playerClasses["Sharpshooter"],
                [503] = playerClasses["Deadeye"],
                [504] = playerClasses["Artillerist"],
                [505] = playerClasses["Scouter"],
                [511] = playerClasses["FemaleGunner"],
                [512] = playerClasses["Gunslinger"],
                [311] = playerClasses["MaleMartialArtist"],
                [312] = playerClasses["Striker"],
                [602] = playerClasses["Artist"]
            };

        }

    }

}
