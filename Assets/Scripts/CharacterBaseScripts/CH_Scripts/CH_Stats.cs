using UnityEngine;
using UnityEngine.AI;
using System;

public class CH_Stats : MonoBehaviour
{
    public StatsChanges GSC { get; private set; } = new();

    public CH_Health Health { get; private set; }
    public CH_AdditionalEffects AdditionalEffects { get; private set; }
    public CH_OnHit OnHit { get; private set; }
    public CH_BuffManager BuffManager { get; private set; }
    public CH_AbilitiesManager AbilitiesManager { get; private set; }
    public Equipment Equipment { get; private set; }
    public Transform Aim { get; private set; }
    public CH_Mana ManaComponent { get; private set; }
    public EnemyNav EnemyNav { get; private set; }
    public CH_Experience Experience { get; private set; }
    public CH_Animation CH_Animation { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public CH_TalentTree TalentTree { get; private set; }
    public CH_DamageFilter DamageFilter { get; private set; }
    public ItemCollector ItemCollector { get; private set; }

    public CH_AbilitiesSwaper AbilitiesSwaper { get; private set; }

    private NavMeshAgent navMeshAgent;
    private CharacterController2D characterController;
    private Rigidbody2D rb2D;

    public event Action OnStatsChange;
    public event Action OnLevelUp;
    public event Action<bool> OnControllLoss;
    public event Action OnMovementLoss;
    public event Action OnAbilityToShootLoss;
    public event Action OnAbilityToCastLoss;
    public event Action<int> OnGoldChange;

    private void Awake()
    {
        Health = GetComponent<CH_Health>();
        AdditionalEffects = GetComponent<CH_AdditionalEffects>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyNav = GetComponent<EnemyNav>();
        characterController = GetComponent<CharacterController2D>();
        rb2D = GetComponent<Rigidbody2D>();
        OnHit = GetComponent<CH_OnHit>();
        BuffManager = GetComponent<CH_BuffManager>();
        AbilitiesManager = GetComponent<CH_AbilitiesManager>();
        Equipment = GetComponent<Equipment>();
        Aim = GameObject.FindGameObjectWithTag("Aim").transform;
        ManaComponent = GetComponent<CH_Mana>();
        Experience = GetComponent<CH_Experience>();
        CH_Animation = GetComponent<CH_Animation>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TalentTree = GetComponent<CH_TalentTree>();
        DamageFilter = GetComponent<CH_DamageFilter>();
        AbilitiesSwaper = GetComponent<CH_AbilitiesSwaper>();
        ItemCollector = GetComponentInChildren<ItemCollector>();
        
        GSC.OnStatsChange += EvaluateStats;
    }

    private void OnDestroy()
    {
        GSC.OnStatsChange -= EvaluateStats;
    }


    [field: Header("Character Type")]
    public CharacterType CharacterType { get; set; }

    //------------------------------------------------------------------------------------------
 
    public float BaseMinDamage { get; private set; }
    [field: SerializeField, Space(10), Header("Offensive")] 
    public float CurrentMinDamage { get; private set; }

    public float BaseMaxDamage { get; private set; }
    [field: SerializeField] public float CurrentMaxDamage { get; private set; }

    public float BaseAttackSpeed { get; private set; }
    [field: SerializeField] public float CurrentAttackSpeed { get; private set; }

    public float BaseCritChance { get; private set; }
    [field: SerializeField] public float CurrentCritChance { get; private set; }

    public float BaseAttackCritMultiplier { get; private set; }
    [field: SerializeField] public float CurrentAttackCritMultiplier{ get; private set; }

    [field: SerializeField] public float BaseSpellCritMultiplier { get; private set; }

    public float BaseAttackRange { get; private set; }
    [field: SerializeField] public float CurrentAttackRange { get; private set; }

    public float BaseReloadSpeed { get; private set; }
    [field: SerializeField] public float CurrentReloadSpeed{ get; private set; }

    public int BaseAmmoCapacity { get; private set; }
    [field: SerializeField] public int CurrentAmmoCapacity { get; private set; }

    [field: SerializeField] public int CurrentAmmo { get; private set; }

    public float BaseAccuracy { get; private set; }
    [field: SerializeField] public float CurrentAccuracy { get; private set; }
    [SerializeField] private AnimationCurve FlatAccuracyToPercent;
    [field: SerializeField] public float PercentAccuracy { get; private set; }

    public float BaseSpreadAngle { get; private set; }
    [field: SerializeField] public float CurrentSpreadAngle { get; private set; }
    //------------------------------------------------------------------------

    //количество взаимодйствий с врагами до разрушеия прожектайла
    public int BaseWeaponChainsAmount { get; private set; }
    [field: SerializeField] public int WeaponChainsAmount { get; private set; }

    //количество взаимодйствий с врагами до разрушеия прожектайла
    public int BaseWeaponPierceAmount { get; private set; }
    [field: SerializeField] public int WeaponPierceAmount { get; private set; }

    //колличество прожектайлов при атаке
    public int BaseWeaponProjectileAmount { get; private set; }
    [field: SerializeField] public int WeaponProjectileAmount { get; private set; }
    //---------------------------------------------------------------------------------------------

    public float BaseArmor { get; private set; }
    [field: SerializeField, Space(10), Header("Defansive")] 
    public float CurrentArmor { get; private set; }
    /*[ReadOnlyField]*/ public AnimationCurve FlatArmorToPercent;
    [field: SerializeField] public float PercentPhisicalResistance { get; private set; }

    public float BaseMagicResist { get; private set; }
    [field: SerializeField, Space(10)] public float CurrentMagicResist { get; private set; }
    [SerializeField] private AnimationCurve FlatMResistToPercent;
    [field: SerializeField] public float PercentMagicResistance { get; private set; }

    public float BaseHP { get; private set; }
    [field: SerializeField] public float MaxHP { get; private set; }
    [field: SerializeField] public float CurrentHP { get; private set; }

    public float BaseHPRegeneration { get; private set; }
    [field: SerializeField] public float CurrentHPRegeneration { get; private set; }

    public float BaseMana { get; private set; }
    [field: SerializeField] public float MaxMana { get; private set; }
    [field: SerializeField] public float CurrentMana { get; private set; }

    public float BaseManaRegeneration { get; private set; }
    [field: SerializeField] public float CurrentManaRegeneration { get; private set; }

    //-----------------------------------------------------------------------------------------------------

    [field: SerializeField, Space(10), Header("Utility")]
    public int CurrentLevel { get; private set; }

    [field:SerializeField] public int CurrentGold { get; private set; }

    public float BaseGoldGainMultiplier { get; private set; }
    [field: SerializeField] public float CurrentGoldGainMultiplier { get; private set; }

    public float BaseExperienceMultiplier { get; private set; }
    [field: SerializeField] public float CurrentExperienceMultiplier { get; private set; }

    public float BaseMovementSpeed { get; private set; }
    [field: SerializeField] public float CurrentMovementSpeed { get; private set; }

    public float BaseCollectorRadius { get; private set; }
    [field: SerializeField] public float CurrentCollectorRadius { get; private set; }

    public float BaseBuffPower { get; private set; }
    [field: SerializeField] public float CurrentBuffPower { get; private set; }

    public float BaseEffectDuration { get; private set; }
    [field: SerializeField] public float CurrentEffectDuration { get; private set; }

    public float BaseGlobalAOEMultiplier { get; private set; }
    [field: SerializeField] public float CurrentGlobalAOEMultiplier { get; private set; }

    public float BaseHealingAmplifier { get; private set; }
    [field: SerializeField] public float CurrentHealingAmplifier { get; private set; }

    public float BaseProjectileSpeed { get; private set; }
    [field: SerializeField] public float CurrentProjectileSpeed { get; private set; }

    [HideInInspector] public bool IsControllable { get; private set; } = true;
    [HideInInspector] public bool CanMove { get; private set; } = true;
    [HideInInspector] public bool CanCast { get; private set; } = true;
    [HideInInspector] public bool CanShoot { get; private set; } = true;
    [HideInInspector] public bool IsImmune { get; private set; } = false;


    [HideInInspector]
    public Vector2 LastForwardDirection
    {
        get
        {
            if (characterController != null)
            {
                return characterController.LastForwardDirection;
            }
            else
            {
                return EnemyNav.LastForwardDirection;
            }
        }
        private set { }
    }
    [HideInInspector]
    public Vector2 CurrentForwardDirection
    {
        get
        {
            if (characterController != null)
            {
                return characterController.CurrentForwardDirection;
            }
            else
            {
                return EnemyNav.CurrentForwardDirection;
            }
        }
        private set { }
    }



    public void SetUpCurrentStatsToBase()
    {
        EvaluateStats();

        if (Experience != null)
            Experience.Initialize(this);
    }

    public void EvaluateStats()
    {
        //---AA_DAMAGE--------------------------
        CurrentMinDamage = (BaseMinDamage + GSC.AttackSC.FlatAttackDamageValue) *
            (1 + GSC.GlobalSC.IncreaseDamageValue + GSC.GlobalSC.IncreasePhysicalDamageValue + GSC.AttackSC.IncreaseAttackDamageValue) *
            (GSC.GlobalSC.MoreDamageValue * GSC.GlobalSC.MorePhysicalDamageValue * GSC.AttackSC.MoreAttackDamageValue) *
            (GSC.GlobalSC.LessDamageValue * GSC.GlobalSC.LessPhysicalDamageValue * GSC.AttackSC.LessAttackDamageValue);

        CurrentMaxDamage = (BaseMaxDamage +  GSC.AttackSC.FlatAttackDamageValue) *
            (1 + GSC.GlobalSC.IncreaseDamageValue + GSC.GlobalSC.IncreasePhysicalDamageValue + GSC.AttackSC.IncreaseAttackDamageValue) *
            (GSC.GlobalSC.MoreDamageValue * GSC.GlobalSC.MorePhysicalDamageValue * GSC.AttackSC.MoreAttackDamageValue) *
            (GSC.GlobalSC.LessDamageValue * GSC.GlobalSC.LessPhysicalDamageValue * GSC.AttackSC.LessAttackDamageValue);
        //---AA_DAMAGE_END----------------------

        //---WEAPON-PROJECTILES-----------------
        WeaponProjectileAmount = BaseWeaponProjectileAmount + GSC.AttackSC.FlatWeaponProjectileAmountValue;
        WeaponPierceAmount = BaseWeaponPierceAmount + GSC.AttackSC.FlatWeaponPierceAmountValue;
        WeaponChainsAmount = BaseWeaponChainsAmount + GSC.AttackSC.FlatWeaponChainsAmountValue;
        //---WEAPON-PROJECTILES-END-------------

        //---HP---------------------------------
        float currentPercentHP = 1 - ((MaxHP - CurrentHP) / MaxHP);
        var tempHP = (BaseHP + GSC.DefanceSC.FlatHPValue) * (1 + GSC.DefanceSC.IncreaseHPValue) * GSC.DefanceSC.MoreHPValue * GSC.DefanceSC.LessHPValue;
        MaxHP = Mathf.Clamp(tempHP, 1, float.MaxValue);
        CurrentHP = MaxHP * currentPercentHP;
        //---HP_END-----------------------------
        
        //---MANA-------------------------------
        float currentPercentMana = 1 - ((MaxMana - CurrentMana) / MaxMana);
        var tempMana = (BaseMana + GSC.MagicSC.FlatManaValue) * (1 + GSC.MagicSC.IncreaseManaValue) * GSC.MagicSC.MoreManaValue * GSC.MagicSC.LessManaValue;
        MaxMana = Mathf.Clamp(tempMana, 0, float.MaxValue);
        CurrentMana = MaxMana * currentPercentMana;
        //---MANA_END---------------------------

        //---RESISTS----------------------------
        var tempArmor = (BaseArmor + GSC.DefanceSC.FlatArmorValue) * (1 + GSC.DefanceSC.IncreaseArmorValue) * GSC.DefanceSC.MoreArmorValue * GSC.DefanceSC.LessArmorValue;
        CurrentArmor = Mathf.Clamp(tempArmor, 0, 10000);

        PercentPhisicalResistance = FlatArmorToPercent.Evaluate(CurrentArmor);

        var tempMagicResist = (BaseMagicResist + GSC.DefanceSC.FlatMagicResistValue) * (1 + GSC.DefanceSC.IncreaseMagicResistValue) * GSC.DefanceSC.MoreMagicResistValue * GSC.DefanceSC.LessMagicResistValue;
        CurrentMagicResist = Mathf.Clamp(tempMagicResist, 0, 10000);

        PercentMagicResistance = FlatMResistToPercent.Evaluate(CurrentMagicResist);
        //--------------------------------------

        //---CRIT-------------------------------
        var tempCritChance = (BaseCritChance + GSC.AttackSC.FlatAttackCritChanceValue) * (1 + GSC.AttackSC.IncreaseAttackCritChanceValue) * GSC.AttackSC.MoreAttackCritChanceValue * GSC.AttackSC.LessAttackCritChanceValue;
        CurrentCritChance = Mathf.Clamp(tempCritChance, 0, 10000);

        var tempCritMultiplier = (BaseAttackCritMultiplier + GSC.GlobalSC.FlatCritMultiplierValue + GSC.AttackSC.FlatAttackCritMultiplierValue) *
            (1 + GSC.GlobalSC.IncreaseCritMultiplierValue + GSC.AttackSC.IncreaseAttackCritMultiplierValue) *
            GSC.GlobalSC.MoreCritMultiplierValue * GSC.AttackSC.MoreAttackCritMultiplierValue *
            GSC.GlobalSC.LessCritMultiplierValue * GSC.AttackSC.LessAttackCritMultiplierValue ;
        CurrentAttackCritMultiplier = Mathf.Clamp(tempCritMultiplier, 1, 10000) / 100;
        //--------------------------------------

        var tempAccuracy = (BaseAccuracy + GSC.AttackSC.FlatAccuracyValue) * (1 + GSC.AttackSC.IncreaseAccuracyValue) * GSC.AttackSC.MoreAccuracyValue * GSC.AttackSC.LessAccuracyValue;
        CurrentAccuracy = Mathf.Clamp(tempAccuracy, 1, 10000);

        PercentAccuracy = FlatAccuracyToPercent.Evaluate(CurrentAccuracy);

        var tempAttackSpeed = BaseAttackSpeed * (1 + GSC.AttackSC.IncreaseAttackSpeedValue) * GSC.AttackSC.MoreAttackSpeedValue * GSC.AttackSC.LessAttackSpeedValue;
        CurrentAttackSpeed = Mathf.Clamp(tempAttackSpeed, 0.001f, 10000);

        var tempReloadSpeed = BaseReloadSpeed * (1 + GSC.AttackSC.IncreaseReloadSpeedValue) * GSC.AttackSC.MoreReloadSpeedValue * GSC.AttackSC.LessReloadSpeedValue;
        CurrentReloadSpeed = Mathf.Clamp(tempReloadSpeed, 0, 10000);

        var tempAmmoCapacity = (BaseAmmoCapacity + GSC.AttackSC.FlatAmmoCapacityValue) * (1 + GSC.AttackSC.IncreaseAmmoCapacityValue) * GSC.AttackSC.MoreAmmoCapacityValue * GSC.AttackSC.LessAmmoCapacityValue;
        CurrentAmmoCapacity = Mathf.Clamp((int)tempAmmoCapacity, 0, 10000);

        var tempAttackRange = BaseAttackRange * (1 + GSC.AttackSC.IncreaseAttackRangeValue) * GSC.AttackSC.MoreAttackRangeValue * GSC.AttackSC.LessAttackRangeValue;
        CurrentAttackRange = Mathf.Clamp(tempAttackRange, 0.01f, 10000);

        var tempSpreadAngle = BaseSpreadAngle * (1 + GSC.AttackSC.IncreaseSpreadAngleValue) * GSC.AttackSC.MoreSpreadAngleValue * GSC.AttackSC.LessSpreadAngleValue;
        CurrentSpreadAngle = Mathf.Clamp(tempSpreadAngle, 0.001f, 360);
        //----------------------------------------------------------------------
        var tempManaRegeneration = (BaseManaRegeneration + GSC.MagicSC.FlatManaRegenerationValue) * (1 + GSC.MagicSC.IncreaseManaRegenerationValue) * GSC.MagicSC.MoreManaRegenerationValue * GSC.MagicSC.LessManaRegenerationValue;
        CurrentManaRegeneration = Mathf.Clamp(tempManaRegeneration, 0, 10000);
        //----------------------------------------------------------------------
        var tempHPRegeneration = (BaseHPRegeneration + GSC.DefanceSC.FlatHPRegenerationValue) * (1 + GSC.DefanceSC.IncreaseHPRegenerationValue) * GSC.DefanceSC.MoreHPRegenerationValue * GSC.DefanceSC.LessHPRegenerationValue;
        CurrentHPRegeneration = Mathf.Clamp(tempHPRegeneration, 0, 10000);

        var tempHealingAmplifier = BaseHealingAmplifier * (1 + GSC.DefanceSC.IncreaseHealingAmplifierValue) * GSC.DefanceSC.MoreHealingAmplifierValue * GSC.DefanceSC.LessHealingAmplifierValue;
        CurrentHealingAmplifier = Mathf.Clamp(tempHealingAmplifier, 1, 10000);
        //----------------------------------------------------------------------
        var tempMovementSpeed = (BaseMovementSpeed + GSC.UtilitySC.FlatMovementSpeedValue) * (1 + GSC.UtilitySC.IncreaseMovementSpeedValue) * GSC.UtilitySC.MoreMovementSpeedValue * GSC.UtilitySC.LessMovementSpeedValue;
        CurrentMovementSpeed = Mathf.Clamp(tempMovementSpeed, 0.001f, 10000);

        var tempAOE = BaseGlobalAOEMultiplier * (1 + GSC.UtilitySC.IncreaseAreaValue) * GSC.UtilitySC.MoreAreaValue * GSC.UtilitySC.LessAreaValue;
        CurrentGlobalAOEMultiplier = Mathf.Clamp(tempAOE, 0.001f, 10000);

        var tempCollectorRadius = BaseCollectorRadius * (1 + GSC.UtilitySC.IncreaseCollectorRadiusValue) * GSC.UtilitySC.MoreCollectorRadiusValue * GSC.UtilitySC.LessCollectorRadiusValue;
        CurrentCollectorRadius = Mathf.Clamp(tempCollectorRadius, 1, 10000);

        var tempBuffPower = BaseBuffPower * (1 + GSC.UtilitySC.IncreaseBuffPowerValue) * GSC.UtilitySC.MoreBuffPowerValue * GSC.UtilitySC.LessBuffPowerValue;
        CurrentBuffPower = Mathf.Clamp(tempBuffPower, 0.001f, 10000);

        var tempEffectDuration = BaseEffectDuration * (1 + GSC.UtilitySC.IncreaseEffectDurationValue) * GSC.UtilitySC.MoreEffectDurationValue * GSC.UtilitySC.LessEffectDurationValue;
        CurrentEffectDuration = Mathf.Clamp(tempEffectDuration, 0.001f, 10000);

        var tempExperienceMultiplier = BaseExperienceMultiplier * (1 + GSC.UtilitySC.IncreaseExperienceValue) * GSC.UtilitySC.MoreExperienceValue * GSC.UtilitySC.LessExperienceValue;
        CurrentExperienceMultiplier = Mathf.Clamp(tempExperienceMultiplier, 0.001f, 10000);

        var tempGoldGainMultiplier = BaseGoldGainMultiplier * (1 + GSC.UtilitySC.IncreaseGoldGainValue) * GSC.UtilitySC.MoreGoldGainValue * GSC.UtilitySC.LessGoldGainValue;
        CurrentGoldGainMultiplier = Mathf.Clamp(tempGoldGainMultiplier, 0.001f, 10000);

        var tempProjectileSpeed = (BaseProjectileSpeed + GSC.UtilitySC.FlatProjectileSpeedValue) * (1 + GSC.UtilitySC.IncreaseProjectileSpeedValue) * GSC.UtilitySC.MoreProjectileSpeedValue * GSC.UtilitySC.LessProjectileSpeedValue;
        CurrentProjectileSpeed = Mathf.Clamp(tempProjectileSpeed, 0.001f, 10000);


        OnStatsChange?.Invoke();
    }

    public void LevelUp()
    {
        CurrentLevel += 1;

        OnLevelUp?.Invoke();
    }

    public void GetGold(int amount)
    {
        CurrentGold += Mathf.RoundToInt(amount * CurrentGoldGainMultiplier);

        OnGoldChange?.Invoke(CurrentGold);
    }

    public bool SpentGold(int amount)
    {
        if (amount > CurrentGold) { return false; }

        CurrentGold -= amount;

        OnGoldChange?.Invoke(CurrentGold);

        return true;
    }

    public void SetAbilityToControll(bool isControllable)
    {
        IsControllable = isControllable;

        rb2D.velocity = Vector2.zero;

        if (isControllable == false)
        {
            if (navMeshAgent != null && navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.isStopped = true;
                OnControllLoss?.Invoke(IsControllable);
            }
        }
        else 
        {
            if (navMeshAgent != null && navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.isStopped = false;
                OnControllLoss?.Invoke(IsControllable);
            }
        }
    }

    public void SetAbilityToMove(bool canMove)
    {
        CanMove = canMove;

        rb2D.velocity = Vector2.zero;

        if (canMove == false && navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;            
        }

        if (canMove == true && navMeshAgent != null)
        {            
            navMeshAgent.isStopped = false;            
        }

        OnMovementLoss?.Invoke();
    }

    public void SetAbilityToShoot(bool canShoot)
    {
        CanShoot = canShoot;

        OnAbilityToShootLoss?.Invoke();
    }

    public void SetAbilityToCast(bool canCast)
    {
        CanCast = canCast;

        AbilitiesManager.StopAllCasting();

        OnAbilityToCastLoss?.Invoke();
    }

    public void SetImmunity(bool isImmune)
    {
        IsImmune = isImmune;
    }

    public void AmmoChange(int value)
    {
        CurrentAmmo += value;
    }

    public void ReplanishAmmo()
    {
        CurrentAmmo = CurrentAmmoCapacity;
    }

    public void HealthChange(float amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, -1, MaxHP);
    }

