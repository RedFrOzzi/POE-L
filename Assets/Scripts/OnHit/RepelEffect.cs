namespace Database
{
    public class RepelEffect : OnHitEffect
    {
        public float DamagePercentRepel { get; set; } = 50;
        public RepelEffect()
        {
            Name = "Repel";
            IsStackable = false;
            OnHitOnOwner = OwnerEffect;
            OnHitOnEnemy = null;
        }
        public override string Description() => $"Returns {DamagePercentRepel}% of premitigated damage taken On Hit to attackers";

        void OwnerEffect(DamageArgs damageArgs)
        {
            if (damageArgs.ShooterStats == null) { return; }

            Damage damage = damageArgs.Damage * (DamagePercentRepel / 100);
            //Разворачиваем damageArgs меняя местами источник и цель
            damageArgs = new(damage, damageArgs.IsCritical, damageArgs.EnemyStats, damageArgs.ShooterStats, DamageArgs.DamageSource.SingleTarget);
            damageArgs.ShooterStats.DamageFilter.OutgoingDAMAGE(damageArgs);
        }
    }
}
