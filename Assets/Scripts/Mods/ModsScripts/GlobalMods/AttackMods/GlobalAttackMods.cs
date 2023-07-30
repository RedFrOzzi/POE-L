using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class GlobalAttackMods
    {
        public static List<ModForGlobalStats> GetGlobalMods()
        {
            List<ModForGlobalStats> mods = new();

            ModForGlobalStats m = new();

            //---Damage---
            m.Name = "IncreaseGlobalAttackDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Damage;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL attack damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalAttackDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Damage;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE GLOBAL attack damage";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "LessGlobalAttackDamage";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Damage;
            m.TierValues = new float[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% LESS GLOBAL attack damage";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.LessAttackDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.LessAttackDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Attack-Speed---
            m = new();
            m.Name = "IncreaseGlobalAttackSpeed";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Speed;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL attack speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalAttackSpeed";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Speed;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE GLOBAL attack speed";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Crit---
            m = new();
            m.Name = "IncreaseGlobalAttackCritChance";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL attack critical hit chance by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackCritChance(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackCritChance(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalAttackCritChance";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE GLOBAL attack critical hit chance";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackCritChance(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackCritChance(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatGlobalAttackCritMulti";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]}% GLOBAL attack crit multiplier";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatAttackCritMultiplier((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatAttackCritMultiplier(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalAttackCritMulti";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Critical;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE GLOBAL attack critical hit multiplier";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackCritMultiplier(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackCritMultiplier(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Reload---
            m = new();
            m.Name = "IncreaseGlobalReloadSpeed";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Reload;
            m.ModTags[1] = ModTag.Speed;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL reload speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseReloadSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseReloadSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalReloadSpeed";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Reload;
            m.ModTags[1] = ModTag.Speed;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE GLOBAL reload speed";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreReloadSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreReloadSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Ammo---
            m = new();
            m.Name = "FlatGlobalAmmoCapacity";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 25, 22, 20, 17, 15, 12, 10, 7, 5, 2 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add {m.TierValues[m.Tier]} to GLOBAL ammo capacity";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAmmoCapacity(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAmmoCapacity(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseGlobalAmmoCapacity";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL ammo capacity by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAmmoCapacity(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAmmoCapacity(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalAmmoCapacity";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE GLOBAL ammo capacity";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAmmoCapacity(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAmmoCapacity(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Range---
            m = new();
            m.Name = "IncreaseGlobalAttackRange";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase GLOBAL attack range by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackRange(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackRange(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "MoreGlobalAttackRange";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Give +{m.TierValues[m.Tier]}% MORE GLOBAL attack range";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackRange(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.MoreAttackRange(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Spread---
            m = new();
            m.Name = "ReduceGlobalBulletSpread";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Reduce GLOBAL bullet spread by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseSpreadAngle(-m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.IncreaseAttackRange(m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "LessGlobalBulletSpread";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"{m.TierValues[m.Tier]}% LESS GLOBAL bullet spread";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.LessSpreadAngle(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.LessSpreadAngle(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Proj---
            m = new();
            m.Name = "FlatGlobalAttackProjectileAmount";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Projectile;
            m.TierValues = new float[] { 4, 4, 3, 3, 3, 2, 2, 2, 1, 1 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} GLOBAL attack projectiles";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatWeaponProjectileAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatGlobalAttackPierceAmount";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Projectile;
            m.ModTags[0] = ModTag.Pierce;
            m.TierValues = new float[] { 4, 4, 3, 3, 3, 2, 2, 2, 1, 1 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} GLOBAL attack projectiles";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatWeaponPierceAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatWeaponPierceAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatGlobalAttackChainsAmount";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.Projectile;
            m.ModTags[0] = ModTag.Chain;
            m.TierValues = new float[] { 4, 4, 3, 3, 3, 2, 2, 2, 1, 1 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} GLOBAL attack projectiles";
            m.ApplyMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatWeaponChainAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForGlobalStats).Stats.GSC.AttackSC.AddFlatWeaponChainAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}