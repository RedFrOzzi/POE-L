using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class Effects
{
    public static class ExplosionEffect
    {
        //Explosion triggers OnHit effects
        public static void Start(Vector2 center, float radius, LayerMask layerMask, DamageArgs damageArgs)
        {
            var colliders = Physics2D.OverlapCircleAll(center, radius, layerMask);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<CH_Stats>(out CH_Stats stats))
                {
                    damageArgs.EnemyStats = stats;

                    Effects.NonProjDamage.DealMagicDamage(damageArgs);
                }
            }
        }
    }

    public static class ChainOfExplosionsEffect
    {
        private static GameObject explosionWithDelayObject;
        private static Transform effectParent;

        //Does not trigger OnHitEffects
        public static void Start(Vector2 center, float radius, DamageArgs damageArgs, int iterations, float delay, LayerMask layerMask, string explosionAnimation, string impactAnimation)
        {
            if (explosionWithDelayObject == null)
            {
                explosionWithDelayObject = Resources.Load<GameObject>("Effects/ExplosionWithDelayObject");
                effectParent = GameObject.FindGameObjectWithTag("Environment").transform;
            }

            var effect = GameObject.Instantiate(explosionWithDelayObject, center, Quaternion.identity, effectParent);
            effect.GetComponent<ChainOfExplosions>().SetUpEffect(radius, damageArgs, iterations, delay, layerMask, explosionAnimation, impactAnimation);
        }
    }

    public static class GroundEffect
    {
        private static GameObject molotovGroundEffectPrefab;
        private static GameObject speedGroundEffectPrefab;
        private static GameObject posionGroundEffectPrefab;
        private static GameObject damageEffectManagerPrefab;
        private static Transform effectParent;

        public static MolotovGroundEffect StartFireGround(Vector2 position, float radius, float duration, CH_Stats effectOwnerStats, LayerMask layerMask, Damage damagePerSec)
        {
            if (molotovGroundEffectPrefab == null)
            {
                molotovGroundEffectPrefab = Resources.Load<GameObject>("Effects/MolotovGroundEffect");
                effectParent = GameObject.FindGameObjectWithTag("Environment").transform;
            }

            if (damageEffectManagerPrefab == null)
            {
                damageEffectManagerPrefab = Resources.Load<GameObject>("Effects/DamageEffectManager");
                GameObject.Instantiate(damageEffectManagerPrefab);
            }

            var effect1 = GameObject.Instantiate(molotovGroundEffectPrefab, position, Quaternion.identity, effectParent);
            var pge = effect1.GetComponent<MolotovGroundEffect>();
            pge.SetUpEffect(effectOwnerStats, radius, duration, damagePerSec, layerMask);
            pge.AddToManagerList();
            return pge;
        }

        public static PoisonGroundEffect StartPoisonGroundEffect(Vector2 position, float radius, float duration, CH_Stats effectOwnerStats, LayerMask layerMask, Damage damagePerSec)
        {
            if (posionGroundEffectPrefab == null)
            {
                posionGroundEffectPrefab = Resources.Load<GameObject>("Effects/PoisonGroundEffect");
                effectParent = GameObject.FindGameObjectWithTag("Environment").transform;
            }

            if (damageEffectManagerPrefab == null)
            {
                damageEffectManagerPrefab = Resources.Load<GameObject>("Effects/DamageEffectManager");
                GameObject.Instantiate(damageEffectManagerPrefab);
            }

            var effect1 = GameObject.Instantiate(posionGroundEffectPrefab, position, Quaternion.identity, effectParent);
            var pge = effect1.GetComponent<PoisonGroundEffect>();
            pge.SetUpEffect(effectOwnerStats, radius, duration, damagePerSec, layerMask);
            pge.AddToManagerList();
            return pge;
        }

        public static void StartSpeed(Vector2 position, float radius, float duration, CH_Stats sourseStats, LayerMask layerMask, float speed, string nameID)
        {
            if (speedGroundEffectPrefab == null)
            {
                speedGroundEffectPrefab = Resources.Load<GameObject>("Effects/SpeedGroundEffect");
                effectParent = GameObject.FindGameObjectWithTag("Environment").transform;
            }

            if (damageEffectManagerPrefab == null)
            {
                damageEffectManagerPrefab = Resources.Load<GameObject>("Effects/DamageEffectManager");
                GameObject.Instantiate(damageEffectManagerPrefab);
            }

            var effect2 = GameObject.Instantiate(speedGroundEffectPrefab, position, Quaternion.identity, effectParent);
            effect2.GetComponent<SpeedGroundEffect>().SetUpEffect(sourseStats, radius, speed, duration, layerMask, nameID);
        }
    }

    public static class NonProjDamage
    {
        public static void DealDamage(DamageArgs damageArgs)
        {
            damageArgs.ShooterStats.DamageFilter.OutgoingAttackHIT(damageArgs);
        }
        public static void DealMagicDamage(DamageArgs damageArgs)
        {
            damageArgs.ShooterStats.DamageFilter.OutgoingSpellHIT(damageArgs);
        }
    }
}