namespace Database
{
    public class CastOnCritEffect : OnHitEffect
    {
        public byte AbilityIndex { get; set; }
        public CastOnCritEffect()
        {
            Name = "CastOnCrit";
            IsStackable = false;
            OnHitOnOwner = null;
            OnHitOnEnemy = EnemyEffect;
        }
        public override string Description() => $"Cast choosen ability on critical attack hit";

        void EnemyEffect(DamageArgs damageArgs)
        {
            if (damageArgs.EnemyStats == null) { return; }

            if (damageArgs.IsCritical == false) { return; }

            damageArgs.ShooterStats.AbilitiesManager.ActivateAbilityByTrigger(AbilityIndex, damageArgs.EnemyStats.transform.position);
        }
    }
}
