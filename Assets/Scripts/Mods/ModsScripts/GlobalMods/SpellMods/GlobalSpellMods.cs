using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class GlobalSpellMods
    {
        public static List<ModForGlobalStats> GetGlobalMods()
        {
            List<ModForGlobalStats> mods = new();

            ModForGlobalStats m = new();

            //---Damage---
            m.Name = "IncreaseGlobalSpellDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Spell;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL spell damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseSpellDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseSpellDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalSpellDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Spell;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE GLOBAL spell damage";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreSpellDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreSpellDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Crit---
            m = new();
            m.Name = "IncreaseGlobalSpellCritChance";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL spell critical hit chance by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseSpellCritChance(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseSpellCritChance(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalSpellCritChance";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE GLOBAL spell critical hit chance";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreSpellCritChance(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreSpellCritChance(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatGlobalSpellCritMulti";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]}% to GLOBAL spell critical hit multiplier";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.AddFlatSpellCritMultiplier((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.AddFlatSpellCritMultiplier(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalSpellCritMulti";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL spell critical hit multiplier by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseSpellCritMultiplier(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseSpellCritMultiplier(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalSpellCritMilti";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE GLOBAL spell critical hit multiplier";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreSpellCritMultiplier(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreSpellCritMultiplier(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Mana---
            m = new();
            m.Name = "FlatGlobalMana";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]} mana";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.AddFlatMana((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.AddFlatMana(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalMana";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase mana by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseMana(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseMana(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalMana";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE mana";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreMana(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreMana(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Mana-Regen---
            m = new();
            m.Name = "IncreaseGlobalManaRegen";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase mana regen by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseManaRegeneration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseManaRegeneration(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalManaRegen";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% MORE mana regen";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreManaRegeneration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreManaRegeneration(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Manacost---
            m = new();
            m.Name = "ReduceGlobalManacost";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Reduce cost of skills by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseManacost(-m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseManacost(m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "LessGlobalManacost";
            m.IsLocal = false;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% LESS mana cost of skills";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.LessManacost(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.LessManacost(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Cooldown---
            m = new();
            m.Name = "IncreaseGlobalCDR";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Cooldown;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL cooldown reduction by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseCooldownReduction(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.IncreaseCooldownReduction(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalCDR";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Cooldown;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% MORE GLOBAL cooldown reduction";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreCooldownReduction(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.MoreCooldownReduction(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Projectile---
            m = new();
            m.Name = "FlatGlobalSpellProjectiles";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Projectile;
            m.TierValues = new float[] { 4, 4, 3, 3, 3, 2, 2, 2, 1, 1 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]} spell projectiles";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.AddFlatSpellProjectileAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.MagicSC.AddFlatSpellProjectileAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}
