using UnityEngine;

namespace Database
{
    public class BuffDashAbility : Buff
    {
        public float Distance = 5f;
        public float KnockbackRadius = 3f;
        public float KnockbackDistance = 2f;
        public BuffDashAbility()
        {
            IsPositive = true;
            Name = "DashAbility";
            Tags[0] = ModTag.Movement;
            Tags[1] = ModTag.Utility;
        }
        public override string Description() => $"Dash you in moving direction for {Distance} units, knocks back enemies at the end";

        public override void OnBuffApplication()
        {
            OwnerStats.AdditionalEffects.Dash(OwnerStats.CurrentForwardDirection, Distance);
            OwnerStats.SetImmunity(true);
        }

        public override void OnBuffExpire()
        {
            Collider2D[] targets;
            LayerMask playerLayerMask = LayerMask.GetMask("Player");
            LayerMask enemyLayerMask = LayerMask.GetMask("Enemy");

            if (OwnerStats.gameObject.CompareTag("Player"))
            {
                targets = Physics2D.OverlapCircleAll(OwnerStats.transform.position, KnockbackRadius, enemyLayerMask);
            }
            else
            {
                targets = Physics2D.OverlapCircleAll(OwnerStats.transform.position, KnockbackRadius, playerLayerMask);
            }

            foreach (Collider2D target in targets)
            {
                if (target.TryGetComponent<CH_AdditionalEffects>(out CH_AdditionalEffects additionalEffects))
                {
                    additionalEffects.KnockBack(OwnerStats.transform, KnockbackDistance);
                }
            }

            OwnerStats.SetImmunity(false);
        }
    }

}
