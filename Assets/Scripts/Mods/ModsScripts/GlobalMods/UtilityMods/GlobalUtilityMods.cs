using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class GlobalUtilityMods
    {
        public static List<ModForGlobalStats> GetGlobalMods()
        {
            List<ModForGlobalStats> mods = new();

            ModForGlobalStats m = new();

            //---Movement-speed---
            m.Name = "IncreaseGlobalMoveSpeed";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Speed;
            m.ModTags[1] = ModTag.Movement;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase movement speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseMovementSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseMovementSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "ReduceGlobalMoveSpeed";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Speed;
            m.ModTags[1] = ModTag.Movement;
            m.TierValues = new float[] { -5, -10, -15, -20, -25, -30, -35, -40, -45, -50 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Reduce movement speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseMovementSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseMovementSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Collector---
            m = new();
            m.Name = "IncreaseGlobalCollectorRadius";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase collector radius by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseCollectorRadius(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseCollectorRadius(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalCollectorRadius";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase collector radius by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreCollectorRadius(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreCollectorRadius(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Duration---
            m = new();
            m.Name = "IncreaseGlobalEffectDuration";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Duration;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL duration of effects by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseEffectDuration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseEffectDuration(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalEffectDuration";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Duration;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% MORE GLOBAL duration of effects";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreEffectDuration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreEffectDuration(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Area---
            m = new();
            m.Name = "IncreaseGlobalArea";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Area;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL area by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseArea(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseArea(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalArea";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Area;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% MORE GLOBAL area";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreArea(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreArea(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Experience---
            m = new();
            m.Name = "IncreaseGlobalExperience";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase experience gains by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseExperience(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseExperience(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalExperience";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% MORE experience gains";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreExperience(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreExperience(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Gold---
            m = new();
            m.Name = "IncreaseGlobalGold";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase gold gains by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseGoldGain(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseGoldGain(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalGold";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% MORE gold gains";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreGoldGain(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreGoldGain(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Projectile-Speed---
            m = new();
            m.Name = "IncreaseGlobalProjSpeed";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase projectile speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseProjectileSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.IncreaseProjectileSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalProjSpeed";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% MORE projectile speed";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreProjectileSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.UtilitySC.MoreProjectileSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}
