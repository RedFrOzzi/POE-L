using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class MagicTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new();

            TProp p = new();

            p.Name = "Common Cooldown Reduction";
            p.Description = "Increase cooldown reduction by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseCooldownReduction(5);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseCooldownReduction(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Cooldown Reduction";
            p.Description = "Increase cooldown reduction by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseCooldownReduction(10);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseCooldownReduction(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Cooldown Reduction";
            p.Description = "Increase cooldown reduction by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseCooldownReduction(15);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseCooldownReduction(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Mana";
            p.Description = "Increase mana by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseMana(5);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseMana(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Mana";
            p.Description = "Increase mana by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseMana(10);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseMana(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Mana";
            p.Description = "Increase mana by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseMana(15);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseMana(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Mana Regeneration";
            p.Description = "Increase mana regeneration by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseManaRegeneration(5);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseManaRegeneration(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Mana Regeneration";
            p.Description = "Increase mana regeneration by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseManaRegeneration(10);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseManaRegeneration(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Mana Regeneration";
            p.Description = "Increase mana regeneration by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.MagicSC.IncreaseManaRegeneration(15);
            p.OnLock = (stats) => stats.GSC.MagicSC.IncreaseManaRegeneration(-15);
            props.Add(p);

            return props;
        }
    }
}