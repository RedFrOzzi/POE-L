using UnityEngine;

namespace Database
{

    public class TalentPistolChain_E : TProp
    {
        private readonly int additionalChains = 3;
        private readonly float increaseAttackDamage = 10;
        private readonly float increaseProjSpeed = 20;
        private CH_Stats playerStats;

        public TalentPistolChain_E()
        {
            Name = "Epic Pistol Chain";
            Description = $"While Pistol is your primary weapon, give {increaseAttackDamage}% increased attack damage, {increaseProjSpeed}% increases projectile speed." +
                $"\nBullets chains {additionalChains} additional times";
            TalentRarity = Rarity.Epic;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.AddFlatWeaponChainAmount(additionalChains);
                    stats.GSC.AttackSC.IncreaseAttackDamage(increaseAttackDamage);
                    stats.GSC.UtilitySC.IncreaseProjectileSpeed(increaseProjSpeed);
                }

                stats.Equipment.OnEquipmentChange += OnEquipmentChange;
            };

            OnLock = (stats) =>
            {
                if (stats.Equipment.CurrentEquipedWeapon is Revolver || stats.Equipment.CurrentEquipedWeapon is Pistol_MK_1)
                {
                    stats.GSC.AttackSC.AddFlatWeaponChainAmount(-additionalChains);
                    stats.GSC.AttackSC.IncreaseAttackDamage(-increaseAttackDamage);
                    stats.GSC.UtilitySC.IncreaseProjectileSpeed(-increaseProjSpeed);
                }

                stats.Equipment.OnEquipmentChange -= OnEquipmentChange;
            };
        }

        private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
        {
            if (oldItem is Revolver || oldItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.AddFlatWeaponChainAmount(-additionalChains);
                playerStats.GSC.AttackSC.IncreaseAttackDamage(-increaseAttackDamage);
                playerStats.GSC.UtilitySC.IncreaseProjectileSpeed(-increaseProjSpeed);
            }

            if (newItem is Revolver || newItem is Pistol_MK_1)
            {
                playerStats.GSC.AttackSC.AddFlatWeaponChainAmount(additionalChains);
                playerStats.GSC.AttackSC.IncreaseAttackDamage(increaseAttackDamage);
                playerStats.GSC.UtilitySC.IncreaseProjectileSpeed(increaseProjSpeed);
            }
        }
    }
}