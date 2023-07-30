using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Enemy")]
    [SerializeField] private float resetDistance = 200;
    [SerializeField] private float resetDOTDistance = 11f;
    [SerializeField] private float resetEnemyPositionMultiplier = 100;
    [SerializeField] private float navUpdateCD = 0.2f;
    
    [Space(10)]
    [SerializeField] private List<GameObject> enemyMeleePrefabs;
    [SerializeField] private List<GameObject> enemyRangePrefabs;
    [SerializeField] private List<GameObject> enemySpecialPrefabs;
    [SerializeField] private List<GameObject> enemyElitePrefabs;
    [SerializeField] private List<GameObject> enemyBossPrefabs;

    [SerializeField, Space(10)] private float minutesForVisualWave;
    private float currentTimeForVisualWave;

    private int indexOfEnemiesWaves;

    [SerializeField, Space(10), Header("Elite Enemies")] private float minutesTillElites;
    private float nextTimeForElites;

    [SerializeField, Space(5)] private float eliteSizeMultiplier;
    [SerializeField] private float eliteDamageMultiplier;
    [SerializeField] private float eliteHealthMultiplier;
    [SerializeField] private float eliteDefanceMultiplier;
    [SerializeField] private float eliteAddedRange;

    private int indexOfElites;

    private Transform player;
    private CH_Stats playerStats;
    private GlobalEnemyModifiers globalEnemyModifiers;
    private GameFlowManager gameFlowManager;

    [Space(10), Header("Dummy")]
    [SerializeField] private GameObject trainingDummyPrefab;

    [Header("System")]
    [SerializeField] private GameObject enemyParentObject;
    [SerializeField] private Transform projectileParent;
    [SerializeField] private GameObject phisicalProjPrefab;
    [SerializeField] private GameObject magicProjPrefab;
    [SerializeField] private float spawnDistance = 10;
    [SerializeField, Space(10)] private float phaseTime = 30;
    [SerializeField, Space(10)] private int liveEnemies = 0;
    [SerializeField] private int spawnCount = 0;

    [Header("Phase stats")]
    [SerializeField, Space(10)] private List<int> liveEnemiesInPhase;   //Phase is index
    [SerializeField, Space(10)] private List<int> attackDamageInPhase;  //Phase is index
    [SerializeField] private List<int> spellDamageInPhase;              //Phase is index
    [SerializeField] private List<int> healthInPhase;                   //Phase is index
    [SerializeField] private List<int> armorInPhase;                    //Phase is index
    [SerializeField] private List<int> megicResistInPhase;              //Phase is index

    public int SpawnPhase { get; private set; }

    private bool isSpawnBegan = false;   
    private float spawnTime;
    private const float spawnCD = 0.2f;

    private bool isSpawnPhaseInProcess = false;
    private float phaseCumulativeTime;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        globalEnemyModifiers = GlobalEnemyModifiers.Instance;
        gameFlowManager = GameFlowManager.Instance;

        enemyMeleePrefabs = new(Resources.LoadAll<GameObject>("Enemies/MeleeEnemies"));
        enemyRangePrefabs = new(Resources.LoadAll<GameObject>("Enemies/RangedEnemies"));
        enemySpecialPrefabs = new(Resources.LoadAll<GameObject>("Enemies/SpecialEnemies"));
        enemyElitePrefabs = new(Resources.LoadAll<GameObject>("Enemies/ElitesEnemies"));
        enemyBossPrefabs = new(Resources.LoadAll<GameObject>("Enemies/BossEnemies"));

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<CH_Stats>();

        liveEnemies = 0;
        spawnCount = 0;
        indexOfEnemiesWaves = 0;
        currentTimeForVisualWave = 0;

        nextTimeForElites = minutesTillElites;
    }


    private void Update()
    {
        if (gameFlowManager.IsGamePaused) { return; }

        if (!isSpawnBegan) { return; }

        PhaseCheck();

        if (!isSpawnPhaseInProcess) { return; }

        if (spawnTime > Time.time) { return; }

        spawnTime = spawnCD + Time.time;

        RegularSpawnEnemies();

        SpawnEliteEnemyIfNeeded();
    }

    
    public void StartSpawn()
    {
        BeginEnemiesSpawn();
    }

    public void StartEndlessSpawn()
    {
        isSpawnBegan = true;
        SpawnPhase = 5;
        spawnTime = 0;
        isSpawnPhaseInProcess = true;
    }
    
    public void StopEndlessSpawn()
    {
        isSpawnPhaseInProcess = false;
    }

    public void OnEnemyDeath()
    {
        liveEnemies--;
    }

    private void RegularSpawnEnemies()
    {
        GameObject enemy = Instantiate(MeleeEnemyFromPool(), RandomPointAroundScreen(), Quaternion.identity, enemyParentObject.transform);

        enemy.GetComponent<EnemySetup>().OnSpawnSetup(player, resetDistance, resetDOTDistance, resetEnemyPositionMultiplier, navUpdateCD, projectileParent, phisicalProjPrefab);

        var stats = enemy.GetComponent<CH_Stats>();

        AddPhaseBaseStatsToEnemy(stats);

        stats.GSC.CombineChanges(globalEnemyModifiers.ESC);

        globalEnemyModifiers.AddAliveEnemy(stats);

        liveEnemies++;

        spawnCount++;
    }

    private void SpawnEliteEnemyIfNeeded()
    {
        if (gameFlowManager.GetCurrentRoundTimeInMinutes() < nextTimeForElites) { return; }
        
        indexOfElites++;

        nextTimeForElites += minutesTillElites;

        GameObject enemy = Instantiate(EliteEnemyFromPool(), RandomPointAroundScreen(), Quaternion.identity, enemyParentObject.transform);

        enemy.GetComponent<EnemySetup>().OnSpawnSetup(player, resetDistance, resetDOTDistance, resetEnemyPositionMultiplier, navUpdateCD, projectileParent, phisicalProjPrefab);

        var stats = enemy.GetComponent<CH_Stats>();

        AddPhaseBaseStatsToEnemy(stats);

        stats.CharacterType = CharacterType.Elite;
        stats.ChangeBaseDamage(stats.BaseMinDamage * eliteDamageMultiplier, stats.BaseMaxDamage * eliteDamageMultiplier);
        stats.ChangeBaseHealth(stats.BaseHP * eliteHealthMultiplier);
        stats.ChangeBaseArmorAndResist(stats.BaseArmor * eliteDefanceMultiplier, stats.BaseMagicResist * eliteDefanceMultiplier);
        stats.ChangeBaseAttackRange(stats.BaseAttackRange + eliteAddedRange);
        enemy.transform.localScale *= eliteSizeMultiplier;

        stats.GSC.CombineChanges(globalEnemyModifiers.ESC);

        globalEnemyModifiers.AddAliveEnemy(stats);

        liveEnemies++;

        spawnCount++;
    }

    public void SpawnBossEnemy()
    {
        GameObject enemy = Instantiate(BossEnemyFromPool(), RandomPointAroundScreen(), Quaternion.identity, enemyParentObject.transform);

        enemy.GetComponent<EnemySetup>().OnSpawnSetup(player, resetDistance, resetDOTDistance, resetEnemyPositionMultiplier, navUpdateCD, projectileParent, phisicalProjPrefab);

        var stats = enemy.GetComponent<CH_Stats>();

        AddPhaseBaseStatsToEnemy(stats);

        stats.GSC.CombineChanges(globalEnemyModifiers.ESC);

        globalEnemyModifiers.AddAliveEnemy(stats);

        liveEnemies++;

        spawnCount++;
    }

    public void SpawnTrainingDummy()
    {
        GameObject enemy = Instantiate(trainingDummyPrefab, new Vector3(0, 0, 0), Quaternion.identity, enemyParentObject.transform);

        enemy.GetComponent<EnemySetup>().OnSpawnSetup(player, resetDistance, resetDOTDistance, resetEnemyPositionMultiplier, navUpdateCD, projectileParent, phisicalProjPrefab);
    }


    private GameObject MeleeEnemyFromPool()
    {
        if (gameFlowManager.GetCurrentRoundTimeInMinutes() > currentTimeForVisualWave)
        {
            indexOfEnemiesWaves++;

            currentTimeForVisualWave += minutesForVisualWave;
        }

        if (indexOfEnemiesWaves >= enemyMeleePrefabs.Count)
        {
            indexOfEnemiesWaves %= enemyMeleePrefabs.Count;
        }

        return enemyMeleePrefabs[indexOfEnemiesWaves];
    }

    private GameObject EliteEnemyFromPool()
    {
        if (indexOfElites >= enemyMeleePrefabs.Count)
        {
            indexOfElites %= enemyMeleePrefabs.Count;
        }

        return enemyMeleePrefabs[indexOfElites];
    }

    private GameObject BossEnemyFromPool()
    {
        return enemyBossPrefabs[Random.Range(0, enemyBossPrefabs.Count)];
    }

    private Vector2 RandomPointAroundScreen()
    {
        return (Vector2)(player.position + (Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 359f))) * Vector2.right * spawnDistance));        
    }

    private void BeginEnemiesSpawn()
    {
        isSpawnBegan = true;
        SpawnPhase = 0;
        spawnTime = 0;
        isSpawnPhaseInProcess = true;
    }

    private void PhaseCheck()
    {
        if (liveEnemies >= liveEnemiesInPhase[SpawnPhase])
        {
            isSpawnPhaseInProcess = false;
        }
        else
        {
            isSpawnPhaseInProcess = true;
        }

        if (gameFlowManager.GetCurrentRoundTimeInSeconds() > phaseCumulativeTime)
        {
            phaseCumulativeTime += phaseTime;

            SpawnPhase++;
            SpawnPhase = Mathf.Clamp(SpawnPhase, 0, liveEnemiesInPhase.Count - 1);

            Debug.Log($"Current spawn phase is {SpawnPhase}");
        }
    }

    private void AddPhaseBaseStatsToEnemy(CH_Stats stats)
    {
        stats.ChangeBaseDamage(stats.BaseMinDamage + attackDamageInPhase[SpawnPhase], stats.BaseMaxDamage + attackDamageInPhase[SpawnPhase]);
        stats.ChangeBaseHealth(stats.BaseHP + healthInPhase[SpawnPhase]);
        stats.ChangeBaseArmorAndResist(stats.BaseArmor + armorInPhase[SpawnPhase], stats.BaseMagicResist + megicResistInPhase[SpawnPhase]);

        stats.GSC.MagicSC.AddFlatSpellDamage(spellDamageInPhase[SpawnPhase]);
    }
}