namespace Database
{
    public class ArmorPenetrationEffect : OnHitEffect
    {
        public float PersentArmorPen { get; set; } = 1;
        public int MaxStacks { get; set; } = 1;
        public float DebuffDuration { get; set; } = 1;

        public ArmorPenetrationEffect()
        {
            Name = "Armor Penetration";
            IsStackable = true;
            OnHitOnOwner = null;
            OnHitOnEnemy = EnemyEffect;
        }
        public override string Description() => $"Apply stackable debuff on target, that reduce armor by {PersentArmorPen}. Stacks up to {MaxStacks} with up to {DebuffDuration} duration";

        void EnemyEffect(DamageArgs damageArgs)
        {
            if (damageArgs.EnemyStats == null) { return; }

            DebuffArmorPen armorPen = new();
            armorPen.MaxStacks = MaxStacks;
            armorPen.PersentPenPerStack = PersentArmorPen;
            armorPen.SetDuration(DebuffDuration);

            damageArgs.EnemyStats.BuffManager.ApplyBuff(armorPen, damageArgs.ShooterStats);
        }
    }
}
