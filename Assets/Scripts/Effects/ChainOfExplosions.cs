using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ChainOfExplosions : MonoBehaviour
{
	public void SetUpEffect(float radius, DamageArgs damageArgs, int iterations, float delay, LayerMask layerMask, string explosionAnimation, string impactAnimation)
    {
        StartCoroutine(ChainOfExplosionsRoutine(radius, damageArgs, iterations, delay, layerMask, explosionAnimation, impactAnimation));
    }

    private IEnumerator ChainOfExplosionsRoutine(float radius, DamageArgs damageArgs, int iterations, float delay, LayerMask layerMask, string explosionAnimation, string impactAnimation)
    {
        Vector2 position = transform.position;

        List<Collider2D> hitColliders = new();

        for (int i = 0; i < iterations; i++)
        {
            AnimationPlayer.Instance.Play(explosionAnimation, position, Quaternion.identity, Vector3.one * radius, 1.5f);

            var colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);

            if (colliders.Length <= 0)
            {
                Destroy(gameObject);
                yield break;
            }

            colliders = colliders.OrderBy(col => ((Vector2)col.transform.position - position).magnitude).ToArray();

            for (int j = 0; j < colliders.Length; j++)
            {
                if (colliders[j].TryGetComponent<CH_Stats>(out CH_Stats stats))
                {
                    damageArgs.EnemyStats = stats;

                    //Первый взрыв с OnHit последующие без
                    if (i == 0)
                    {
                        hitColliders.Add(colliders[0]);
                        Effects.NonProjDamage.DealDamage(damageArgs);
                    }
                    else
                    {
                        damageArgs.ShooterStats.DamageFilter.OutgoingDAMAGE(damageArgs);
                    }

                    //Visuals
                    AnimationPlayer.Instance.Play(impactAnimation, colliders[j].transform.position, Quaternion.identity, Vector3.one, 1.5f);
                }
            }

            //Проверка всех оставшихся целей на предмет их разрушения до окончания цикла
            for (int l = 0; l < colliders.Length; l++)
            {
                if (colliders[l] == null || hitColliders.Contains(colliders[l]))
                {
                    if (l == colliders.Length - 1)
                    {
                        Destroy(gameObject);
                        yield break;
                    }

                    continue;
                }
                else
                {
                    position = colliders[l].transform.position;
                    hitColliders.Add(colliders[l]);
                    break;
                }
            }

            yield return new WaitForSeconds(delay);
        }
        
        Destroy(gameObject);
    }
}