    public void ManaChange(float amount)
    {
        CurrentMana = Mathf.Clamp(CurrentMana + amount, 0, MaxMana);
    }

    public void StatsInitialization(StatsValues statsValues)
    {
        CharacterType = statsValues.CharacterType;

        //offense
        BaseMaxDamage = statsValues.BaseMaxDamage;
        BaseMinDamage = statsValues.BaseMinDamage;
        BaseAmmoCapacity = statsValues.BaseAmmoCapacity;
        BaseAttackSpeed = statsValues.BaseAttackSpeed;
        BaseCritChance = statsValues.BaseCritChance;
        BaseAttackCritMultiplier = statsValues.BaseCritMultiplier;
        BaseSpellCritMultiplier = statsValues.BaseSpellCritMultiplier;
        BaseAccuracy = statsValues.BaseAccuracy;
        BaseSpreadAngle = statsValues.BaseSpreadAngle;
        FlatAccuracyToPercent = statsValues.FlatAccuracyToPercent;

        BaseBuffPower = statsValues.BaseBuffPower;
        BaseEffectDuration = statsValues.BaseBuffDurationAmplifier;
        //defanse
        BaseArmor = statsValues.BaseArmor;
        FlatArmorToPercent = statsValues.FlatArmorToPercent;
        BaseHP = statsValues.BaseHP;
        MaxHP = BaseHP;
        CurrentHP = MaxHP;
        BaseHPRegeneration = statsValues.BaseHPRegeneration;
        BaseMana = statsValues.BaseMana;
        MaxMana = BaseMana;
        CurrentMana = MaxMana;
        BaseManaRegeneration = statsValues.BaseManaRegeneration;
        BaseMagicResist = statsValues.BaseMagicResist;
        FlatMResistToPercent = statsValues.FlatMResistToPercent;
        BaseHealingAmplifier = statsValues.BaseHealingAmplifier;
        //utility
        BaseCollectorRadius = statsValues.BaseCollectorRadius;
        BaseMovementSpeed = statsValues.BaseMovementSpeed;
        BaseReloadSpeed = statsValues.BaseReloadSpeed;
        BaseAttackRange = statsValues.BaseAttackRange;
        BaseProjectileSpeed = statsValues.BaseProjectileSpeed;
        BaseGlobalAOEMultiplier = statsValues.BaseGlobalAOEMultiplier;
        BaseExperienceMultiplier = statsValues.BaseExperienceMultiplier;
        BaseGoldGainMultiplier = statsValues.BaseGoldGainMultipler;
        

        BaseWeaponProjectileAmount = statsValues.ProjectileAmount;
        BaseWeaponPierceAmount = statsValues.PierceAmount;
        BaseWeaponChainsAmount = statsValues.ChainsAmount;

        GSC.MagicSC.AddFlatSpellProjectileAmount(statsValues.AddedSpellProjectileAmount);
    }

