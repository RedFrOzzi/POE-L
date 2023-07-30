using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class SpecialTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new()
            {
                new TalentProjAbilDamage_C(),
                new TalentAOEAbilDamage_C(),
                new TalentPistolAttackDamage_E(),
                new TalentPistolBoost_R(),
                new TalentPistolChain_E(),
                new TalentPistolLaserSight_R(),
                new TalentPistolSpecialist_E(),
                new TalentPistolSpecialist_L(),
                new TalentPistolSpecialist_M(),
            };

            return props;
        }
    }
}
