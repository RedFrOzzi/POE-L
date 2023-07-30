using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class GlobalDefanceMods
    {
        public static List<ModForGlobalStats> GetGlobalMods()
        {
            List<ModForGlobalStats> mods = new();

            ModForGlobalStats m = new();

            //---Health---
            m.Name = "FlatGlobalHP";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]} GLOBAL health points";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatHP((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatHP(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalHP";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL health by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseHP(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseHP(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalHP";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE GLOBAL health";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreHP(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreHP(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Health-Regen---
            m = new();
            m.Name = "FlatGlobalHealthRegen";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { .025f, .022f, .02f, .017f, .015f, .012f, .01f, .007f, .005f, .002f };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]} GLOBAL health regeneration";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatHPRegeneration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatHPRegeneration(-m.TierValues[m.Tier]);
            mods.Add(m);
            
            m = new();
            m.Name = "IncreaseGlobalHealthRegen";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL health regeneration by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseHPRegeneration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseHPRegeneration(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalHealthRegen";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Health;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE GLOBAL health regeneration";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreHPRegeneration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreHPRegeneration(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Armor---
            m = new();
            m.Name = "FlatGlobalArmor";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Armor;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 200, 170, 150, 120, 100, 80, 60, 40, 20, 10 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]} GLOBAL armor";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatArmor((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatArmor(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalArmor";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Armor;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL armor by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseArmor(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseArmor(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalArmor";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Armor;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE GLOBAL armor";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreArmor(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreArmor(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Magic-Resist---
            m = new();
            m.Name = "FlatGlobalMagicResist";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.MagicResist;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 200, 170, 150, 120, 100, 80, 60, 40, 20, 10 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]} GLOBAL magic resist";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatMagicResist((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.AddFlatMagicResist(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalMagicResist";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.MagicResist;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL magic resist by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseMagicResist(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.IncreaseMagicResist(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalMagicResist";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.MagicResist;
            m.ModTags[1] = ModTag.Defance;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE GLOBAL magic resist";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreMagicResist(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.DefanceSC.MoreMagicResist(-m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}
