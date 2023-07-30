using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class DefanseTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new();

            TProp p = new();

            p.Name = "Common Armor";
            p.Description = "Increase armor by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseArmor(5);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseArmor(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Armor";
            p.Description = "Increase armor by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseArmor(10);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseArmor(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Armor";
            p.Description = "Increase armor by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseArmor(15);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseArmor(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Magic Resist";
            p.Description = "Increase magic resist by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseMagicResist(5);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseMagicResist(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Magic Resist";
            p.Description = "Increase magic resist by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseMagicResist(10);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseMagicResist(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Magic Resist";
            p.Description = "Increase magic resist by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseMagicResist(15);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseMagicResist(-15);
            props.Add(p);

            return props;
        }
    }
}