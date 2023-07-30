using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class GlobalDamageMods
    {
        public static List<ModForGlobalStats> GetGlobalMods()
        {
            List<ModForGlobalStats> mods = new();

            ModForGlobalStats m = new();

            m.Name = "IncreaseGlobalDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Damage;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreaseDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreaseDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalPhysicalDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Physical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL physical damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreasePhysicalDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreasePhysicalDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalMagicDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Magic;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL magic damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreaseMagicDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreaseMagicDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalAreaDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Area;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL magic damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreaseAreaDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.IncreaseAreaDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatGlobalPhysDotMulti";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.DamageOverTime;
            m.ModTags[1] = ModTag.Physical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]}% to GLOBAL physical damage over time multiplier";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.AddFlatPhysDOTMulti((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.AddFlatPhysDOTMulti(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatGlobalMagicDotMulti";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.DamageOverTime;
            m.ModTags[1] = ModTag.Magic;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]}% to GLOBAL magic damage over time multiplier";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.AddFlatMagicDOTMulti((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.GlobalSC.AddFlatMagicDOTMulti(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}