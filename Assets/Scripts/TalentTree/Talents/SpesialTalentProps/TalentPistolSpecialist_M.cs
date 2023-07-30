namespace Database
{
    public class TalentPistolSpecialist_M : TProp
    {
        private CH_Stats playerStats;

        public TalentPistolSpecialist_M()
        {
            Name = "Mythic Pistol Specialist";
            Description = $"While Pistol is your primary weapon, give 20% MORE attack damage, 30 to ammo capacity, 5% to BASE crit chance and 50 to crit multiplier.";
            TalentRarity = Rarity.Mythic;
            OnUnlock = (stats) =>
            {
                playerStats = stats;

                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(20);
                    stats.GSC.AttackSC.AddFlatAmmoCapacity(30);
                    stats.GSC.AttackSC.AddFlatAttackCritChance(5);
                    stats.GSC.AttackSC.IncreaseAttackCritMultiplier(50);
                }

                stats.Equipment.OnEquipmentChange += OnEquipmentChange;
            };

            OnLock = (stats) =>
            {
                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(-20);
                    stats.GSC.AttackSC.AddFlatAmmoCapacity(-30);
                    stats.GSC.AttackSC.AddFlatAttackCritChance(-5);
                    stats.GSC.AttackSC.IncreaseAttackCritMultiplier(-50);
                }

                stats.Equipment.OnEquipmentChange -= OnEquipmentChange;
            };
        }

        private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
        {
            if (oldItem is Revolver || oldItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(-20);
                playerStats.GSC.AttackSC.AddFlatAmmoCapacity(-30);
                playerStats.GSC.AttackSC.AddFlatAttackCritChance(-5);
                playerStats.GSC.AttackSC.IncreaseAttackCritMultiplier(-50);
            }

            if (newItem is Revolver || newItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(20);
                playerStats.GSC.AttackSC.AddFlatAmmoCapacity(30);
                playerStats.GSC.AttackSC.AddFlatAttackCritChance(5);
                playerStats.GSC.AttackSC.IncreaseAttackCritMultiplier(50);
            }
        }
    }
}