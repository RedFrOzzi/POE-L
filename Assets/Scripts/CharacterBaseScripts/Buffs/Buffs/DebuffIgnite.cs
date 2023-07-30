namespace Database
{
    public class DebuffIgnite : BuffStackable
    {
        private float MagicDamagePerSecond;
        public DebuffIgnite()
        {
            ShouldShow = true;
            IsPositive = false;
            Name = "Ignite";
            Tags[0] = ModTag.DamageOverTime;
            Tags[1] = ModTag.Magic;
        }
        public override string Description() => $"Deals {MagicDamagePerSecond} damage per second for {Duration} seconds";

        /// <summary>
        /// No damage manipulation inside
        /// </summary>
        public DebuffIgnite SetMDPS(float magicDPS)
        {
            MagicDamagePerSecond = magicDPS;
            return this;
        }

        public override void OnBuffTick(float tickDalay)
        {
            DamageArgs damageArgs = new(new(0, MagicDamagePerSecond * (1 + SourceStats.GSC.GlobalSC.FlatMagicDOTMultiValue) * StacksAmount * tickDalay, 0),
                false, SourceStats, OwnerStats, DamageArgs.DamageSource.DOT);

            SourceStats.DamageFilter.OutgoingDAMAGE(damageArgs);
        }
    }

}
