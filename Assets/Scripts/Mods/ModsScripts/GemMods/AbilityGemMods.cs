using UnityEngine;
using System.Collections.Generic;

namespace Database
{
    public class AbilityGemMods
    {
        public static List<ModForAbilityGem> GetAbilityGemMods()
        {
            List<ModForAbilityGem> mods = new();

            ModForAbilityGem m = new();

            //---Damage---
            m.Name = "IncreaseLocalSpellDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Spell;
            m.TierValues = new float[] { 200, 180, 160, 140, 120, 100, 80, 60, 40, 20 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL spell damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseSpellDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseSpellDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreLocalSpellDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Spell;
            m.TierValues = new float[] { 30, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 500, 600, 600, 700, 800 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE LOCAL spell damage";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.MoreSpellDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.MoreSpellDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "DecreaseLocalSpellDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Spell;
            m.TierValues = new float[] { 20, 40, 60, 140, 80, 100, 120, 160, 180, 200 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 500, 600, 600, 700, 800 };
            m.Description = (m) => $"Decrease LOCAL spell damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseSpellDamage(-m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseSpellDamage(m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "LessLocalSpellDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Spell;
            m.TierValues = new float[] { 5, 7, 10, 12, 15, 17, 20, 22, 25, 30 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 500, 600, 600, 700, 800 };
            m.Description = (m) => $"Spell deals {m.TierValues[m.Tier]}% LESS damage";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.LessSpellDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.LessSpellDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseLocalMagicDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Magic;
            m.TierValues = new float[] { 200, 180, 160, 140, 120, 100, 80, 60, 40, 20 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL magic damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.GlobalSC.IncreaseMagicDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.GlobalSC.IncreaseMagicDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseLocalPhysicalDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Magic;
            m.TierValues = new float[] { 200, 180, 160, 140, 120, 100, 80, 60, 40, 20 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL physical damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.GlobalSC.IncreasePhysicalDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.GlobalSC.IncreasePhysicalDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Crit---
            m = new();
            m.Name = "IncreaseLocalSpellCritChance";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Increase LOCAL spell critical hit chance by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseSpellCritChance(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseSpellCritChance(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalSpellCritMulti";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]}% LOCAL spell critical hit multiplier";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.AddFlatSpellCritMultiplier((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.AddFlatSpellCritMultiplier(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            //---Cooldown---
            m = new();
            m.Name = "IncreaseLocalCooldownRecovery";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Utility;
            m.ModTags[1] = ModTag.Cooldown;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Increase LOCAL cooldown reduction by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseCooldownReduction(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseCooldownReduction(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "DecreaseLocalCooldownRecovery";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Utility;
            m.ModTags[1] = ModTag.Cooldown;
            m.TierValues = new float[] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Decrease LOCAL cooldown reduction by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseCooldownReduction(-m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseCooldownReduction(m.TierValues[m.Tier]);
            mods.Add(m);

            //---Level---
            m = new();
            m.Name = "FlatLocalSpellLevel";
            m.IsLocal = true;
            m.TierValues = new float[] { 5, 5, 4, 4, 3, 3, 2, 2, 1, 1 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to ability level";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.AddFlatAbilityLevel((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.AddFlatAbilityLevel(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            //---Manacost---
            m = new();
            m.Name = "ReduceLocalManacost";
            m.IsLocal = true;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Reduce LOCAL manacost by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseManacost(-m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.IncreaseManacost(m.TierValues[m.Tier]);
            mods.Add(m);

            //---Area---
            m = new();
            m.Name = "IncreaseLocalArea";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Area;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Increase LOCAL area of effect by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.UtilitySC.IncreaseArea(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.UtilitySC.IncreaseArea(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Duration---
            m = new();
            m.Name = "IncreaseLocalDuration";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Area;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Increase LOCAL duration by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.UtilitySC.IncreaseEffectDuration(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.UtilitySC.IncreaseEffectDuration(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Projectile---
            m = new();
            m.Name = "FlatLocalSpellProjectileAmount";
            m.IsLocal = true;
            m.TierValues = new float[] { 5, 5, 4, 4, 3, 3, 2, 2, 1, 1 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 700 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to spell projectiles";
            m.ApplyMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.AddFlatSpellProjectileAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForAbilityGem).AbilityGem.LSC.MagicSC.AddFlatSpellProjectileAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}
