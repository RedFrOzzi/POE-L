namespace Database
{
    //Base class
    public class VoidAbility : Ability
    {
        public VoidAbility()
        {
            ID = AbilityID.Void;            
            Name = "Void";
            Level = 1;
            Cooldown = 1;
            Manacost = 0;
        }

        public override string Description()
        {
            return "Void";
        }
    }
}
