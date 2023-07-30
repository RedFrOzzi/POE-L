using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SignatureModsSpawnWeights", menuName = "ScriptableObjects/Weights/SignatureModsSpawnWeights")]
public class SignatureModsSpawnWeights : ScriptableObject
{
    [field: SerializeField, Space(10)] public int T0SignatureModDropWeight { get; private set; } = 0;
    [field: SerializeField] public int T1SignatureModDropWeight { get; private set; } = 0;
    [field: SerializeField] public int T2SignatureModDropWeight { get; private set; } = 0;
    [field: SerializeField] public int T3SignatureModDropWeight { get; private set; } = 0;
    [field: SerializeField] public int T4SignatureModDropWeight { get; private set; } = 0;

    private float SMOverallWeight;

    private float[] SMWeights;
    private float[] SMChances;

    public void SetUpChances()
    {
        SMWeights = new float[] { T0SignatureModDropWeight, T1SignatureModDropWeight, T2SignatureModDropWeight, T3SignatureModDropWeight,
        T4SignatureModDropWeight };

        Array.ForEach(SMWeights, x => SMOverallWeight += x);

        SMChances = new float[SMWeights.Length];

        for (int i = 0; i < SMWeights.Length; i++)
        {
            SMChances[i] = SMWeights[i] / SMOverallWeight;
        }
    }

    public int GetWeightedSignatureModTier()
    {
        float chance = UnityEngine.Random.value;

        float accum = 0f;
        for (int i = 0; i < SMChances.Length; i++)
        {
            accum += SMChances[i];
            if (chance < accum)
            {
                return i;
            }
        }

        return 4;
    }
}
