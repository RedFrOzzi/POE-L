namespace Database
{
    public class DebuffDisableControll : Buff
    {        
        public DebuffDisableControll()
        {
            IsPositive = false;
            Name = "ControllLoss";
            Tags[0] = ModTag.Movement;
            Tags[1] = ModTag.Utility;
            Tags[2] = ModTag.CrowdControll;
        }
        public override string Description() => $"Disables controll and makes you imune for {Duration} seconds";

        public override void OnBuffApplication()
        {
            OwnerStats.SetAbilityToControll(false);
        }

        public override void OnBuffExpire()
        {
            OwnerStats.SetAbilityToControll(true);
        }
    }

}
