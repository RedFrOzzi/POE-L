using System.Linq;
using UnityEngine;

public class GrenadeProjectile : BaseProjectile
{
    public Vector2 TargetLocation { get; set; }
    public bool IsBouncing { get; set; }

    private float liveTime;
    private Vector3 lScale;
    private Vector3 targetScale;
    private float fullTime;
    private float halfTime;
    private bool halfTimePass;

    private const float scaleChangeSpeedMultiplier = 3f;

    private void Start()
    {
        liveTime = 0;
        lScale = transform.localScale;
        targetScale = lScale * 3f;
        fullTime = ProjDestroyTime - Time.time;
        halfTime = fullTime / 2;

        var dir = TargetLocation - (Vector2)transform.position;
        var speed = dir.magnitude / fullTime;
        RB.velocity = dir.normalized * speed;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * dir);
    }

    private void Update()
    {
        if (Time.time > ProjDestroyTime)
        {
            Explode();
        }
    }

    private void FixedUpdate()
    {
        float x;
        if (halfTimePass == false)
        {
            x = Mathf.Lerp(lScale.x, targetScale.x, liveTime * scaleChangeSpeedMultiplier);
            liveTime += Time.fixedDeltaTime;
            if (liveTime > halfTime)
                halfTimePass = true;
        }
        else
        {
            x = Mathf.Lerp(lScale.x, targetScale.x, liveTime * scaleChangeSpeedMultiplier);
            liveTime -= Time.fixedDeltaTime;
        }

        transform.localScale = new Vector3(x, x, x);
    }

    private void Explode()
    {
        if (IsBouncing)
        {
            BouncingBehaviour();
        }
        else
        {
            NormalBehaviour();
        }
    }

    public void ResetProjectile(Vector2 newTargetLocation, float newDestroyTime)
    {
        ProjDestroyTime = Time.time + newDestroyTime;

        liveTime = 0;
        fullTime = newDestroyTime;
        halfTime = fullTime / 2;
        halfTimePass = false;

        var dir = newTargetLocation - (Vector2)transform.position;
        var speed = dir.magnitude / fullTime;
        RB.velocity = dir.normalized * speed;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == WeaponBehaviour.WallLayer)
        {
            //ANIMATIONS---
            AnimationPlayer.Instance.Play(WeaponBehaviour.WallHitAnimation, collision.ClosestPoint(transform.position), Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, -90)) * RB.velocity.normalized), WeaponBehaviour.WallHitAnimationScale, 2f);
            //-------------

            WeaponBehaviour.ProjectileBehaviourGrenadeWallHit(this, collision);
        }
    }

    private void NormalBehaviour()
    {
        var col = Physics2D.OverlapCircle(transform.position, SecondaryExplosionRadius, LayerMask);

        if (col != null && col.TryGetComponent(out CH_Stats enemyStats))
        {
            DamageArgs da = new(Damage, IsCritical, ShooterStats, enemyStats, DamageArgs.DamageSource.AOE);

            ShooterStats.DamageFilter.OutgoingAttackHIT(da);

            ProjectileBehavior(this, col);
        }
        else
        {
            //VISUALS IF NO TARGETS FOUND
            AnimationPlayer.Instance.Play(ImpactAnimation, transform.position, Quaternion.Euler(0, 0, ImpactAnimationRotation), ImpactAnimationScale, 1.5f);

            Destroy(gameObject);
        }
    }

    private void BouncingBehaviour()
    {
        var cols = Physics2D.OverlapCircleAll(transform.position, SecondaryExplosionRadius, LayerMask);

        if (cols.Length <= 0)
        {
            //VISUALS IF NO TARGETS FOUND
            AnimationPlayer.Instance.Play(ImpactAnimation, transform.position, Quaternion.Euler(0, 0, ImpactAnimationRotation), ImpactAnimationScale, 1.5f);

            Destroy(gameObject);
        }

        AnimationPlayer.Instance.Play(ImpactAnimation, transform.position, Quaternion.Euler(0, 0, ImpactAnimationRotation), ImpactAnimationScale, 1.5f);

        foreach (var col in cols)
        {
            if (col.TryGetComponent(out CH_Stats enemyStats))
            {
                DamageArgs da = new(Damage, IsCritical, ShooterStats, enemyStats, DamageArgs.DamageSource.AOE);

                ShooterStats.DamageFilter.OutgoingAttackHIT(da);
            }
        }

        var collision = cols.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).FirstOrDefault();

        if (collision != null)
            Colliders.Add(collision);

        ProjectileBehavior(this, collision);
    }
}
