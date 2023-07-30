namespace Database
{
    public class ExplodeOnDeathEffect : OnHitEffect
    {
        public float Radius { get; set; } = 3f;
        public float Damage { get; set; } = 10f;

        public ExplodeOnDeathEffect()
        {
            Name = "ExplodeOnDeath";
            IsStackable = false;
            OnHitOnOwner = null;
            OnHitOnEnemy = EnemyEffect;
        }
        public override string Description() => $"Target Explode on death in {Radius} radius with {Damage}% of target max hp as phisical damage";

        void EnemyEffect(DamageArgs damageArgs)
        {
            if (damageArgs.EnemyStats == null) { return; }

            var explode = new ExplosionOnDeath();
            explode.Radius = Radius;
            explode.Damage = Damage;
            explode.GeneratedID = GeneratedID;
            damageArgs.EnemyStats.Health.AddOnDeathEffect(explode, damageArgs.ShooterStats);
        }
    }
}
