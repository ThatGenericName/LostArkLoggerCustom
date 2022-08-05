using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using LostArkLogger.LarkCustom.Units;
using LostArkLogger.LarkCustom.UI;


namespace LostArkLogger.LarkCustom.Config
{
    public class UserConfig
    {
        public static UserConfig? CurrentUserConfig { get; private set; }
        public ClassBrushes ClassBrushes { get; set; } = new ClassBrushes(true);
        public bool SplitParties { get; set; } = false;
        public bool UserAtTop { get; set; } = false;
        public int BattleHistoryPeriod { get; set; } = 40;
        public CombatHistoryContent.CombatHistoryMode CombatHistoryMode { get; set; } = CombatHistoryContent.CombatHistoryMode.All;
        public RaidInfo.RaidInfoEnum CurrentRaid { get; set; } = RaidInfo.RaidInfoEnum.Default;
        public int DPSPeriod { get; set; } = 40;

        public RaidInfo.DifficultyModifier CurrentDifficulty { get; set; } = RaidInfo.DifficultyModifier.Normal;

        public UserConfig()
        {

        }

        public static void Init()
        {
            if (File.Exists(Properties.Resources.UserSettingsPath))
            {
                string data = File.ReadAllText(Properties.Resources.UserSettingsPath);
                CurrentUserConfig = JsonSerializer.Deserialize<UserConfig>(data);
            }
            else
            {
                CurrentUserConfig = new UserConfig();
            }
        }

        public static void Init(string data)
        {
            CurrentUserConfig = JsonSerializer.Deserialize<UserConfig>(data);
        }

        public static void Serialize()
        {
            if (CurrentUserConfig == null)
            {
                Init();
            }
            string data = JsonSerializer.Serialize<UserConfig>(CurrentUserConfig);
            File.WriteAllText(Properties.Resources.UserSettingsPath, data);
        }

        public Content CreateContentWindow()
        {
            Content content = new UserConfigContent(this);

            return content;
        }
    }
}


