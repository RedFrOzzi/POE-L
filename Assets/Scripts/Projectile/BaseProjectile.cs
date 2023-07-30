using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;


public class BaseProjectile : MonoBehaviour
{    
    public Damage Damage { get; set; }
    public bool IsCritical { get; set; }

    public CH_Stats ShooterStats { get; set; }
    public Vector3 ShooterInitialPosition { get; set; }

    public Action<BaseProjectile, Collider2D> ProjectileBehavior;

    public float SecondaryExplosionRadius { get; set; }
    public float PercentOfDamageToExplosionDamage { get; set; }
    public int ProjIndex { get; set; }
    public bool IsMultipleProjWork { get; set; }
    public float ProjSpeed { get; set; }
    public string ImpactAnimation { get; set; }
    public float ImpactAnimationRotation { get; set; }
    public Vector3 ImpactAnimationScale { get; set; } = new(1, 1, 1);
    public float ProjDestroyTime { get; set; } = 0;
    public int ChainsLeft { get; set; } = 0;
    public int PierceLeft { get; set; } = 0;

    public List<Collider2D> Colliders { get; set; } = new();    //лист с уже задетыми этим прожектайлом обьектов

    public LayerMask LayerMask { get; set; } //layerMask противника, используется в behaviour для поиска целей по цепи

    public Rigidbody2D RB { get; protected set; }
    public TrailRenderer TrailRenderer { get; protected set; }
    private SpriteRenderer spriteRenderer;

    protected float projLiveTime;
    
    protected void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        TrailRenderer = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        projLiveTime = ProjDestroyTime - Time.time;
    }

    private void OnEnable()
    {
        projLiveTime = ProjDestroyTime - Time.time;
    }

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
            //ANIMATIONS---
            AnimationPlayer.Instance.Play(ImpactAnimation, collision.ClosestPoint(transform.position), Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, ImpactAnimationRotation)) * RB.velocity.normalized), ImpactAnimationScale);
            //-------------

            DamageArgs damageArgs = new(Damage, IsCritical, ShooterStats, enemyStats, DamageArgs.DamageSource.Projectile);
            
            if (IsMultipleProjWork)
            {
                ShooterStats.DamageFilter.OutgoingAttackHIT(damageArgs);

                ProjectileBehavior(this, collision);
            }
            else
            {
                if (enemyStats.OnHit.HitByProjIndex.Contains(ProjIndex) == false)
                {
                    ShooterStats.DamageFilter.OutgoingAttackHIT(damageArgs);

                    ProjectileBehavior(this, collision);
                }

                enemyStats.OnHit.HitByProjIndex.Add(ProjIndex);
            }
        }
        else
        {
            //ANIMATIONS---
            AnimationPlayer.Instance.Play(WeaponBehaviour.WallHitAnimation, collision.ClosestPoint(transform.position), Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, -90)) * RB.velocity.normalized), WeaponBehaviour.WallHitAnimationScale, 2f);
            //-------------

            WeaponBehaviour.ProjectileBehaviourWallHit(this, collision);
        }
    }
}