    public void ApplyStatsFromNewWeapon(Weapon weapon)
    {
        BaseMinDamage = weapon.MinDamage;
        BaseMaxDamage = weapon.MaxDamage;
        BaseAccuracy = weapon.Accuracy;
        BaseSpreadAngle = weapon.SpreadAngle;
        BaseAttackSpeed = weapon.AttackSpeed;
        BaseCritChance = weapon.CritChance;
        BaseAttackCritMultiplier = weapon.CritMultiplier;
        BaseReloadSpeed = weapon.ReloadSpeed;
        BaseAmmoCapacity = weapon.AmmoCapacity;
        BaseAttackRange = weapon.Range;
        BaseProjectileSpeed = weapon.ProjectileSpeed;
        CurrentAmmo = weapon.CurrentAmmo;
        BaseWeaponChainsAmount = weapon.ChainsAmount;
        BaseWeaponChainsAmount = Mathf.Clamp(BaseWeaponChainsAmount, 0, 1000);
        BaseWeaponPierceAmount = weapon.PierceAmount;
        BaseWeaponPierceAmount = Mathf.Clamp(BaseWeaponPierceAmount, 0, 1000);
        BaseWeaponProjectileAmount = weapon.ProjectileAmount;
        BaseWeaponProjectileAmount = Mathf.Clamp(BaseWeaponProjectileAmount, 0, 1000);
    }

