using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System.Reflection;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "ChallengeRewardsDatabase", menuName = "ScriptableObjects/Challenges/ChallengeRewardsDatabase")]
public class ChallengeRewardsDatabase : ScriptableObject
{
    [SerializeField] private List<ChallengeRewardsSO> rewardsSO;

    public Dictionary<string, ChallengeReward> ChallengeRewards { get; private set; } = new(); //награды в словаре
    public List<ChallengeReward> ChallengeRewardsList { get; private set; } = new(); //те же нагарды в листе

    public void Initialize()
    {
        ChallengeRewards.Clear();
        ChallengeRewardsList.Clear();

        foreach (var reward in rewardsSO)
        {
            var list = reward.GetRewards();

            foreach (var item in list)
            {
                ChallengeRewards.Add(item.Name, item);
            }

            list = list.Where(x => x.Name != "Empty").ToList();

            ChallengeRewardsList.AddRange(list);
        }
    }
}
