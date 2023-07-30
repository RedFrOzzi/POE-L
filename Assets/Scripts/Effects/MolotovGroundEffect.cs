using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovGroundEffect : MonoBehaviour, IDamageEffect
{
    private bool isEngaged;
    private Damage damage;
    private float radius;
    private float expireTime;
    private LayerMask layerMask;
    private CH_Stats effectOwnerStats;

    private float animNextTriggerTime;
    private const float animCD = 0.15f;

    public void SetUpEffect(CH_Stats effectOwnerStats, float radius, float duration, Damage damagePerSec, LayerMask layerMask)
    {
        var tickCooldown = DamageEffectManager.tickCD;
        this.effectOwnerStats = effectOwnerStats;
        this.radius = radius;
        this.damage = new Damage(damagePerSec.Physical * tickCooldown, damagePerSec.Magic * tickCooldown, damagePerSec.True * tickCooldown,
            damagePerSec.PercentPhysical * tickCooldown, damagePerSec.PercentMagic * tickCooldown, damagePerSec.PercentTrue * tickCooldown);
        expireTime = Time.time + duration;
        this.layerMask = layerMask;
        isEngaged = true;

        AnimationPlayer.Instance.PlayForDuration("FireCircle_01", transform.position, Quaternion.identity, 2 * radius * Vector2.one, new Color(1, 1, 1, 0.2f), duration, 1f, AnimationSortingOrder.BehindPlayer);
    }

    private void Update()
    {
        if (GameFlowManager.Instance.IsGamePaused) { return; }

        if (isEngaged == false) { return; }

        AnimationTrigger();

        if (Time.time > expireTime)
        {
            RemoveFromManagerList();
            Destroy(gameObject);
        }
    }

    private void AnimationTrigger()
    {
        if (animNextTriggerTime > Time.time) { return; }

        animNextTriggerTime = Time.time + animCD;

        for (byte i = 0; i < 4; i++)
        {
            var ang = (Vector2)transform.position + Random.insideUnitCircle * radius / 2;
            AnimationPlayer.Instance.Play($"FireGround_0{i + 1}", ang, Quaternion.identity, Vector3.one, new Color(1, 1, 1, 0.5f), AnimationSortingOrder.BehindPlayer);
        }
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
