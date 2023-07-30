namespace Database
{
    public class BuffDoubleDamage : Buff
    {
        public BuffDoubleDamage()
        {
            ShouldShow = true;
            IsPositive = true;
            Name = "DoubleDamage";
            Tags[0] = ModTag.Damage;
        }
        public override string Description() => $"Double your damage for {Duration} seconds";

        public override void OnBuffApplication()
        {
            OwnerStats.GSC.GlobalSC.MoreDamage(100);
            OwnerStats.EvaluateStats();
        }

        public override void OnBuffExpire()
        {
            OwnerStats.GSC.GlobalSC.MoreDamage(-100);
            OwnerStats.EvaluateStats();
        }
    }

}
