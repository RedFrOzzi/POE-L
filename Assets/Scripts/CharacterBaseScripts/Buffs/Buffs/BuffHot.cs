namespace Database
{
    public class BuffHot : Buff
    {
        public float HealAmount = 100f;
        
        public BuffHot()
        {
            ShouldShow = true;
            IsPositive = true;            
            Name = "HealOverTime";
            Tags[0] = ModTag.Health;
            Tags[1] = ModTag.HealthOverTime;
        }
        public override string Description() => $"Heals for {HealAmount} over {Duration} seconds";

        public override void OnBuffTick(float tickDalay)
        {            
            OwnerStats.Health.TakeHeal((HealAmount / Duration) * OwnerStats.CurrentBuffPower * tickDalay);
        }
    }

}
