using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    public class AttackTalentProps
    {
        public static List<TProp> GetTalentProps()
        {
            List<TProp> props = new();

            TProp p = new();

            p.Name = "Common Attack Speed";
            p.Description = "Increase attack speed by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackSpeed(5);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackSpeed(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Attack Speed";
            p.Description = "Increase attack speed by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackSpeed(10);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackSpeed(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Attack Speed";
            p.Description = "Increase attack speed by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackSpeed(15);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackSpeed(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Attack Damage";
            p.Description = "Increase attack damage by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackDamage(5);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackDamage(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Attack Damage";
            p.Description = "Increase attack damage by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackDamage(10);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackDamage(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Attack Damage";
            p.Description = "Increase attack damage by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackDamage(15);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackDamage(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Attack Crit Chance";
            p.Description = "Increase weapon crit chance by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackCritChance(5);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackCritChance(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Attack Crit Chance";
            p.Description = "Increase weapon crit chance by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackCritChance(10);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackCritChance(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Attack Crit Chance";
            p.Description = "Increase weapon crit chance by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackCritChance(15);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackCritChance(-15);
            props.Add(p);

            p = new();
            p.Name = "Epic Flat Attack Crit Chance";
            p.Description = "Add 2% BASE crit chance to weapon";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Epic;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritChance(2);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritChance(-2);
            props.Add(p);

            p = new();
            p.Name = "Legendary Flat Attack Crit Chance";
            p.Description = "Add 4% BASE crit chance to weapon";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Legendary;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritChance(4);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritChance(-4);
            props.Add(p);

            p = new();
            p.Name = "Common Bullet Spread Angle";
            p.Description = "Decrease weapon bullet spread angle by 7%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseSpreadAngle(-7);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseSpreadAngle(7);
            props.Add(p);

            p = new();
            p.Name = "Rare Bullet Spread Angle";
            p.Description = "Decrease weapon bullet spread angle by 7%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseSpreadAngle(-20);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseSpreadAngle(20);
            props.Add(p);

            p = new();
            p.Name = "Common Reload Speed";
            p.Description = "Increase reload speed by 5%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseReloadSpeed(5);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseReloadSpeed(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Reload Speed";
            p.Description = "Increase reload speed by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseReloadSpeed(10);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseReloadSpeed(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Reload Speed";
            p.Description = "Increase reload speed by 15%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseReloadSpeed(15);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseReloadSpeed(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Attack Crit Multiplier";
            p.Description = "Add 5% to attack crit multiplier";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritMultiplier(5);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritMultiplier(-5);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Attack Crit Multiplier";
            p.Description = "Add 10% to attack crit multiplier";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritMultiplier(10);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritMultiplier(-10);
            props.Add(p);

            p = new();
            p.Name = "Rare Attack Crit Multiplier";
            p.Description = "Add 15% to attack crit multiplier";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritMultiplier(15);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatAttackCritMultiplier(-15);
            props.Add(p);

            p = new();
            p.Name = "Common Attack Range";
            p.Description = "Increase attack range by 10%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Common;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackRange(10);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackRange(-10);
            props.Add(p);

            p = new();
            p.Name = "Uncommon Attack Range";
            p.Description = "Increase attack range by 20%";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Uncommon;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.IncreaseAttackRange(20);
            p.OnLock = (stats) => stats.GSC.AttackSC.IncreaseAttackRange(-20);
            props.Add(p);

            p = new();
            p.Name = "Epic Additional Weapon Projectile";
            p.Description = "Add +1 projectiles to weapon";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Epic;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(1);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-1);
            props.Add(p);

            p = new();
            p.Name = "Legendary Additional Weapon Projectile";
            p.Description = "Add +3 projectiles to weapon";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Legendary;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(3);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-3);
            props.Add(p);

            p = new();
            p.Name = "Rare Additional Weapon Pierce";
            p.Description = "Bullets pierce 1 additional targets";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatWeaponPierceAmount(1);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatWeaponPierceAmount(-1);
            props.Add(p);

            p = new();
            p.Name = "Epic Additional Weapon Pierce";
            p.Description = "Bullets pierce 3 additional targets";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Epic;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatWeaponPierceAmount(3);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatWeaponPierceAmount(-3);
            props.Add(p);

            p = new();
            p.Name = "Rare Additional Weapon Chain";
            p.Description = "Bullets chain 1 additional targets";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Rare;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatWeaponChainAmount(1);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatWeaponChainAmount(-1);
            props.Add(p);

            p = new();
            p.Name = "Epic Additional Weapon Chain";
            p.Description = "Bullets chain 1 additional targets";
            p.SpriteName = "QuickFire";
            p.TalentRarity = Rarity.Epic;
            p.OnUnlock = (stats) => stats.GSC.AttackSC.AddFlatWeaponChainAmount(3);
            p.OnLock = (stats) => stats.GSC.AttackSC.AddFlatWeaponChainAmount(-3);
            props.Add(p);

            return props;
        }
    }
}