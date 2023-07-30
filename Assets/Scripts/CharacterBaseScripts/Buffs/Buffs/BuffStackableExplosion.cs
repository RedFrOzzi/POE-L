using UnityEngine;

namespace Database
{
    public class BuffStackableExplosion : BuffTriggerOnFullStacks
    {
        public float Damage = 10;
        public float Radius = 2f;

        public BuffStackableExplosion()
        {
            ShouldShow = true;
            IsPositive = false;
            Name = "StackableExplosion";
            Tags[0] = ModTag.Damage;
            Tags[1] = ModTag.Magic;
            Tags[2] = ModTag.Area;
        }
        public override string Description() => $"Apply stackable debuff for {Duration} seconds. On {StacksToTrigger} stacks target explodes for {Damage} base magic damage in {Radius} radius";

        public override void OnFullStacks()
        {
            int layerMask;
            if (OwnerStats.gameObject.layer == LayerMask.NameToLayer("Player"))            
                layerMask = LayerMask.GetMask("Player");
            else
                layerMask = LayerMask.GetMask("Enemy");

            var colliders = Physics2D.OverlapCircleAll(OwnerStats.transform.position, Radius, layerMask);
            Damage damage = new(0, Damage *
                        (1 + OwnerStats.GSC.GlobalSC.IncreaseDamageValue + OwnerStats.GSC.GlobalSC.IncreaseMagicDamageValue + OwnerStats.GSC.GlobalSC.IncreaseAreaDamageValue) *
                        (OwnerStats.GSC.GlobalSC.MoreDamageValue * OwnerStats.GSC.GlobalSC.MoreMagicDamageValue * OwnerStats.GSC.GlobalSC.MoreAreaDamageValue) *
                        (OwnerStats.GSC.GlobalSC.LessDamageValue * OwnerStats.GSC.GlobalSC.LessMagicDamageValue * OwnerStats.GSC.GlobalSC.LessAreaDamageValue)
                        , 0);

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out CH_Stats enemyStats))
                {
                    SourceStats.DamageFilter.OutgoingDAMAGE(new(damage, false, SourceStats, enemyStats, DamageArgs.DamageSource.AOE));
                }
            }
        }
    }

}
