namespace Database
{
    public class TalentProjAbilDamage_C : TProp
    {
        private CH_Stats playerStats;
        public TalentProjAbilDamage_C()
        {
            Name = "Small Projectile Ability Damage";
            Description = $"Increase damage for projectile abilities by 10%";
            TalentRarity = Rarity.Common;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                stats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, ApplyAction, ModTag.Projectile);

                stats.AbilitiesManager.OnAbilityChange += OnAbilityChange;
            };

            OnLock = (stats) =>
            {
                stats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, RemoveAction, ModTag.Projectile);

                stats.AbilitiesManager.OnAbilityChange -= OnAbilityChange;
            };
        }

        private void OnAbilityChange(byte index)
        {
            playerStats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, RemoveAction, ModTag.Projectile);

            playerStats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, ApplyAction, ModTag.Projectile);
        }

        private void ApplyAction(StatsChanges[] lSCs)
        {
            foreach (var lsc in lSCs)
            {
                lsc.MagicSC.IncreaseSpellDamage(10);
            }
        }

        private void RemoveAction(StatsChanges[] lSCs)
        {
            foreach (var lsc in lSCs)
            {
                lsc.MagicSC.IncreaseSpellDamage(-10);
            }
        }
    }
}