using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class GlobalMods
    {
        public static List<ModForGlobalStats> GetGlobalMods()
        {
            List<ModForGlobalStats> mods = new();

            ModForGlobalStats m = new();

            m.Name = "EmptyMod";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.None;
            m.TierValues = Array.Empty<float>();
            m.Description = (m) => "Do nothing";
            m.ApplyMod = (m) => { return; };
            m.RemoveMod = (m) => { return; };
            mods.Add(m);

            mods.AddRange(GlobalAttackMods.GetGlobalMods());
            mods.AddRange(GlobalDefanceMods.GetGlobalMods());
            mods.AddRange(GlobalSpellMods.GetGlobalMods());
            mods.AddRange(GlobalUtilityMods.GetGlobalMods());
            mods.AddRange(GlobalDamageMods.GetGlobalMods());

            return mods;
        }
    }
}
