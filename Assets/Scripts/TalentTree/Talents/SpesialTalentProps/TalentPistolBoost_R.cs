namespace Database
{
    public class TalentPistolBoost_R : TProp
	{
        private CH_Stats playerStats;

        public TalentPistolBoost_R()
        {
            Name = "Rare Pistol Boost";
            Description = "Give 10% MORE attack damage and 1 additional projectile, if Pistol is your primary weapon";
            TalentRarity = Rarity.Rare;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(10);
                    stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(1);
                }

                stats.Equipment.OnEquipmentChange += OnEquipmentChange;
            };

            OnLock = (stats) =>
            {
                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(-10);
                    stats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-1);
                }

                stats.Equipment.OnEquipmentChange -= OnEquipmentChange;
            };
        }

        private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
        {
            if (oldItem is Revolver || oldItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.IncreaseAttackDamage(-10);
                playerStats.GSC.AttackSC.AddFlatWeaponProjectileAmount(-1);
            }

            if (newItem is Revolver || newItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.IncreaseAttackDamage(10);
                playerStats.GSC.AttackSC.AddFlatWeaponProjectileAmount(1);
            }
        }
    }
}