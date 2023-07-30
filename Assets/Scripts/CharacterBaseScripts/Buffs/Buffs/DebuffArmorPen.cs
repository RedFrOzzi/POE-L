namespace Database
{
    public class DebuffArmorPen : BuffStackable
    {
        public float PersentPenPerStack = 1;

        public DebuffArmorPen()
        {
            MaxStacks = 5;
            IsPositive = false;
            Name = "ArmorPenetration";
            Tags[0] = ModTag.Armor;
            Tags[1] = ModTag.Utility;
        }
        public override string Description() => $"Reduces armor for {PersentPenPerStack} per stack, up to {MaxStacks} stacks";

        public override void OnBuffApplication()
        {
            OwnerStats.GSC.DefanceSC.IncreaseArmor(-PersentPenPerStack);
            OwnerStats.EvaluateStats();
        }

        public override void OnBuffExpire()
        {
            OwnerStats.GSC.DefanceSC.IncreaseArmor(PersentPenPerStack * StacksAmount);
            OwnerStats.EvaluateStats();
        }
    }

}
