namespace Database
{
    public class KnockbackEffect : OnHitEffect
    {
        public float Distance { get; set; } = 5f;
        public KnockbackEffect()
        {
            Name = "Knockback";
            IsStackable = false;
            OnHitOnOwner = null;
            OnHitOnEnemy = EnemyEffect;
        }
        public override string Description() => $"Knocks back for {Distance} units";

        void EnemyEffect(DamageArgs damageArgs)
        {
            if (damageArgs.EnemyStats == null) { return; }

            damageArgs.EnemyStats.AdditionalEffects.KnockBack(damageArgs.ShooterStats.transform, Distance);
        }
    }
}
