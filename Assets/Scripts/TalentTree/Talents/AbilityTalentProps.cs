using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace Database
{
    public class AbilityTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new();

            TProp p = new();

            p.Name = "Common Spell Damage";
            p.Description = "Increase spell damage by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseSpellDamage(5);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseSpellDamage(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Spell Damage";
            p.Description = "Increase spell damage by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseSpellDamage(10);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseSpellDamage(-10);
            props.Add(p);

            return props;
        }
    }
}