using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;


public class CH_OnHit : MonoBehaviour
{
    /// <summary>
    /// методы запускаются на владельце скрипта при получении удара от врага
    /// </summary>
    public List<OnHitEffect> Behavior_OnOwner_GettingHit { get; private set; } = new();
    
    /// <summary>
    /// методы запускаются на владельце скрипта при нанесении им удара
    /// </summary>
    public List<OnHitEffect> Behavior_OnOwner_GivingHit { get; private set; } = new();

    /// <summary>
    /// методы запускаются на владельце скрипта при нанесении им удара заклинанием
    /// </summary>
    public List<OnHitEffect> Behavior_OnOwner_GivingMagicHit { get; private set; } = new();

    /// <summary>
    /// методы запускаются на враге при нанесении ему удара
    /// </summary>
    public List<OnHitEffect> Behavior_OnEnemy_GivingHit { get; private set; } = new();

    /// <summary>
    /// методы запускаются на враге при нанесении ему удара заклинанием
    /// </summary>
    public List<OnHitEffect> Behavior_OnEnemy_GivingMagicHit { get; private set; } = new();


    public List<int> HitByProjIndex = new(); //BaseProjectile просматривает список и если обьект уже ударил прож с тем же индексом, то он игнорируется

    //Callbacks
    [HideInInspector] public event Action<DamageArgs> OnGivivngPhisicalHitCallback;
    [HideInInspector] public event Action<DamageArgs> OnGivingMagicHitCallback;
    [HideInInspector] public event Action<DamageArgs> OnGettingPhisicalHitCallback;
    [HideInInspector] public event Action<DamageArgs> OnGettingMagicHitCallback;

    public CH_Stats OwnerStats { get; private set; }
    public CH_Health OwnerHealth { get; private set; }
    public CH_BuffManager OwnerBuffManager { get; private set; }

    private void Awake()
    {
        OwnerStats = GetComponent<CH_Stats>();
        OwnerHealth = GetComponent<CH_Health>();
        OwnerBuffManager = GetComponent<CH_BuffManager>();
    }

    public void OnGivingAttackHit(DamageArgs damageArgs)
    {
        TriggerEveryOnHitEffectOnYouWhenEnemyHit(damageArgs);

        OnGivivngPhisicalHitCallback?.Invoke(damageArgs);
    }

    public void OnGivingSpellHit(DamageArgs damageArgs)
    {
        TriggerEveryMagicOnHitEffectOnYouWhenEnemyHit(damageArgs);

        OnGivingMagicHitCallback?.Invoke(damageArgs);
    }


    public void OnGettingAttackHit(DamageArgs damageArgs)
    {
        TriggerYourOwnOnHitEffectsWhenGetHit(damageArgs);

        TriggerEveryOnHitEffect(damageArgs);

        OnGettingPhisicalHitCallback?.Invoke(damageArgs);
    }


    public void OnGettingSpellHit(DamageArgs damageArgs)
    {        
        TriggerYourOwnOnHitEffectsWhenGetHit(damageArgs);

        TriggerEveryMagicOnHitEffect(damageArgs);

        OnGettingMagicHitCallback?.Invoke(damageArgs);
    }  



    public void AddOnHit_GettingHit(OnHitEffect onHitEffect)
    {
        var onHit = onHitEffect.CopyV2();
        onHit.OnHitOnEnemy = onHitEffect.OnHitOnEnemy;
        onHit.OnHitOnOwner = onHitEffect.OnHitOnOwner;

        //проверка на стакоболость:)
        foreach (OnHitEffect oh in Behavior_OnOwner_GettingHit)
        {
            if (oh.Name == onHit.Name && oh.IsStackable == false)
            {
                return;
            }
        }

        Behavior_OnOwner_GettingHit.Add(onHit);
    }

    public void RemoveOnHit_GettingHit(string generatedID)
    {
        if (Behavior_OnOwner_GettingHit.Count > 0)
        {
            foreach (OnHitEffect onHit in Behavior_OnOwner_GettingHit)
            {
                if (onHit.GeneratedID == generatedID)
                {
                    Behavior_OnOwner_GettingHit.Remove(onHit);
                    break;
                }
            }
        }
    }

    public void AddOnHit_GivingHit(OnHitEffect onHitEffect)
    {
        var onHit = onHitEffect.CopyV2();
        onHit.OnHitOnEnemy = onHitEffect.OnHitOnEnemy;
        onHit.OnHitOnOwner = onHitEffect.OnHitOnOwner;

        if (onHit.OnHitOnEnemy is not null)
        {
            bool shouldAddEffect = true;
            //проверка на стакоболость
            foreach (OnHitEffect oh in Behavior_OnEnemy_GivingHit)
            {
                if (oh.Name == onHit.Name && oh.IsStackable == false)
                {
                    shouldAddEffect = false;
                    break;
                }                
            }

            if (shouldAddEffect)
            {
                Behavior_OnEnemy_GivingHit.Add(onHit);                
            }
        }

        if (onHit.OnHitOnOwner is not null)
        {
            bool shouldAddEffect = true;
            //проверка на стакоболость
            foreach (OnHitEffect oh in Behavior_OnOwner_GivingHit)
            {
                if (oh.Name == onHit.Name && oh.IsStackable == false)
                {
                    shouldAddEffect = false;
                    break;
                }
            }

            if (shouldAddEffect)
            {
                Behavior_OnOwner_GivingHit.Add(onHit);
            }
        }
    }

