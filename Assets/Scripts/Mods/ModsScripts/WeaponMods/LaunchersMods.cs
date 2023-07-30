using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class LaunchersMods
    {
        public static List<ModForWeapon> GetWeaponMods()
        {
            List<ModForWeapon> mods = new();

            ModForWeapon m = new();

            m.Name = "IncreaseLocalExplosionRadius";
            m.IsLocal = true;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Increase LOCAL explosion radius by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseExplosionRadius(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseExplosionRadius(-m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseLocalTimeToReachTarget";
            m.IsLocal = true;
            m.TierValues = new float[] { 50, 45, 40, 35, 30, 25, 20, 15, 10, 5 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"Decrease time for projectile to reach target by {m.TierValues[m.Tier]}%";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseTimeToReach(-m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreaseTimeToReach(m.TierValues[m.Tier]);
            mods.Add(m);

            m = new();
            m.Name = "IncreaseLocalPercentDamageToExplode";
            m.IsLocal = true;
            m.TierValues = new float[] { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
            m.TierWeights = new float[] { 100, 200, 300, 400, 400, 500, 500, 600, 600, 600 };
            m.Description = (m) => $"+{m.TierValues[m.Tier]}% increase percentage of damage to explosion";
            m.ApplyMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreasePercentOfDamageToExplosion(m.TierValues[m.Tier]);
            m.RemoveMod = (m) => (m as ModForWeapon).WeaponItem.LSC.AttackSC.IncreasePercentOfDamageToExplosion(-m.TierValues[m.Tier]);
            mods.Add(m);

            return mods;
        }
    }
}
            