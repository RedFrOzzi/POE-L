namespace Database
{
    public class TalentPistolSpecialist_E : TProp
    {
        private CH_Stats playerStats;

        public TalentPistolSpecialist_E()
        {
            Name = "Epic Pistol Specialist";
            Description = $"While Pistol is your primary weapon, give 10% MORE attack damage, 10 to ammo capacity and 20% MORE reload speed.";
            TalentRarity = Rarity.Epic;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(10);
                    stats.GSC.AttackSC.AddFlatAmmoCapacity(10);
                    stats.GSC.AttackSC.MoreReloadSpeed(20);
                }

                stats.Equipment.OnEquipmentChange += OnEquipmentChange;
            };

            OnLock = (stats) =>
            {
                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(-10);
                    stats.GSC.AttackSC.AddFlatAmmoCapacity(-10);
                    stats.GSC.AttackSC.MoreReloadSpeed(-20);
                }

                stats.Equipment.OnEquipmentChange -= OnEquipmentChange;
            };
        }
        
        private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
        {
            if (oldItem is Revolver || oldItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(-10);
                playerStats.GSC.AttackSC.AddFlatAmmoCapacity(-10);
                playerStats.GSC.AttackSC.MoreReloadSpeed(-20);
            }

            if (newItem is Revolver || newItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(10);
                playerStats.GSC.AttackSC.AddFlatAmmoCapacity(10);
                playerStats.GSC.AttackSC.MoreReloadSpeed(20);
            }
        }
    }
}