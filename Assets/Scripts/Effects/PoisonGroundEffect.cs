using UnityEngine;

public class PoisonGroundEffect : MonoBehaviour, IDamageEffect
{
    private Damage damage;
    private float radius;
    private float expireTime;
    private LayerMask layerMask;
    private CH_Stats effectOwnerStats;

    private float nextAnimSpawn;
    private const float animSpawnCD = 0.3f;

    
    public void SetUpEffect(CH_Stats effectOwnerStats, float radius, float duration, Damage damagePerSec, LayerMask layerMask)
    {
        var tickCooldown = DamageEffectManager.tickCD;
        this.effectOwnerStats = effectOwnerStats;
        this.radius = radius;
        this.damage = new Damage(damagePerSec.Physical * tickCooldown, damagePerSec.Magic * tickCooldown, damagePerSec.True * tickCooldown,
            damagePerSec.PercentPhysical * tickCooldown, damagePerSec.PercentMagic * tickCooldown, damagePerSec.PercentTrue * tickCooldown);
        expireTime = Time.time + duration;
        this.layerMask = layerMask;
    }

    private void Update()
    {
        if (GameFlowManager.Instance.IsGamePaused) { return; }

        if (Time.time > expireTime)
        {
            RemoveFromManagerList();
            Destroy(gameObject);
        }

        if (nextAnimSpawn > Time.time) { return; }

        nextAnimSpawn = Time.time + animSpawnCD;

        AnimationPlayer.Instance.Play("PoisonCloud_01", (Vector2)transform.position + Random.insideUnitCircle / 4
                        , Quaternion.identity, Vector3.one * radius, new Color(0.8156863f, 0.979804f, 0.6411765f), AnimationSortingOrder.BehindPlayer);
    }

    public (Collider2D[], Damage) TickDamage()
    {
        return (Physics2D.OverlapCircleAll(transform.position, radius, layerMask), damage);
    }

    public void AddToManagerList()
    {
        DamageEffectManager.AddDamageEffectToList(this);
    }

    public void RemoveFromManagerList()
    {
        DamageEffectManager.RemoveDamageEffectFromList(this);
    }

    public CH_Stats GetEffectOwnerStats()
    {
        return effectOwnerStats;
    }
}
