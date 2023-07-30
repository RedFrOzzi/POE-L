namespace Database
{
    public class DebuffSlow : Buff
    {
        public float SlowAmount = 20;
        public DebuffSlow()
        {
            ShouldShow = true;
            IsPositive = false;
            Name = "Slowing";
            Tags[0] = ModTag.Speed;
            Tags[1] = ModTag.Movement;
            Tags[2] = ModTag.Utility;
            Tags[3] = ModTag.CrowdControll;
        }
        public override string Description() => $"Decrease movement speed by {SlowAmount}% over {Duration} seconds";

        public DebuffSlow SetSlow(float slowPercent)
        {
            SlowAmount = slowPercent;
            return this;
        }

        public override void OnBuffApplication()
        {
            OwnerStats.GSC.UtilitySC.IncreaseMovementSpeed(-SlowAmount);
            OwnerStats.EvaluateStats();
        }

        public override void OnBuffExpire()
        {
            OwnerStats.GSC.UtilitySC.IncreaseMovementSpeed(SlowAmount);
            OwnerStats.EvaluateStats();
        }
    }

}
