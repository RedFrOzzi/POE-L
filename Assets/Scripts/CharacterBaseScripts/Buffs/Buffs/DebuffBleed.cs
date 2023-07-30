using UnityEngine;

namespace Database
{
    public class DebuffBleed : BuffStackable
    {
        private float PhysDamagePerSecond;

        public DebuffBleed()
        {
            ShouldShow = true;
            IsPositive = false;
            Name = "Bleed";
            Tags[1] = ModTag.DamageOverTime;
            Tags[2] = ModTag.Physical;
        }
        public override string Description() => $"Deals {PhysDamagePerSecond} damage per second for {Duration} seconds";

        public DebuffBleed SetDPS(float physDPS)
        {
            PhysDamagePerSecond = physDPS;
            return this;
        }

        public override void OnBuffTick(float tickDalay)
        {
            DamageArgs damageArgs = new(new(PhysDamagePerSecond * (1 + SourceStats.GSC.GlobalSC.FlatPhysDOTMultiValue) * StacksAmount * tickDalay, 0, 0),
                false, SourceStats, OwnerStats, DamageArgs.DamageSource.DOT);
            SourceStats.DamageFilter.OutgoingDAMAGE(damageArgs);
        }
    }
}