    public void AddOnHit_GivingMagicHit(OnHitEffect onHitEffect)
    {
        var onHit = onHitEffect.CopyV2();
        onHit.OnHitOnEnemy = onHitEffect.OnHitOnEnemy;
        onHit.OnHitOnOwner = onHitEffect.OnHitOnOwner;

        if (onHit.OnHitOnEnemy is not null)
        {
            bool shouldAddEffect = true;
            //проверка на стакоболость
            foreach (OnHitEffect oh in Behavior_OnEnemy_GivingMagicHit)
            {
                if (oh.Name == onHit.Name && oh.IsStackable == false)
                {
                    shouldAddEffect = false;
                    break;
                }
            }

            if (shouldAddEffect)
            {
                Behavior_OnEnemy_GivingMagicHit.Add(onHit);
            }
        }

        if (onHit.OnHitOnOwner is not null)
        {
            bool shouldAddEffect = true;
            foreach (OnHitEffect oh in Behavior_OnOwner_GivingMagicHit)
            {
                if (oh.Name == onHit.Name && oh.IsStackable == false)
                {
                    shouldAddEffect = false;
                    break;
                }                
            }

            if (shouldAddEffect)
            {
                Behavior_OnOwner_GivingMagicHit.Add(onHit);
            }
        }
    }

    public void RemoveOnHit_GivingHit(string generatedID)
    {
        if (Behavior_OnEnemy_GivingHit.Count > 0)
        {
            foreach (OnHitEffect onHit in Behavior_OnEnemy_GivingHit)
            {
                if (onHit.GeneratedID == generatedID)
                {
                    Behavior_OnEnemy_GivingHit.Remove(onHit);
                    break;
                }
            }
        }

        if (Behavior_OnOwner_GivingHit.Count > 0)
        {
            foreach (OnHitEffect onHit in Behavior_OnOwner_GivingHit)
            {
                if (onHit.GeneratedID == generatedID)
                {
                    Behavior_OnOwner_GivingHit.Remove(onHit);
                    break;
                }
            }
        }
    }

    public void RemoveOnHit_GivingMagicHit(string generatedID)
    {
        if (Behavior_OnOwner_GivingMagicHit.Count > 0)
        {
            foreach (OnHitEffect onHit in Behavior_OnOwner_GivingMagicHit)
            {
                if (onHit.GeneratedID == generatedID)
                {
                    Behavior_OnOwner_GivingMagicHit.Remove(onHit);
                    break;
                }
            }
        }

        if (Behavior_OnEnemy_GivingMagicHit.Count > 0)
        {
            foreach (OnHitEffect onHit in Behavior_OnEnemy_GivingMagicHit)
            {
                if (onHit.GeneratedID == generatedID)
                {
                    Behavior_OnEnemy_GivingMagicHit.Remove(onHit);
                    break;
                }
            }
        }
    }

    //------UTILITY------------
    private void TriggerEveryOnHitEffectOnYouWhenEnemyHit(DamageArgs damageArgs)
    {
        if (Behavior_OnOwner_GivingHit.Count > 0)
        {
            foreach (OnHitEffect hitEffect in Behavior_OnOwner_GivingHit)
            {
                hitEffect.OnHitOnOwner?.Invoke(damageArgs);
            }
        }
    }
    
    private void TriggerEveryMagicOnHitEffectOnYouWhenEnemyHit(DamageArgs damageArgs)
    {
        if (Behavior_OnOwner_GivingMagicHit.Count > 0)
        {
            foreach (OnHitEffect hitEffect in Behavior_OnOwner_GivingMagicHit)
            {
                hitEffect.OnHitOnOwner?.Invoke(damageArgs);
            }
        }
    }

    private void TriggerYourOwnOnHitEffectsWhenGetHit(DamageArgs damageArgs)
    {
        if (Behavior_OnOwner_GettingHit.Count > 0)
        {
            foreach (OnHitEffect hitEffect in Behavior_OnOwner_GettingHit)
            {
                hitEffect.OnHitOnOwner?.Invoke(damageArgs);
            }
        }
    }

    private void TriggerEveryOnHitEffect(DamageArgs damageArgs)
    {
        if (damageArgs.ShooterStats.OnHit.Behavior_OnEnemy_GivingHit.Count > 0)
        {
            foreach (OnHitEffect hitEffect in damageArgs.ShooterStats.OnHit.Behavior_OnEnemy_GivingHit)
            {
                hitEffect.OnHitOnEnemy?.Invoke(damageArgs);
            }
        }
    }
    
    private void TriggerEveryMagicOnHitEffect(DamageArgs damageArgs)
    {
        if (damageArgs.ShooterStats.OnHit.Behavior_OnEnemy_GivingMagicHit.Count > 0)
        {
            foreach (OnHitEffect hitEffect in damageArgs.ShooterStats.OnHit.Behavior_OnEnemy_GivingMagicHit)
            {
                hitEffect.OnHitOnEnemy?.Invoke(damageArgs);
            }
        }
    }
}
