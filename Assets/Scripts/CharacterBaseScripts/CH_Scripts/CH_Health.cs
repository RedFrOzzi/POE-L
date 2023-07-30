using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;

public class CH_Health : MonoBehaviour
{
    private const float flashOnHitTime = 0.1f;
    private WaitForSeconds waitTime;
    private Material defaultMaterial;

    public CH_Stats Stats { get; private set; }
    private SpriteRenderer spriteRenderer;

    private bool alreadyDied = false;
    private GameStatistics gameStatistics;
    private ItemSpawner itemSpawner;

    private readonly List<OnDeathEffect> onDeathEffects = new();

    private const float initialRegenerationRate = 0.5f;
    private float regenerationRate = 0.5f;
    private float nextReg;

    private const float minimumRegistratedDamage = 0.001f;

    public event Action<Damage> OnDamageTake;
    public event Action<float> OnHealthChange;
    public event Action OnDeath;

    private void Awake()
    {
        Stats = GetComponent<CH_Stats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        regenerationRate = initialRegenerationRate;

        waitTime = new(flashOnHitTime);
    }

    private void Start()
    {
        gameStatistics = GameStatistics.instance;
        itemSpawner = ItemSpawner.Instance;
    }

    private void Update()
    {
        if (Time.time < nextReg) { return; }

        nextReg = Time.time + regenerationRate;

        Regeneration();
    }

    private void OnDisable()
    {
        spriteRenderer.material = defaultMaterial;
    }


    public void TakeDamage(Damage damage, bool isCritical)
    {
        if (Stats.IsImmune) { return; }

        Damage postMitigationDamage = new(damage.Physical * (1 - Stats.PercentPhisicalResistance / 100),
            damage.Magic * (1 - Stats.PercentMagicResistance / 100),
            damage.True,
            Stats.CurrentHP * damage.PercentPhysical / 100 * (1 - Stats.PercentPhisicalResistance / 100),
            Stats.CurrentHP * damage.PercentMagic / 100 * (1 - Stats.PercentMagicResistance / 100),
            Stats.CurrentHP * damage.PercentTrue / 100);

        float pmDamageCombined = postMitigationDamage.CombinedDamage();

        if (pmDamageCombined < minimumRegistratedDamage) { return; } //не получает урона ниже минимального уровня для предотвращения бесконечного цикла

        StartCoroutine(HitFlashCoroutine()); //Моргает белым цветом

        Stats.HealthChange(-pmDamageCombined);

        DamagePopupManager.Instance.ShowDamage(transform.position, pmDamageCombined, isCritical);

        OnHealthChange?.Invoke(-pmDamageCombined);
        OnDamageTake?.Invoke(postMitigationDamage * -1);


        gameStatistics.DamageStatistics(gameObject.tag, postMitigationDamage);

        CheckForLethal();
    }

    public void TakeHeal(float hp)
    {
        if (alreadyDied) { return; }

        if (Mathf.Approximately(hp, 0f)) { return; }

        Stats.HealthChange(hp);

        OnHealthChange?.Invoke(hp);

        CheckForLethal();
    }

    public void ChangeRegenRate(float newRate)
    {
        regenerationRate = newRate;
    }

    private void Regeneration()
    {
        TakeHeal(Stats.CurrentHPRegeneration * Stats.CurrentHealingAmplifier * regenerationRate);
    }

    private void CheckForLethal()
    {
        if (Stats.CurrentHP > 0) { return; }

        if (alreadyDied) { return; }

        alreadyDied = true;

        gameStatistics.OnDeathStatistic(gameObject.tag);

        if (Stats.CharacterType != CharacterType.Player)
        {
            GlobalEnemyModifiers.Instance.RemmoveAliveEnemy(Stats);

            EnemySpawner.Instance.OnEnemyDeath();

            itemSpawner.OnEnemyDeath(Stats.CharacterType, transform.position);
        }
        else if (Stats.CharacterType == CharacterType.Player)
        {
            TriggerOnDeathEffects();

            OnDeath?.Invoke();

            Stats.SetImmunity(true);

            Stats.SetAbilityToControll(false);

            Stats.CH_Animation.PlayEmpty();

            return;
        }
        
        TriggerOnDeathEffects();

        OnDeath?.Invoke();

        Destroy(gameObject);
    }


    private void TriggerOnDeathEffects()
    {

        foreach (OnDeathEffect effect in onDeathEffects)
        {
            effect.Effect?.Invoke(Stats);                       
        }
    }


    public void AddOnDeathEffect(OnDeathEffect onDeathEffect, CH_Stats applicantStats)
    {
        bool shouldAddEffect = true;

        foreach (OnDeathEffect deathEffect in onDeathEffects)
        {            
            if (deathEffect.GeneratedID == onDeathEffect.GeneratedID && deathEffect.IsStuckable == false)
            {
                shouldAddEffect = false;
                break;
            }           
        }

        if (shouldAddEffect)
        {
            var effect = onDeathEffect.CopyV2();
            effect.Effect = effect.Eff;
            effect.SourceStats = applicantStats;

            onDeathEffects.Add(effect);
        }        
    }

    public void RemoveOnDeathEffect(string generatedID)
    {
        if (onDeathEffects.Count <= 0) { return; }

        foreach (OnDeathEffect deathEffect in onDeathEffects)
        {
            if (deathEffect.GeneratedID == generatedID)
            {
                onDeathEffects.Remove(deathEffect);
                break;
            }
        }
    }

    private IEnumerator HitFlashCoroutine()
    {
        spriteRenderer.material = WeaponBehaviour.Materials["WhiteMaterial"];

        yield return waitTime;

        spriteRenderer.material = defaultMaterial;
    }

    
}
