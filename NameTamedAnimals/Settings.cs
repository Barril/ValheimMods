using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NameTamedAnimals
{
    public static class Settings
    {
        public static ConfigEntry<string> RenameModifierKey { get; private set; }

        public static void Init()
        {
            RenameModifierKey = Plugin.Instance.Config.Bind(
                "General",
                "RenameModifierKey",
                "LeftShift",
                "Holding this key while interacting with a tamed animal will bring up the rename dialog.");
        }
    }
}
