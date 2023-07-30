namespace Database
{
    public class DebuffStun : Buff
    {        
        public DebuffStun()
        {
            ShouldShow = true;
            IsPositive = false;
            Name = "Stuning";            
            Tags[0] = ModTag.Movement;
            Tags[1] = ModTag.Utility;
            Tags[2] = ModTag.CrowdControll;
        }
        public override string Description() => $"Stuns for {Duration} seconds";

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