    public void RemoveStatsFromOldWeapon(Weapon weapon)
    {
        BaseMinDamage = 0;
        BaseMaxDamage = 0;
        BaseAccuracy = 0;
        BaseSpreadAngle = 0;
        BaseAttackSpeed = 0;
        BaseCritChance = 0;
        BaseAttackCritMultiplier = 0;
        BaseReloadSpeed = 0;
        BaseAmmoCapacity = 0;
        BaseAttackRange = 0;
        BaseProjectileSpeed = 0;
        CurrentAmmo = 0;
        BaseWeaponChainsAmount = 0;
        BaseWeaponPierceAmount = 0;
        BaseWeaponProjectileAmount = 0;
    }

    public void ApplyStatsFromNewArmor(Armor armor)
    {
        BaseArmor += armor.LocalArmor;
        BaseMagicResist += armor.MagicResist;
        BaseHP += armor.HP;
    }

    public void RemoveStatsFromOldArmor(Armor armor)
    {
        BaseArmor -= armor.LocalArmor;
        BaseMagicResist -= armor.MagicResist;
        BaseHP -= armor.HP;
    }

    public void ChangeBaseDamage(float newMinDamage, float newMaxDamage)
    {
        BaseMinDamage = newMinDamage;
        BaseMaxDamage = newMaxDamage;

        EvaluateStats();
    }

    public void ChangeBaseHealth(float newHealth)
    {
        BaseHP = newHealth;

        EvaluateStats();
    }

    public void ChangeBaseArmorAndResist(float newArmor, float newResist)
    {
        BaseArmor = newArmor;
        BaseMagicResist = newResist;

        EvaluateStats();
    }

    public void ChangeBaseAttackRange(float newAttackRange)
    {
        BaseAttackRange = newAttackRange;

        EvaluateStats();
    }
}
