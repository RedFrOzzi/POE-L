using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourMelee : EnemyBehaviour
{
    private Coroutine attackCoroutine;

    private const float attackDetectionDivider = 1.5f;

    private Transform thisTranform;

    private void Awake()
    {
        thisTranform = transform;
    }

    private void Start()
    {
        stats.OnAbilityToShootLoss += StopAttackCorotineNoParams;
        stats.OnControllLoss += StopAttackCoroutine;
    }

    private void OnDestroy()
    {
        stats.OnAbilityToShootLoss -= StopAttackCorotineNoParams;
        stats.OnControllLoss -= StopAttackCoroutine;
    }

    public override void Moving(Vector2 movingTo)
    {
        if (stats.IsControllable == false || stats.CanMove == false) { return; }

        if (agent.SetDestination(movingTo) == false) { return; }

        if (IsAttacking == false)
        {
            if (stats.CurrentForwardDirection != Vector2.zero && IsMoving == false)
            {
                cH_Animation.PlayWalk();
                IsMoving = true;
            }
            else if (stats.CurrentForwardDirection == Vector2.zero && IsMoving)
            {
                cH_Animation.PlayIdle();
                IsMoving = false;
            }
        }
        else
        {
            IsMoving = false;
        }
    }

    public override void Attack(Vector2 target)
    {
        if (stats.CanShoot == false) { return; }

        if (nextHit > Time.time) { return; }

        //проверка на начало анимации (расстояние в полтора раза меньше ренжа атаки)
        if ((target - (Vector2)stats.transform.position).magnitude > stats.CurrentAttackRange / attackDetectionDivider) { return; }
        
        float attackDelay = 1 / stats.CurrentAttackSpeed;
        additionalEffects.Root(attackDelay - 0.2f); //вычитаем 0.2 чтобы рут слетал до конца атаки, иначе жопа логике

        nextHit = Time.time + attackDelay;

        attackCoroutine = StartCoroutine(AttackCoroutine(attackDelay));
    }

    private IEnumerator AttackCoroutine(float attackLengh)
    {
        IsAttacking = true;

        cH_Animation.Stop();

        if (attackLengh > cH_Animation.AttackClipLengh)
        {
            cH_Animation.PlayAttack(cH_Animation.AttackClipLengh);
        }
        else
            cH_Animation.PlayAttack(attackLengh);

        float timeTillHit = attackLengh - (attackLengh / 2);

        yield return new WaitForSeconds(timeTillHit);

        var colliders = Physics2D.OverlapCircleAll(stats.transform.position, stats.CurrentAttackRange, playerLayer);

        if (colliders.Length > 0)
        {
            if (colliders[0].TryGetComponent(out CH_Stats enemyStats))
            {
                DamageArgs da = GetDamageArgs(enemyStats);

                stats.DamageFilter.OutgoingAttackHIT(da);
            }
            else
            {
                Debug.Log($"{colliders[0].name} does not have Health script");
                yield break;
            }
        }

        yield return new WaitForSeconds(attackLengh - timeTillHit - 0.1f);

        cH_Animation.PlayWalk();

        IsAttacking = false;
    }

    private void StopAttackCoroutine(bool isControllable)
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        IsAttacking = false;

        cH_Animation.Freeze(!isControllable);
    }

    private void StopAttackCorotineNoParams()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        IsAttacking = false;
    }

    private DamageArgs GetDamageArgs(CH_Stats enemyStats)
    {
        Damage damage = new(Random.Range(stats.CurrentMinDamage, stats.CurrentMaxDamage), 0, 0);

        return new(damage, false, stats, enemyStats, DamageArgs.DamageSource.MeleeHit);
    }
}
