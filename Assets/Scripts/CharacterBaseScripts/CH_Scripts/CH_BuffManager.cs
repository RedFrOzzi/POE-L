using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System;

public class CH_BuffManager : MonoBehaviour
{    
    private readonly float currentTickDelay = 0.5f;
    private float nextTick = 0f;

    public List<Buff> Buffs { get; private set; } = new();

    public event Action OnBuffsChange;

    private CH_Stats stats;

    private void Awake()
    {
        stats = GetComponent<CH_Stats>();
    }

    private void Update()
    {
        if (GameFlowManager.Instance.IsGamePaused) { return; }
        
        TickBuff(currentTickDelay);
    }

    
    public void ApplyBuff(Buff buff, CH_Stats sourseStats)
    {
        if (buff is BuffTriggerOnFullStacks)
        {
            HandleTriggerBuff(buff, sourseStats);
        }
        else if (buff is BuffStackable)
        {
            HandleStackableBuff(buff, sourseStats);
        }
        else
        {
            HandleRegularBuff(buff, sourseStats);
        }
    }


    private void TickBuff(float tickDelay)
    {
        if (Time.time < nextTick) { return; }
        
        nextTick = Time.time + tickDelay;
        
                
        for (int i = Buffs.Count - 1; i >= 0; i--)
        {            
            Buffs[i].OnBuffTick(tickDelay);

            if(Buffs[i].ExpireTime <= Time.time)
            {
                Buffs[i].OnBuffExpire();

                Buffs.Remove(Buffs[i]);

                OnBuffsChange?.Invoke();
            }
        }
    }

    public void RemoveBuff(string generatedID)
    {
        foreach (Buff buff in Buffs)
        {
            if (buff.GeneratedID == generatedID)
            {
                buff.OnBuffExpire();
                Buffs.Remove(buff);

                OnBuffsChange?.Invoke();
                break;
            }
        }
    }

    public bool IsBuffActive(string name)
    {
        bool isActive = false;

        if (Buffs.Count > 0)
        {            
            foreach (Buff b in Buffs)
            {
                if (b.Name == name)
                {
                    isActive = true;
                    break;
                }
            }
        }
        else
            isActive = false;

        return isActive;
    }

    private void HandleRegularBuff(Buff newBuff, CH_Stats sourseStats)
    {
        bool notFound = true;

        foreach (Buff iteratedBuff in Buffs)
        {
            if (iteratedBuff.Name == newBuff.Name)
            {
                notFound = false;

                iteratedBuff.SetExpireTime(Time.time + (iteratedBuff.Duration * stats.CurrentEffectDuration));
            }
        }

        if (notFound)
        {
            var newB = NewBuff(newBuff, sourseStats);

            newB.OnBuffApplication();

            Buffs.Add(newB);

            OnBuffsChange?.Invoke();
        }
    }

    private void HandleTriggerBuff(Buff newBuff, CH_Stats sourseStats)
    {
        bool notFound = true;

        foreach (Buff iteratedBuff in Buffs)
        {
            if (iteratedBuff is BuffTriggerOnFullStacks triggerBuff)
            {
                if (triggerBuff.Name == newBuff.Name)
                {
                    notFound = false;

                    triggerBuff.StacksAmount++;

                    triggerBuff.SetExpireTime(Time.time + (triggerBuff.Duration * stats.CurrentEffectDuration));

                    if (triggerBuff.StacksAmount >= triggerBuff.StacksToTrigger)
                    {
                        triggerBuff.OnFullStacks();
                        triggerBuff.OnBuffExpire();
                        Buffs.Remove(triggerBuff);

                        OnBuffsChange?.Invoke();
                        break;
                    }
                }
            }
        }

        if (notFound)
        {
            if (newBuff is BuffTriggerOnFullStacks triggerBuff)
            {
                var newB = NewBuff(triggerBuff, sourseStats) as BuffTriggerOnFullStacks;
                newB.OnBuffApplication();
                newB.StacksAmount++;
                Buffs.Add(newB);

                OnBuffsChange?.Invoke();
            }            
        }
    }

    private void HandleStackableBuff(Buff newBuff, CH_Stats sourseStats)
    {
        bool notFound = true;

        foreach (Buff iteratedBuff in Buffs)
        {
            if (iteratedBuff is BuffStackable stackable)
            {
                if (stackable.Name == newBuff.Name)
                {
                    notFound = false;

                    if (stackable.StacksAmount < stackable.MaxStacks)
                    {
                        stackable.OnBuffApplication();
                        stackable.StacksAmount++;
                        stackable.SetExpireTime(Time.time + (stackable.Duration * stats.CurrentEffectDuration));
                        break;
                    }
                    else
                    {
                        stackable.SetExpireTime(Time.time + (stackable.Duration * stats.CurrentEffectDuration));
                        break;
                    }
                }
            }
        }

        if (notFound)
        {
            if (newBuff is BuffStackable stackable)
            {
                var newB = NewBuff(stackable, sourseStats) as BuffStackable;

                newB.OnBuffApplication();
                newB.StacksAmount++;
                Buffs.Add(newB);

                OnBuffsChange?.Invoke();
            }            
        }
    }

    private Buff NewBuff(Buff buff, CH_Stats sourseStats)
    {
        var newBuff = buff.CopyV2();

        newBuff.SetOwnerStats(stats);

        newBuff.SetSourseStats(sourseStats);

        newBuff.SetExpireTime(Time.time + (newBuff.Duration * stats.CurrentEffectDuration));

        return newBuff;
    }
}
