using UnityEngine;

namespace Database
{
    public class Stomp : Ability
    {
        private readonly float[] damagePerLevel;
        private readonly float[] durationPerLevel;
        private readonly float[] radiusPerLevel;

        private const float knockbackDistance = .5f;
        private const float movementSpeedIncrease = 30;
        private const float durationMulti = 1.5f;

        private const float newScale = 2f;
        private const float hitCD = 0.4f;
        private float currentCD;

        private bool isActive;

        public Stomp()
        {
            ID = AbilityID.Stomp;
            Name = "Stomp";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Magic;
            Tags[1] = ModTag.Damage;
            Tags[2] = ModTag.Area;
            Tags[3] = ModTag.CrowdControll;

            damagePerLevel = new float[] { 20, 20, 25, 25, 25, 30, 30, 30, 30, 35, 35, 35, 40, 40, 40, 45, 45, 50, 50, 70, 70 };
            durationPerLevel = new float[] { 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 9 };
            radiusPerLevel = new float[] { 3.5f, 3.5f, 3.5f, 3.5f, 3.5f, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7 };

            CritChance = 8f;
            Cooldown = 20;
            Manacost = 2;

            Tip = "Player increase in size and begin stomping enemies around, knocking them back. If activated manualy, also increases movement speed and duration of the effect";
        }
        public override string Description()
        {
            return $"Player increases in size and begin stomping enemies around, knocking them back. If activated manualy, also increases movement speed by {movementSpeedIncrease}% and duration of the effect is {durationMulti}x";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            if (isActive) { return; }

            float _duration = Slot.GetAbilityDuration(durationPerLevel[LevelClamp()]);
            
            stats.transform.localScale = new Vector3(newScale, newScale, newScale);

            isActive = true;

            if (isAutocasted)
                Autocast(stats, _duration);
            else
                ManualCast(stats, _duration);
        }

        public override void OnPeriodicCall(CH_Stats stats, float periodicCallCD)
        {
            if (isActive == false) { return; }

            currentCD += periodicCallCD;

            if (currentCD < hitCD) { return; }

            currentCD = 0;

            var currentRadius = Slot.GetAbilityAOE(radiusPerLevel[LevelClamp()]);

            Visuals(stats, currentRadius);

            var targets = Physics2D.OverlapCircleAll(stats.transform.position, currentRadius, MagicBehaviour.EnemyLayerMask);

            (Damage _damage, bool _isCritical) = Slot.GetAbilityDamage(new (0, damagePerLevel[LevelClamp()], 0), CritChance, Tags);

            DamageArgs damageArgs = new(_damage * hitCD, _isCritical, stats, null, DamageArgs.DamageSource.AOE);

            foreach (var target in targets)
            {
                if (target.TryGetComponent(out CH_Stats enemyStats))
                {
                    damageArgs.EnemyStats = enemyStats;

                    stats.DamageFilter.OutgoingDAMAGE(damageArgs);

                    enemyStats.AdditionalEffects.KnockBack(stats.transform, knockbackDistance);
                }
            }
        }

        private void Autocast(CH_Stats stats, float duration)
        {
            UtilityDelayFunctions.RunWithDelay(() =>
            {
                stats.transform.localScale = Vector3.one;
                isActive = false;
            },
            duration);
        }

        private void ManualCast(CH_Stats stats, float duration)
        {
            stats.GSC.UtilitySC.IncreaseMovementSpeed(movementSpeedIncrease);

            UtilityDelayFunctions.RunWithDelay(() =>
            {
                stats.transform.localScale = Vector3.one;
                isActive = false;
                stats.GSC.UtilitySC.IncreaseMovementSpeed(-movementSpeedIncrease);
            },
            duration * durationMulti);
        }

        private void Visuals(CH_Stats stats, float radius)
        {
            AnimationPlayer.Instance.Play("Smoke_01", new Vector2(stats.transform.position.x, stats.transform.position.y - 1f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f), 3f, AnimationSortingOrder.BehindPlayer);
            AnimationPlayer.Instance.Play("Smoke_Circle_01", new Vector2(stats.transform.position.x, stats.transform.position.y - 1f),
                Quaternion.identity, radius * 2 * Vector3.one, new Color(0.5f, 0.5f, 0.5f, 0.8f), 0.7f, AnimationSortingOrder.BehindPlayer);
        }
    }
}
