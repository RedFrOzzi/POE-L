using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class GlobalTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new();

            TProp p = new();

            p.Name = "Uncommon Damage";
            p.Description = "Increase all damage by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseDamage(5);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseDamage(-5);
            props.Add(p);

            p = new();
            p.Name = "Rare Damage";
            p.Description = "Increase all damage by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseDamage(10);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseDamage(-10);
            props.Add(p);

            p = new();
            p.Name = "Epic Damage";
            p.Description = "Increase all damage by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Epic;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseDamage(15);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseDamage(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Physical Damage";
            p.Description = "Increase physical damage by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreasePhysicalDamage(5);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreasePhysicalDamage(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Physical Damage";
            p.Description = "Increase physical damage by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreasePhysicalDamage(10);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreasePhysicalDamage(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Physical Damage";
            p.Description = "Increase physical damage by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreasePhysicalDamage(15);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreasePhysicalDamage(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Magic Damage";
            p.Description = "Increase magic damage by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseMagicDamage(5);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseMagicDamage(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Magic Damage";
            p.Description = "Increase magic damage by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseMagicDamage(10);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseMagicDamage(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Magic Damage";
            p.Description = "Increase magic damage by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseMagicDamage(15);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseMagicDamage(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Projectile Speed";
            p.Description = "Increase projectile speed by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.UtilitySC.IncreaseProjectileSpeed(10);
            p.OnLock = (stats) => stats.GSC.UtilitySC.IncreaseProjectileSpeed(-10);
            props.Add(p);

            p = new();
            p.Name = "Common Area Damage";
            p.Description = "Increase area damage by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseAreaDamage(5);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseAreaDamage(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Area Damage";
            p.Description = "Increase area damage by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseAreaDamage(10);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseAreaDamage(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Area Damage";
            p.Description = "Increase area damage by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.GlobalSC.IncreaseAreaDamage(15);
            p.OnLock = (stats) => stats.GSC.GlobalSC.IncreaseAreaDamage(-15);
            props.Add(p);

            return props;
        }
    }
}