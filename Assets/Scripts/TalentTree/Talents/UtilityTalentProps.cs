using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class UtilityTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new();

            TProp p = new();

            p.Name = "Common Movement Speed";
            p.Description = "Increase monvement speed by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseMovementSpeed(5);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseMovementSpeed(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Movement Speed";
            p.Description = "Increase monvement speed by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseMovementSpeed(10);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseMovementSpeed(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Movement Speed";
            p.Description = "Increase monvement speed by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseMovementSpeed(15);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseMovementSpeed(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Area Of Effect";
            p.Description = "Increase area of effect by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseArea(5);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseArea(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Area Of Effect";
            p.Description = "Increase area of effect by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseArea(10);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseArea(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Area Of Effect";
            p.Description = "Increase area of effect by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseArea(15);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseArea(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Effect Duration";
            p.Description = "Increase duration of effects by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseEffectDuration(5);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseEffectDuration(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Effect Duration";
            p.Description = "Increase duration of effects by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseEffectDuration(10);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseEffectDuration(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Effect Duration";
            p.Description = "Increase duration of effects by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseEffectDuration(15);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseEffectDuration(-15);
            props.Add(p);

            return props;
        }
    }
}
