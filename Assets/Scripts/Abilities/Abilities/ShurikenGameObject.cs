using System.Collections;
using UnityEngine;
using Database;

public class ShurikenGameObject : MagicProjectile
{
    private bool isProjTriggerOutOfRange;
    private const float rotationSpeed = 0.15f;
    private float amountOfReturnings = 1;

    public byte AmountOfReturningsLeft { get; set; }
    public Shuriken ShurikenSkill { get; set; }

    private void FixedUpdate()
    {
        RB.velocity = transform.right * ProjSpeed;

        if (isProjTriggerOutOfRange == false && (transform.position - ShooterStats.transform.position).magnitude > 10)
        {
            isProjTriggerOutOfRange = true;
        }

        if (isProjTriggerOutOfRange)
        {
            var dir = ShooterStats.transform.position - transform.position;
            var rotGoal = Quaternion.FromToRotation(Vector2.right, dir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, rotationSpeed * amountOfReturnings);

            if (dir.magnitude < 1)
            {
                isProjTriggerOutOfRange = false;
                amountOfReturnings += 0.5f;
                AmountOfReturningsLeft--;

                if (AmountOfReturningsLeft <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDisable()
    {
        ShurikenSkill?.RemoveActiveShuriken();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CH_Stats enemyStats))
        {
            //ANIMATION------------------------------------
            AnimationPlayer.Instance.Play(ImpactAnimation, collision.ClosestPoint(transform.position), Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, ImpactAnimationRotation)) * RB.velocity.normalized), ImpactAnimationScale);
            //---------------------------------------------


            DamageArgs damageArgs = new(Damage, IsCritical, ShooterStats, enemyStats, DamageArgs.DamageSource.Projectile);

            if (IsMultipleProjWork)
            {
                ShooterStats.DamageFilter.OutgoingSpellHIT(damageArgs);

                ProjectileBehavior(this, collision);
            }
            else
            {
                if (enemyStats.OnHit.HitByProjIndex.Contains(ProjIndex) == false)
                {
                    ShooterStats.DamageFilter.OutgoingSpellHIT(damageArgs);

                    ProjectileBehavior(this, collision);
                }

                enemyStats.OnHit.HitByProjIndex.Add(ProjIndex);
            }
        }
        else
        {
            //ANIMATION---------------------------------
            AnimationPlayer.Instance.Play(MagicBehaviour.WallHitAnimation, collision.ClosestPoint(transform.position), Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, -90)) * RB.velocity.normalized), MagicBehaviour.WallHitAnimationScale);
            //------------------------------------------


            WeaponBehaviour.ProjectileBehaviourWallHit(this, collision);
        }
    }
}
