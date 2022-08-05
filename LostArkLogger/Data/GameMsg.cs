﻿using System;
using System.Collections.Generic;

using System.Text.Json;
using System.IO;

namespace LostArkLogger
{
    public class GameMsg
    {
        public static Dictionary<String, String> Items = (Dictionary<String, String>)ObjectSerialize.Deserialize(Properties.Settings.Default.Region == Region.Steam ? Properties.Resources.GameMsg_English : Properties.Resources.GameMsg);
    }
}
