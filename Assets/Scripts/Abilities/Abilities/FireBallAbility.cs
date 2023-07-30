using System;
using UnityEngine;

namespace Database
{
    public class FireBallAbility : Ability
    {
        private readonly float Speed;
        private readonly float Range;
        private readonly float[] DamagePerLevel;
        private readonly int[] ProjectilesPerLevel;
        private readonly int[] PiercePerLevel;
        public FireBallAbility()
        {
            ID = AbilityID.FireBall;
            Name = "Fire ball";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Damage;
            Tags[1] = ModTag.Projectile;
            Tags[2] = ModTag.Magic;
            DamagePerLevel = new float[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210 };
            ProjectilesPerLevel = new int[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7, 7 };
            PiercePerLevel = new int[] { 3, 3, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 7, 7 };
            CritChance = 8f;
            Speed = 8;
            Range = 20;
            Cooldown = 2;
            Manacost = 2;

            Tip = "Throw fire projectiles, that pierce targets";
        }
        public override string Description()
        {
            var (damage, _) = Slot.GetAbilityDamage(new Damage(0, DamagePerLevel[LevelClamp()], 0), 0, Tags);

            return $"Throw {ProjectilesPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue} fire projectiles for" +
                $" {damage.Magic} magic damage, that pierce {PiercePerLevel[LevelClamp()]} targets";
        }
        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            ProjectileCreationArgs projectileArgs = new(MagicBehaviour.ProjectilesGameObjects["FireBall"], new Damage(0, DamagePerLevel[LevelClamp()], 0),
                CritChance, Speed, Range, ProjectilesPerLevel[LevelClamp()], PiercePerLevel[LevelClamp()]);

            SpawnFireBall(stats, Slot, projectileArgs, aim, FireBallHitBehaviour);
        }

        private void SpawnFireBall(CH_Stats stats, AbilitySlot slot, ProjectileCreationArgs projectileArgs, Vector2 aim, Action<BaseProjectile, Collider2D> projBehavior)
        {
            Vector2 firstAngle = new();
            Rigidbody2D rigidbody2D;
            BaseProjectile baseProjectile;
            (Damage damage, bool isCritical) = Slot.GetAbilityDamage(projectileArgs.Damage, projectileArgs.BaseCritChance, Tags);

            for (int i = 0; i < projectileArgs.ProjectilesAmount + slot.LSC.MagicSC.FlatSpellProjectileAmountValue + slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue; i++)
            {

                GameObject go = GameObject.Instantiate(projectileArgs.Projectile, stats.transform.position, Quaternion.identity, MagicBehaviour.ProjectileParentObject.transform);

                rigidbody2D = go.GetComponent<Rigidbody2D>();
                baseProjectile = go.GetComponent<BaseProjectile>();
                var spriteRenderer = go.GetComponent<SpriteRenderer>();

                //VISUALS
                ObjectTrailRenderer.Instance.PlayTrail(spriteRenderer, go.transform, Vector3.one);

                go.layer = MagicBehaviour.PlayerProjectileLayer;

                if (i == 0)
                {
                    firstAngle = (aim - (Vector2)stats.transform.position).normalized;

                    rigidbody2D.velocity = firstAngle * projectileArgs.Speed;

                    go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
                }
                else if (i % 2 == 0)
                {
                    rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * 2)) * firstAngle * projectileArgs.Speed;

                    go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
                }
                else
                {
                    rigidbody2D.velocity = Quaternion.Euler(new Vector3(0, 0, i * -2)) * firstAngle * projectileArgs.Speed;

                    go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
                }

                baseProjectile.ShooterStats = stats;

                baseProjectile.ProjectileBehavior = projBehavior;
                baseProjectile.ChainsLeft = projectileArgs.ChainsAmount;
                baseProjectile.PierceLeft = projectileArgs.PierceAmount;

                baseProjectile.ProjDestroyTime = Time.time + projectileArgs.Range / projectileArgs.Speed;
                baseProjectile.ProjSpeed = projectileArgs.Speed;
                baseProjectile.ProjIndex = WeaponBehaviour.IndexOfProjectiles;
                baseProjectile.IsMultipleProjWork = false;
                baseProjectile.LayerMask = MagicBehaviour.EnemyLayerMask;
                baseProjectile.ShooterInitialPosition = stats.transform.position;
                baseProjectile.ImpactAnimation = "FireBurn_02";
                baseProjectile.ImpactAnimationRotation = -90;
                baseProjectile.ImpactAnimationScale = Vector3.one;

                baseProjectile.Damage = damage;
                baseProjectile.IsCritical = isCritical;
            }

            WeaponBehaviour.IndexOfProjectiles++;
        }

        private void FireBallHitBehaviour(BaseProjectile baseProjectile, Collider2D _)
        {
            if (baseProjectile.PierceLeft > 0)
            {
                baseProjectile.PierceLeft--;
            }            
            else
            {
                GameObject.Destroy(baseProjectile.gameObject);
            }
        }
    }
}
