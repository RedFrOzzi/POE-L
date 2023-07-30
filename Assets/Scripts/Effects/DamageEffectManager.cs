using System.Collections.Generic;
using UnityEngine;

public class DamageEffectManager : MonoBehaviour
{
	private static DamageEffectManager instance;

    private List<IDamageEffect> damageEffectObjects;
    private Dictionary<Collider2D, DamageArgs> hitObjectsAndDamageToDeal;

    private float nextTickTime;

    public const float tickCD = 0.5f;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        damageEffectObjects = new();
        hitObjectsAndDamageToDeal = new();
    }

    private void Update()
    {
        if (nextTickTime > Time.time) { return; }

        nextTickTime = Time.time + tickCD;

        if (damageEffectObjects.Count <= 0) { return; }

        for (int i = 0; i < damageEffectObjects.Count; i++)
        {
            (Collider2D[] colliders, Damage damage) = damageEffectObjects[i].TickDamage();
            DamageArgs damageArgs = new(damage, false, damageEffectObjects[i].GetEffectOwnerStats(), null, DamageArgs.DamageSource.AOE);

            for (int j = 0; j < colliders.Length; j++)
            {
                hitObjectsAndDamageToDeal.TryAdd(colliders[j], damageArgs);
            }
        }

        foreach (var hit in hitObjectsAndDamageToDeal)
        {
            if (hit.Key.TryGetComponent(out CH_Stats enemyStats))
            {
                DamageArgs damageArgs = hit.Value;

                damageArgs.EnemyStats = enemyStats;
                damageArgs.ShooterStats.DamageFilter.OutgoingDAMAGE(damageArgs);
            }
        }

        hitObjectsAndDamageToDeal.Clear();
    }

    public static void AddDamageEffectToList(IDamageEffect damageEffect)
    {
        instance.damageEffectObjects.Add(damageEffect);
    }

    public static void RemoveDamageEffectFromList(IDamageEffect damageEffect)
    {
        instance.damageEffectObjects.Remove(damageEffect);
    }
}
