using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : BaseProjectile
{
    private void Update()
    {
        if (Time.time > ProjDestroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjDestroyTime = Time.time + projLiveTime;

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
