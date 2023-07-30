namespace Database
{
    public class TalentPistolLaserSight_R : TProp
    {
        private readonly float flatCritChance = 1.5f;
        private readonly float increaseCritChance = 10;
        private readonly float increaseCritMulti = 15;
        private readonly float reducesSpreadAngle = 20;
        private readonly int additionalProjectiles = 1;
        private CH_Stats playerStats;

        public TalentPistolLaserSight_R()
        {
            Name = "Rare Pistol Sight";
            Description = "Give 1.5% to BASE crit chance, 10% increase crit chance, 15% increase crit multiplier,\n20% reduced spread angle and 1 additional projectile.";
            TalentRarity = Rarity.Rare;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.AddFlatAttackCritChance(flatCritChance);
                    stats.GSC.AttackSC.IncreaseAttackCritChance(increaseCritChance);
                    stats.GSC.AttackSC.IncreaseAttackCritMultiplier(increaseCritMulti);
                    stats.GSC.AttackSC.IncreaseSpreadAngle(-reducesSpreadAngle);
                    stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(additionalProjectiles);
                }

                stats.Equipment.OnEquipmentChange += OnEquipmentChange;
            };

            OnLock = (stats) =>
            {
                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.AddFlatAttackCritChance(flatCritChance);
                    stats.GSC.AttackSC.IncreaseAttackCritChance(increaseCritChance);
                    stats.GSC.AttackSC.IncreaseAttackCritMultiplier(increaseCritMulti);
                    stats.GSC.AttackSC.IncreaseSpreadAngle(-reducesSpreadAngle);
                    stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(additionalProjectiles);
                }

                stats.Equipment.OnEquipmentChange -= OnEquipmentChange;
            };
        }

        private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
        {
            if (oldItem is Revolver || oldItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.AddFlatAttackCritChance(-flatCritChance);
                playerStats.GSC.AttackSC.IncreaseAttackCritChance(-increaseCritChance);
                playerStats.GSC.AttackSC.IncreaseAttackCritMultiplier(-increaseCritMulti);
                playerStats.GSC.AttackSC.IncreaseSpreadAngle(reducesSpreadAngle);
                playerStats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-additionalProjectiles);
            }

            if (newItem is Revolver || newItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.AddFlatAttackCritChance(flatCritChance);
                playerStats.GSC.AttackSC.IncreaseAttackCritChance(increaseCritChance);
                playerStats.GSC.AttackSC.IncreaseAttackCritMultiplier(increaseCritMulti);
                playerStats.GSC.AttackSC.IncreaseSpreadAngle(-reducesSpreadAngle);
                playerStats.GSC.AttackSC.AddFlatWeaponProjectileAmount(additionalProjectiles);
            }
        }
    }
}