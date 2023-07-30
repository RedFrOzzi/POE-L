namespace Database
{
    public class BuffSpeedUp : Buff
    {
        private float SpeedAmount = 100f;
        public BuffSpeedUp()
        {
            ShouldShow = true;
            IsPositive = true;
            Name = "SpeedUp";
            Tags[0] = ModTag.Speed;
            Tags[1] = ModTag.Movement;
            Tags[2] = ModTag.Utility;
        }
        public override string Description() => $"Speed you up for {SpeedAmount}% for {Duration} seconds";

        public BuffSpeedUp SetSpeed(float speedPercent)
        {
            SpeedAmount = speedPercent;
            return this;
        }

        public override void OnBuffApplication()
        {
            OwnerStats.GSC.UtilitySC.IncreaseMovementSpeed(SpeedAmount);
            OwnerStats.EvaluateStats();
        }

        public override void OnBuffExpire()
        {
            OwnerStats.GSC.UtilitySC.IncreaseMovementSpeed(-SpeedAmount);
            OwnerStats.EvaluateStats();
        }
    }

}
