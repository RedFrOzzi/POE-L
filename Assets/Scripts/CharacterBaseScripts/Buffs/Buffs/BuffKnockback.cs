namespace Database
{
    public class BuffKnockback : Buff
    {
        public float Distance { get; set; } = 5f;

        public BuffKnockback()
        {
            IsPositive = true;
            Name = "Knockback";            
            Tags[0] = ModTag.Movement;
            Tags[1] = ModTag.Utility;
            Tags[2] = ModTag.CrowdControll;
        }
        public override string Description() => $"Knocks back targets for {Distance} units for {Duration} seconds";

        public override void OnBuffApplication()
        {
            KnockbackEffect knockbackEffect = new KnockbackEffect();
            knockbackEffect.GeneratedID = GeneratedID;
            knockbackEffect.Distance = Distance;

            OwnerStats.OnHit.AddOnHit_GivingHit(knockbackEffect);
        }

        public override void OnBuffExpire()
        {
            OwnerStats.OnHit.RemoveOnHit_GivingHit(GeneratedID);
        }
    }

}
