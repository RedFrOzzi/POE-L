using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainLightningGameObject : MonoBehaviour
{
    public bool IsActive { get; private set; }

    private readonly float newTileTimeDelay = 0.05f;
    private float delay;

    private LineRenderer lineRenderer;

    private const float materialOffset = 0.125f;
    private float currentOffset = 0;

    private bool isShown;
    private readonly WaitForSeconds showTime = new(0.15f);

    private List<Collider2D> hitTargets;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        hitTargets = new();
    }

    private void Update()
    {
        if (isShown == false) { return; }

        if (delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }

        delay = newTileTimeDelay;

        if (currentOffset >= 1)
            currentOffset = 0;

        lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, currentOffset));

        currentOffset += materialOffset;
    }

    public void StartChainLightning(Vector2 casterPos, Vector2 startPosition, DamageArgs damageArgs, int chainsAmount, float seekRadius, float maxRange, float endOfChainMulti)
    {
        IsActive = true;

        transform.position = casterPos;

        StartCoroutine(ChainCoroutine(casterPos + Vector2.ClampMagnitude(startPosition - casterPos, maxRange), chainsAmount, seekRadius, damageArgs, endOfChainMulti));
    }    

    private IEnumerator ChainCoroutine(Vector2 startPosition, int chainsAmount, float seekRadius, DamageArgs damageArgs, float endOfChainMulti)
    {
        Vector2 cashedNewPos = Vector2.one;
        Vector2 cashedOldPos = Vector2.one;

        Collider2D lastHitedCillider = null;

        for (byte i = 0; i <= chainsAmount; i++)
        {
            var target = FindTarget(startPosition, seekRadius);
            if (target == null)
            {
                if (i == 0)
                {
                    isShown = true;
                    lineRenderer.enabled = true;

                    lineRenderer.SetPositions(new Vector3[] { transform.position, startPosition, startPosition });

                    yield return showTime;

                    lineRenderer.enabled = false;
                    isShown = false;

                    break;
                }
                
                if (lastHitedCillider != null && TryGetComponent(out CH_Stats enemyStats))
                {
                    //Дополнительный урон в зависимости от оствшихся чейнов
                    damageArgs.EnemyStats = enemyStats;
                    damageArgs.Damage *= endOfChainMulti * (chainsAmount - i);
                    damageArgs.ShooterStats.DamageFilter.OutgoingDAMAGE(damageArgs);
                }

                break;
            }

            if (i == 0)
            {
                lineRenderer.SetPositions(new Vector3[] { transform.position, target.transform.position, target.transform.position });
                cashedOldPos = target.transform.position;
                startPosition = cashedOldPos;
                lastHitedCillider = target;
            }
            else if (i == 1)
            {
                lineRenderer.SetPositions(new Vector3[] { transform.position, cashedOldPos, target.transform.position });
                cashedNewPos = target.transform.position;
                startPosition = cashedNewPos;
                lastHitedCillider = target;
            }
            else
            {
                lineRenderer.SetPositions(new Vector3[] { cashedOldPos, cashedNewPos, target.transform.position });
                cashedOldPos = cashedNewPos;
                cashedNewPos = target.transform.position;
                startPosition = cashedNewPos;
                lastHitedCillider = target;
            }

            OnHitEffectAndDamage(target, damageArgs);

            isShown = true;
            lineRenderer.enabled = true;

            yield return showTime;
        }

        lineRenderer.enabled = false;
        isShown = false;

        hitTargets.Clear();

        IsActive = false;
    }

    private Collider2D FindTarget(Vector2 startPos, float radius)
    {
        var colliders = Physics2D.OverlapCircleAll(startPos, radius, MagicBehaviour.EnemyLayerMask);
        colliders = colliders.Where(x => hitTargets.Contains(x) == false).ToArray();

        if (colliders.Length > 0)
        {
            var target = colliders.PickRandom();

            hitTargets.Add(target);

            return target;
        }
        else
        {
            return null;
        }
    }

    private void OnHitEffectAndDamage(Collider2D collider, DamageArgs damageArgs)
    {
        if (collider.TryGetComponent(out CH_Stats enemyStats))
        {
            //ANIMATION------------------------------------
            AnimationPlayer.Instance.Play("LightningImpact_01", collider.transform.position, Quaternion.identity, Vector3.one, 2f);
            //---------------------------------------------

            damageArgs.EnemyStats = enemyStats;

            damageArgs.ShooterStats.DamageFilter.OutgoingSpellHIT(damageArgs);
        }
    }
}
