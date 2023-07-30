namespace Database
{
    public class TalentPistolSpecialist_L : TProp
    {
        private CH_Stats playerStats;

        public TalentPistolSpecialist_L()
        {
            Name = "Legendary Pistol Specialist";
            Description = "While Pistol is your primary weapon, give 10% MORE attack damage, 30% LESS bullet spread," +
                " 20% MORE critical strike chance and 50% MORE critical strike multiplier.\nBullets pierce 4 additional targets." +
                "\nBullets chain 4 additional times";
            TalentRarity = Rarity.Legendary;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(10);
                    stats.GSC.AttackSC.LessSpreadAngle(30);
                    stats.GSC.AttackSC.MoreAttackCritChance(20);
                    stats.GSC.AttackSC.MoreAttackCritMultiplier(50);
                    stats.GSC.AttackSC.AddFlatWeaponPierceAmount(4);
                    stats.GSC.AttackSC.AddFlatWeaponChainAmount(4);
                }

                stats.Equipment.OnEquipmentChange += OnEquipmentChange;
            };

            OnLock = (stats) =>
            {
                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.MoreAttackDamage(-10);
                    stats.GSC.AttackSC.LessSpreadAngle(-30);
                    stats.GSC.AttackSC.MoreAttackCritChance(-20);
                    stats.GSC.AttackSC.MoreAttackCritMultiplier(-50);
                    stats.GSC.AttackSC.AddFlatWeaponPierceAmount(-4);
                    stats.GSC.AttackSC.AddFlatWeaponChainAmount(-4);
                }

                stats.Equipment.OnEquipmentChange -= OnEquipmentChange;
            };
        }
        

        private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
        {
            if (oldItem is Revolver || oldItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(-10);
                playerStats.GSC.AttackSC.LessSpreadAngle(-30);
                playerStats.GSC.AttackSC.MoreAttackCritChance(-20);
                playerStats.GSC.AttackSC.MoreAttackCritMultiplier(-50);
                playerStats.GSC.AttackSC.AddFlatWeaponPierceAmount(-4);
                playerStats.GSC.AttackSC.AddFlatWeaponChainAmount(-4);
            }

            if (newItem is Revolver || newItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.MoreAttackDamage(10);
                playerStats.GSC.AttackSC.LessSpreadAngle(30);
                playerStats.GSC.AttackSC.MoreAttackCritChance(20);
                playerStats.GSC.AttackSC.MoreAttackCritMultiplier(50);
                playerStats.GSC.AttackSC.AddFlatWeaponPierceAmount(4);
                playerStats.GSC.AttackSC.AddFlatWeaponChainAmount(4);
            }
        }
    }
}