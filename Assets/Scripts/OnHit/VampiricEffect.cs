namespace Database
{
    public class VampiricEffect : OnHitEffect
    {
        public float PercentDamageToHeal { get; set; } = 0.1f;
        public VampiricEffect()
        {
            Name = "Vampirism";
            IsStackable = true;
            OnHitOnOwner = OwnerEffect;
            OnHitOnEnemy = null;
        }
        public override string Description() => $"Give {PercentDamageToHeal * 100f}% phisical damage vampirism";

        void OwnerEffect(DamageArgs damageArgs)
        {
            if (damageArgs.ShooterStats == null) { return; }

            float damage = ((damageArgs.Damage.Physical * PercentDamageToHeal) + 
                (damageArgs.Damage.PercentPhysical * damageArgs.EnemyStats.CurrentHP / 100) * PercentDamageToHeal) * damageArgs.ShooterStats.CurrentHealingAmplifier;

            damageArgs.ShooterStats.Health.TakeHeal(damage);
        }
    }
}
