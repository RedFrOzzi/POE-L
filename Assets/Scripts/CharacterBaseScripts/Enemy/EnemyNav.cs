using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    private Transform targetTransform;
    public Vector2 LastForwardDirection { get; private set; } = Vector2.zero;
    public Vector2 CurrentForwardDirection { get; private set; } = Vector2.zero;

    private float resetDistance = 200;
    private float resetDOTDistance = 11f;
    private float resetEnemyPositionMultiplier = 100;

       
    private CH_Stats stats;
    private CH_AdditionalEffects additionalEffects;
    private SpriteRenderer spriteRenderer;
    private CH_Animation cH_Animation;
    private float navUpdateCD = 0.2f;
    private float navUpdate;
    private NavMeshAgent agent;
    private const float distanceCheckCD = 1.5f;
    private float nextDistanceCheck;
    private bool turnedRight = true;
    private Vector3 curentSteeringTarget;
    private EnemyBehaviour behaviour;
    private Transform projectileParent;
    private GameObject projectilePrefab;

    private void Awake()
    {
        stats = GetComponent<CH_Stats>();
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        additionalEffects = GetComponent<CH_AdditionalEffects>();
        cH_Animation = GetComponent<CH_Animation>();
        behaviour = GetComponent<EnemyBehaviour>();

        stats.OnStatsChange += OnStatsChange;

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    

    private void OnEnable()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void OnDestroy()
    {
        stats.OnStatsChange -= OnStatsChange;
    }


    private void Update()
    {
        if (GameFlowManager.Instance.IsGamePaused) { return; }

        ResetPositionIfFar();

        AttackIfPossible();

        if (navUpdate > Time.time) { return; }

        navUpdate = Time.time + navUpdateCD;

        FindTargetAndMove();
    }

    public void SetUpNavigation(Transform targetTransform, float resetDistance, float resetDOTDistance, float resetEnemyPositionMultiplier, float navUpdateCD, Transform projectileParent, GameObject projectilePrefab)
    {
        this.targetTransform = targetTransform;
        this.resetDistance = resetDistance;
        this.resetEnemyPositionMultiplier = resetEnemyPositionMultiplier;
        this.navUpdateCD = navUpdateCD;
        this.projectileParent = projectileParent;
        this.projectilePrefab = projectilePrefab;
        this.resetDOTDistance = resetDOTDistance;
    }

    private void AttackIfPossible()
    {
        behaviour.Attack(targetTransform.position);
    }

    private void FindTargetAndMove()
    {
        SetUpLastAndCurrentDirections();

        behaviour.Moving(targetTransform.position);
    }

    private void ResetPositionIfFar()
    {
        if(nextDistanceCheck > Time.time) { return; }

        nextDistanceCheck = Time.time + distanceCheckCD;

        Vector2 dir = targetTransform.position - transform.position;

        if (dir.magnitude > resetDistance || Mathf.Abs(Vector2.Dot(dir, Vector2.up)) > 11)
        {
            transform.position = (Vector2)transform.position + dir * resetEnemyPositionMultiplier;
        }
    }
    
    private void SetUpLastAndCurrentDirections()
    {
        try
        {
            curentSteeringTarget = agent.steeringTarget;
            LastForwardDirection = curentSteeringTarget.normalized;

            //---Flip-Speite---

            FlipSpriteIfNeeded();

            //-----------------

            if (stats.IsControllable == false || stats.CanMove == false)
            {
                CurrentForwardDirection = Vector2.zero;
            }
            else
            {
                CurrentForwardDirection = LastForwardDirection;
            }
        }
        catch
        {
            Debug.Log("Shits goes down. Last or current direction cant be set");
        }
    }

    public void SetUpBehaviour()
    {
        behaviour.SetUpBehaviour(cH_Animation, agent, stats, additionalEffects, stats.CharacterType, projectilePrefab, projectileParent);
    }
    
    private void OnStatsChange()
    {
        agent.speed = stats.CurrentMovementSpeed;
    }

    private void FlipSpriteIfNeeded()
    {
        if (Vector2.Dot(curentSteeringTarget - transform.position, transform.right) < 0f && turnedRight)
        {
            spriteRenderer.flipX = true;
            turnedRight = false;
        }
        else if (Vector2.Dot(curentSteeringTarget - transform.position, transform.right) > 0f && turnedRight == false)
        {
            spriteRenderer.flipX = false;
            turnedRight = true;
        }
    }
}
