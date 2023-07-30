namespace Database
{
    public class BuffStackableSpeed : BuffStackable
    {
        public float SpeedAmount = 20f;

        public BuffStackableSpeed()
        {
            ShouldShow = true;
            IsPositive = true;
            Name = "StackableSpeed";
            Tags[0] = ModTag.Speed;
            Tags[1] = ModTag.Movement;
            Tags[2] = ModTag.Utility;
        }
        public override string Description() => $"Speed you up for {SpeedAmount}% for {Duration} seconds, stacks up to {StacksAmount} times";

        public override void OnBuffApplication()
        {
            OwnerStats.GSC.UtilitySC.IncreaseMovementSpeed(SpeedAmount);
            OwnerStats.EvaluateStats();
        }

        public override void OnBuffExpire()
        {
            OwnerStats.GSC.UtilitySC.IncreaseMovementSpeed(-SpeedAmount * StacksAmount);
            OwnerStats.EvaluateStats();
        }
    }

}
