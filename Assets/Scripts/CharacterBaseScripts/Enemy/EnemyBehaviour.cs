using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    protected CH_AdditionalEffects additionalEffects;
    protected CharacterType characterType;
    protected NavMeshAgent agent;
    protected CH_Stats stats;
    protected CH_Animation cH_Animation;
    protected LayerMask playerLayer;

    protected GameObject projectilePrefab;
    protected Transform projectileParent;

    protected float nextHit = 0;

    public bool IsAttacking { get; protected set; } = false;
    public bool IsMoving { get; protected set; } = false;
    


    public void SetUpBehaviour(CH_Animation cH_Animation, NavMeshAgent agent, CH_Stats stats, CH_AdditionalEffects additionalEffects, CharacterType characterType, GameObject projectilePrefab, Transform projectileParent)
    {
        this.additionalEffects = additionalEffects;
        this.characterType = characterType;
        this.stats = stats;
        this.agent = agent;
        this.cH_Animation = cH_Animation;
        this.projectilePrefab = projectilePrefab;
        this.projectileParent = projectileParent;
        playerLayer = LayerMask.GetMask("Player");
        nextHit = 0;
    }

    public virtual void Moving(Vector2 movingTo) { }
    public virtual void Attack(Vector2 target) { }
}
