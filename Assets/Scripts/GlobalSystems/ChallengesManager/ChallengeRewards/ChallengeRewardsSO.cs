using UnityEngine;
using Database;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "ChallengeRewards", menuName = "ScriptableObjects/Challenges/ChallengeRewards")]
public class ChallengeRewardsSO : ScriptableObject
{
	[SerializeField] private List<ChallengeReward> currentConditions = new();

	public List<ChallengeReward> GetRewards()
    {
		currentConditions.Clear();

		ChallengeReward cr = new();

		//---EMPTY-----------------
		cr.Name = "Empty";
		currentConditions.Add(cr);
		//-------------------------

		//---ATTACK----------------
		cr = new();
		cr.Name = "IncreaseAttackDamage";
		cr.TierValues = new float[] { 11, 10, 9, 7, 5, 3, 2, 1 };
		cr.ModTags[0] = ModTag.Attack;
		cr.ModTags[1] = ModTag.Damage;
		cr.Description = (r) => $"Permanently increase attack damage by {r.TierValues[r.Tier]}%";
		cr.GiveReward = (s, r) => s.GSC.AttackSC.IncreaseAttackDamage(r.TierValues[r.Tier]);
		currentConditions.Add(cr);

		cr = new();
		cr.Name = "IncreaseAttackCritChance";
		cr.TierValues = new float[] { 11, 10, 9, 7, 5, 3, 2, 1 };
		cr.ModTags[0] = ModTag.Attack;
		cr.ModTags[1] = ModTag.Critical;
		cr.Description = (r) => $"Permanently increase crit chance for attacks by {r.TierValues[r.Tier]}%";
		cr.GiveReward = (s, r) => s.GSC.AttackSC.IncreaseAttackCritChance(r.TierValues[r.Tier]);
		currentConditions.Add(cr);

		cr = new();
		cr.Name = "IncreaseAttackCritMulti";
		cr.TierValues = new float[] { 11, 10, 9, 7, 5, 3, 2, 1 };
		cr.ModTags[0] = ModTag.Attack;
		cr.ModTags[1] = ModTag.Critical;
		cr.Description = (r) => $"Permanently adds +{r.TierValues[r.Tier]}% crit multiplier for attacks";
		cr.GiveReward = (s, r) => s.GSC.AttackSC.AddFlatAttackCritMultiplier((int)r.TierValues[r.Tier]);
		currentConditions.Add(cr);
		//-------------------------

		//---DEFANCE---------------
		cr = new();
		cr.Name = "IncreaseArmor";
		cr.TierValues = new float[] { 11, 10, 9, 7, 5, 3, 2, 1 };
		cr.ModTags[0] = ModTag.Armor;
		cr.ModTags[1] = ModTag.Defance;
		cr.Description = (r) => $"Permanently increase armor by {r.TierValues[r.Tier]}%";
		cr.GiveReward = (s, r) => s.GSC.DefanceSC.IncreaseArmor(r.TierValues[r.Tier]);
		currentConditions.Add(cr);

		cr = new();
		cr.Name = "IncreaseHealth";
		cr.TierValues = new float[] { 11, 10, 9, 7, 5, 3, 2, 1 };
		cr.ModTags[0] = ModTag.Health;
		cr.ModTags[1] = ModTag.Defance;
		cr.Description = (r) => $"Permanently increase armor by {r.TierValues[r.Tier]}%";
		cr.GiveReward = (s, r) => s.GSC.DefanceSC.IncreaseArmor(r.TierValues[r.Tier]);
		currentConditions.Add(cr);
		//-------------------------

		//--UTILITY----------------
		cr = new();
		cr.Name = "IncreaseSpeed";
		cr.TierValues = new float[] { 11, 10, 9, 7, 5, 3, 2, 1 };
		cr.ModTags[0] = ModTag.Speed;
		cr.Description = (r) => $"Permanently increase movement speed by {r.TierValues[r.Tier]}%";
		cr.GiveReward = (s, r) => s.GSC.UtilitySC.IncreaseMovementSpeed(r.TierValues[r.Tier]);
		currentConditions.Add(cr);
		//-------------------------

		//---OTHER-----------------
		cr = new();
		cr.Name = "SpawnItem";
		cr.TierValues = Array.Empty<float>();
		cr.ModTags[0] = ModTag.Item;
		cr.Description = (r) => $"Gives random item";
		cr.GiveReward = (s, r) => ItemSpawner.Instance.GiveRandomItem(r.Tier);
		currentConditions.Add(cr);

		cr = new();
		cr.Name = "RandomAbility";
		cr.TierValues = Array.Empty<float>();
		cr.ModTags[0] = ModTag.Abillity;
		cr.Description = (r) => $"Give random ability";
		cr.GiveReward = (s, r) => s.AbilitiesSwaper.SetUpAbility();
		currentConditions.Add(cr);
		//-------------------------

		return currentConditions;
	}
}
