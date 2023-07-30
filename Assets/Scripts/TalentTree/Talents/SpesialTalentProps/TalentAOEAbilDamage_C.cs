namespace Database
{
    public class TalentAOEAbilDamage_C : TProp
    {
        private CH_Stats playerStats;
        public TalentAOEAbilDamage_C()
        {
            Name = "Common AOE Ability Damage";
            Description = $"Increase damage for area abilities by 10%";
            TalentRarity = Rarity.Common;

            OnUnlock = (stats) =>
            {
                playerStats = stats;

                stats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, ApplyAction, ModTag.Area);

                stats.AbilitiesManager.OnAbilityChange += OnAbilityChange;
            };

            OnLock = (stats) =>
            {
                stats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, RemoveAction, ModTag.Area);

                stats.AbilitiesManager.OnAbilityChange -= OnAbilityChange;
            };
        }

        private void OnAbilityChange(byte index)
        {
            playerStats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, RemoveAction, ModTag.Area);

            playerStats.AbilitiesManager.ChangeAbilitiesStats(CommonQueries.FindAbilitiesStatsByTags, ApplyAction, ModTag.Area);
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