using UnityEngine;
using Database;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ChallengeConditions", menuName = "ScriptableObjects/Challenges/ChallengeConditions")]
public class ChallengeConditionsSO : ScriptableObject
{
	[SerializeField] private List<ChallengeCondition> currentConditions = new();

	public List<ChallengeCondition> GetConditions()
    {
        currentConditions.Clear();

        ChallengeCondition cc = new();

        //---TIME-------------
        cc.Name = "TimeCondition";
        cc.TimeValues = new float[] { 120, 100, 90, 60 };
        cc.ModTags[0] = ModTag.Time;
        cc.Description = (c) => $"Survive for {c.TimeValues[c.Tier]} seconds";
        cc.SetCondition = (m, c) => m.SetConditionTime(c.TimeValues[c.Tier]);
        currentConditions.Add(cc);
        //--------------------

        //---KILLS------------
        cc = new();
        cc.Name = "KillsCondition";
        cc.KillsValues = new int[] { 300, 200, 100, 50 };
        cc.ModTags[0] = ModTag.Kill;
        cc.Description = (c) => $"Kill {c.KillsValues[c.Tier]} enemies";
        cc.SetCondition = (m, c) => m.SetConditionKills(c.KillsValues[c.Tier]);
        currentConditions.Add(cc);
        //--------------------

        //---TIME-AND-KILLS---
        cc = new();
        cc.Name = "TimeAndKillsCondition";
        cc.TimeValues = new float[] { 100, 80, 60, 45 };
        cc.KillsValues = new int[] { 200, 100, 60, 30 };
        cc.ModTags[0] = ModTag.Kill;
        cc.ModTags[1] = ModTag.Time;
        cc.Description = (c) => $"Kill {c.KillsValues[c.Tier]} enemies and survive for {c.TimeValues[c.Tier]} seconds";
        cc.SetCondition = (m, c) =>
            {
                m.SetConditionKills(c.KillsValues[c.Tier]);
                m.SetConditionTime(c.TimeValues[c.Tier]);
            };
        currentConditions.Add(cc);
        //--------------------

        return currentConditions;
    }
}
