namespace Database
{
    public class TalentPistolAttackDamage_E : TProp
    {
        private CH_Stats playerStats;

        public TalentPistolAttackDamage_E()
        {
            Name = "Pistol Epic Boost";
            Description = $"Give 20% MORE attack damage and 3 additional projectile, if Pistol is your primary weapon";
            TalentRarity = Rarity.Epic;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(20);
                    stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(3);
                }

                stats.Equipment.OnEquipmentChange += OnEquipmentChange;
            };

            OnLock = (stats) =>
            {
                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(-20);
                    stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-3);
                }

                stats.Equipment.OnEquipmentChange -= OnEquipmentChange;
            };
        }

        private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
        {
            if (oldItem is Revolver || oldItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(-20);
                playerStats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-3);
            }

            if (newItem is Revolver || newItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(20);
                playerStats.GSC.AttackSC.AddFlatWeaponProjectileAmount(3);
            }
        }
    }
}