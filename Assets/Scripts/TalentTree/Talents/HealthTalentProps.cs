using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class HealthTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new();

            TProp p = new();

            p.Name = "Common Health";
            p.Description = "Increase health by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseHP(5);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseHP(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Health";
            p.Description = "Increase health by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseHP(10);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseHP(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Health";
            p.Description = "Increase health by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseHP(15);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseHP(-15);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Health Regeneration";
            p.Description = "Increase health regeneration by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.DefanceSC.IncreaseHPRegeneration(5);
            p.OnLock = (stats) => stats.GSC.DefanceSC.IncreaseHPRegeneration(-5);
            props.Add(p);

            p = new();
            p.Name = "Epic Massiveness";
            p.Description = "Adds 20 to health.\nIncrease health by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) =>
            {
                stats.GSC.DefanceSC.AddFlatHP(20);
                stats.GSC.DefanceSC.IncreaseHP(10);
            };

            p.OnLock = (stats) =>
            {
                stats.GSC.DefanceSC.AddFlatHP(-20);
                stats.GSC.DefanceSC.IncreaseHP(-10);
            };

            props.Add(p);

            return props;
        }
    }
}