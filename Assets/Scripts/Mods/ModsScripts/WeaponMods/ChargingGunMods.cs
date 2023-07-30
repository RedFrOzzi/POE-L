using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class ChargingGunMods
    {
        public static List<ModForWeapon> GetWeaponMods()
        {
            List<ModForWeapon> mods = new();

            ModForWeapon m = new();

            //---Damage---
            m.Name = "IncreaseLocalChargeRate";
            m.IsLocal = true;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL charging rate by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseChargeRate(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseChargeRate(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseLocalMaxCharge";
            m.IsLocal = true;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL maximum charge by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseMaxCharge(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseMaxCharge(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseLocalChargeDamage";
            m.IsLocal = true;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL charge to damage modifier by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseChargeDamageModifier(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseChargeDamageModifier(-m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}
            