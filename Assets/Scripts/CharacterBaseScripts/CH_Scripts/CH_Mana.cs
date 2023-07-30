using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mana : MonoBehaviour
{
    [HideInInspector] public Action OnManaChange;

    private CH_Stats stats;

    private const float regenerationRate = 2f;

    private void Awake()
    {
        stats = GetComponent<CH_Stats>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Regeneration), 0, regenerationRate);
    }

    public void SpendMana(float amount)
    {
        stats.ManaChange(-amount);

        OnManaChange?.Invoke();
    }

    public void ReplanishMana(float amount)
    {
        stats.ManaChange(amount);

        OnManaChange?.Invoke();
    }

    private void Regeneration()
    {
        ReplanishMana(stats.CurrentManaRegeneration * regenerationRate);
    }
}
