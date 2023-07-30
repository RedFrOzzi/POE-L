using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class WeaponMods
    {
        public static List<ModForWeapon> GetWeaponMods()
        {
            List<ModForWeapon> mods = new();

            ModForWeapon m = new();

            //---Damage---
            m.Name = "IncreaseLocalDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Attack;
            m.ModTags[2] = ModTag.Physical;
            m.TierValues = new float[] { 200, 180, 160, 140, 120, 100, 80, 60, 40, 20 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL damage by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalDamage";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Damage;
            m.ModTags[1] = ModTag.Attack;
            m.ModTags[2] = ModTag.Physical;
            m.TierValues = new float[] { 100, 95, 80, 60, 40, 20, 15, 10, 5, 2, 1 };
            m.TierWeights = new float[] { 100, 150, 200, 250, 300, 350, 400, 450, 550, 650, 800 };
            m.Description = (m) => $"Add flat {m.TierValues[m.Tier]} to base damage";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatAttackDamage((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatAttackDamage(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "LocalHybridAttackDamageSpread";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Damage;
            m.ModTags[2] = ModTag.Utility;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase LOCAL attack damage by {m.TierValues[m.Tier]}% and reduce LOCAL bullet spread by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseSpreadAngle(-m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(-m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseSpreadAngle(m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            m = new();
            m.Name = "LocalHybridAttackDamageProjSpeed";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Damage;
            m.ModTags[2] = ModTag.Utility;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase LOCAL attack damage by {m.TierValues[m.Tier]}% and increase LOCAL projectile speed by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.UtilitySC.IncreaseProjectileSpeed(m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(-m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.UtilitySC.IncreaseProjectileSpeed(-m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            m = new();
            m.Name = "LocalHybridAttackDamageAmmo";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Damage;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 25, 22, 20, 18, 15, 12, 10, 8, 5, 2 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase LOCAL attack damage by {m.TierValues[m.Tier]}% and increase LOCAL ammo capacity by {m.TierValues2[m.Tier]}";
            m.ApplyMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatAmmoCapacity((int)m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(-m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatAmmoCapacity(-(int)m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            //---Attack-Speed---
            m = new();
            m.Name = "IncreaseLocalAttackSpeed";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Speed;
            m.ModTags[1] = ModTag.Attack;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL attack speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "LocalHybridAttackSpeedReloadSpeed";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Attack;
            m.ModTags[1] = ModTag.Speed;
            m.ModTags[2] = ModTag.Utility;
            m.TierValues = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierValues2 = new float[] { 27, 25, 22, 20, 17, 15, 12, 10, 7, 5 };
            m.TierWeights = new float[] { 50, 100, 200, 250, 250, 300, 400, 450, 500, 500 };
            m.Description = (m) => $"Increase LOCAL attack damage by {m.TierValues[m.Tier]}% and reduce LOCAL reload speed by {m.TierValues2[m.Tier]}%";
            m.ApplyMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseReloadSpeed(-m.TierValues2[m.Tier]);
            };
            m.RemoveMod = (m) =>
            {
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackDamage(-m.TierValues[m.Tier]);
                (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseReloadSpeed(m.TierValues2[m.Tier]);
            };
            mods.Add(m);

            //---Crit---
            m = new();
            m.Name = "IncreaseLocalCritChance";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Critical;
            m.ModTags[1] = ModTag.Attack;
            m.TierValues = new float[] { 100, 95, 80, 60, 40, 20, 15, 10 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL crit chance by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackCritChance(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackCritChance(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalCritMultiplier";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Critical;
            m.TierValues = new float[] { 200, 180, 160, 140, 120, 100, 80, 60, 40, 20, 10 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 700, 800 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]}% to base critical strike multiplier";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.GlobalSC.AddFlatCritMultiplier((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.GlobalSC.AddFlatCritMultiplier(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            //---Reload---
            m = new();
            m.Name = "IncreaseLocalReloadSpeed";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Speed;
            m.ModTags[1] = ModTag.Reload;
            m.TierValues = new float[] { 100, 80, 70, 60, 40, 20, 15, 10 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL reload speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseReloadSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseReloadSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Ammo---
            m = new();
            m.Name = "FlatLocalAmmoCapacity";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 20, 18, 15, 12, 10, 8, 5, 2 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600 };
            m.Description = (m) => $"Add flat +{m.TierValues[m.Tier]} to base ammo capacity";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatAmmoCapacity((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatAmmoCapacity(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            //---Spread---
            m = new();
            m.Name = "ReduceLocalSpreadAngle";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 40, 38, 35, 28, 25, 20, 15, 10, 8, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Reduce base spread angle by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseSpreadAngle(-m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseSpreadAngle(m.TierValues[m.Tier]);
            mods.Add(m);

            //---Range---
            m = new();
            m.Name = "IncreaseLocalRange";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 20, 18, 15, 12, 10, 8, 5, 2 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600 };
            m.Description = (m) => $"Reduce base attack range by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackRange(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseAttackRange(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Proj-Speed---
            m = new();
            m.Name = "IncreaseLocalProjectileSpeed";
            m.IsLocal = true;
            m.ModTags[0] = ModTag.Utility;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Increase base projectile speed by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.UtilitySC.IncreaseProjectileSpeed(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.UtilitySC.IncreaseProjectileSpeed(-m.TierValues[m.Tier]);
            mods.Add(m);

            //---Projectile---
            m = new();
            m.Name = "FlatLocalAttackProjectileAmount";
            m.IsLocal = true;
            m.ModTags[1] = ModTag.Attack;
            m.ModTags[0] = ModTag.Projectile;
            m.TierValues = new float[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Add +{m.TierValues[m.Tier]} to wapon base projectiles";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatWeaponProjectileAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatWeaponProjectileAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalChainsAmount";
            m.IsLocal = true;
            m.ModTags[1] = ModTag.Attack;
            m.ModTags[0] = ModTag.Projectile;
            m.TierValues = new float[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Weapon bullets chains +{m.TierValues[m.Tier]} times";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatWeaponChainAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatWeaponChainAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "FlatLocalPierceAmount";
            m.IsLocal = true;
            m.ModTags[1] = ModTag.Attack;
            m.ModTags[0] = ModTag.Projectile;
            m.TierValues = new float[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 500, 600, 600, 600, 600, 600 };
            m.Description = (m) => $"Weapon bullets pierce +{m.TierValues[m.Tier]} times";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatWeaponPierceAmount((int)m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.AddFlatWeaponPierceAmount(-(int)m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}