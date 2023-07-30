using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System.Reflection;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "ChallengeConditionsDatabase", menuName = "ScriptableObjects/Challenges/ChallengeConditionsDatabase")]
public class ChallengeConditionsDatabase : ScriptableObject
{
    [SerializeField] private List<ChallengeConditionsSO> conditionsSO;
    public Dictionary<string, ChallengeCondition> ChallengeConditions { get; private set; } = new(); //награды в словаре
    public List<ChallengeCondition> ChallengeConditionsList { get; private set; } = new(); //те же нагарды в листе

    public void Initialize()
    {
        ChallengeConditions.Clear();
        ChallengeConditionsList.Clear();

        foreach (var chal in conditionsSO)
        {
            var list = chal.GetConditions();

            foreach (var item in list)
            {
                ChallengeConditions.Add(item.Name, item);
            }

            list = list.Where(x => x.Name != "Empty").ToList();

            ChallengeConditionsList.AddRange(list);
        }
    }
}
