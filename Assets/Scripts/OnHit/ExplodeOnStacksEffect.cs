namespace Database
{
    public class ExplodeOnStacksEffect : OnHitEffect
    {
        public int StacksAmount { get; set; } = 3;
        public float Radius { get; set; } = 2f;
        public float Damage { get; set; } = 100;
        public float Duration { get; set; } = 3f;
        public ExplodeOnStacksEffect()
        {
            Name = "ExplodeOnStacks";
            IsStackable = true;
            OnHitOnOwner = null;
            OnHitOnEnemy = EnemyEffect;
        }
        public override string Description() => $"Apply stacks of explosives on target. On {StacksAmount} stacks enemy explodes in {Radius} radius with {Damage} damage";

        void EnemyEffect(DamageArgs damageArgs)
        {
            if (damageArgs.EnemyStats == null) { return; }

            BuffStackableExplosion buff = new();
            buff.Damage = Damage;
            buff.StacksToTrigger = StacksAmount;
            buff.Radius = Radius;
            buff.SetDuration(Duration);
            damageArgs.EnemyStats.BuffManager.ApplyBuff(buff, damageArgs.ShooterStats);
        }
    }
}
