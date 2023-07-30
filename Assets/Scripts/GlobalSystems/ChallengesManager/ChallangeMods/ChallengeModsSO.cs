using System.Collections.Generic;
using UnityEngine;
using Database;

[CreateAssetMenu(fileName = "_ChallengeMods", menuName = "ScriptableObjects/Challenges/ChallengeModsType")]
public class ChallengeModsSO : ScriptableObject
{
    [SerializeField] private List<ChallengeMod> currentMods = new();

	public List<ChallengeMod> GetMods()
    {
        currentMods.Clear();

        //---EMPTY---
        ChallengeMod mod = new();
        mod.Name = "Empty";
        currentMods.Add(mod);
        //-----------

        //---ATTACK---
        mod = new();
        mod.Name = "EnemyAttackSpeed";
        mod.TierValues = new float[] { 40 ,35, 30, 25, 20, 15, 10, 5 };
        mod.ModTags[0] = ModTag.Speed;
        mod.ModTags[1] = ModTag.Attack;
        mod.Description = (mod) => $"Increase enemies attack speed by {mod.TierValues[mod.Tier]}%";
        mod.ApplyMod = (pStats, enemyModifiers) => enemyModifiers.ESC.AttackSC.IncreaseAttackSpeed(mod.TierValues[mod.Tier]);
        mod.RemoveMod = (pStats, enemyModifiers) => enemyModifiers.ESC.AttackSC.IncreaseAttackSpeed(-mod.TierValues[mod.Tier]);
        mod.GetChallangeValue = (t) => 5 * (mod.TierValues.Length - t);
        currentMods.Add(mod);

        mod = new();
        mod.Name = "EnemyDamage";
        mod.TierValues = new float[] { 50, 40, 30, 25, 20, 15, 10, 5 };
        mod.ModTags[0] = ModTag.Damage;
        mod.Description = (mod) => $"Increase enemies damage by {mod.TierValues[mod.Tier]}%";
        mod.ApplyMod = (pStats, enemyModifiers) => enemyModifiers.ESC.GlobalSC.IncreaseDamage(mod.TierValues[mod.Tier]);
        mod.RemoveMod = (pStats, enemyModifiers) => enemyModifiers.ESC.GlobalSC.IncreaseDamage(-mod.TierValues[mod.Tier]);
        mod.GetChallangeValue = (t) => 5 * (mod.TierValues.Length - t);
        currentMods.Add(mod);
        //------------

        //---DEFANCE---
        mod = new();
        mod.Name = "EnemyIncreaseHealth";
        mod.TierValues = new float[] { 50, 40, 30, 25, 20, 15, 10, 5 };
        mod.ModTags[0] = ModTag.Health;
        mod.Description = (mod) => $"Increase enemies health by {mod.TierValues[mod.Tier]}%";
        mod.ApplyMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.IncreaseHP(mod.TierValues[mod.Tier]);
        mod.RemoveMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.IncreaseHP(-mod.TierValues[mod.Tier]);
        mod.GetChallangeValue = (t) => 5 * (mod.TierValues.Length - t);
        currentMods.Add(mod);

        mod = new();
        mod.Name = "EnemyMoreHealth";
        mod.TierValues = new float[] { 20, 17, 15, 11, 9, 7, 5, 2 };
        mod.ModTags[0] = ModTag.Health;
        mod.Description = (mod) => $"Enemies get +{mod.TierValues[mod.Tier]}% MORE health";
        mod.ApplyMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.MoreHP(mod.TierValues[mod.Tier]);
        mod.RemoveMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.MoreHP(-mod.TierValues[mod.Tier]);
        mod.GetChallangeValue = (t) => 10 * (mod.TierValues.Length - t);
        currentMods.Add(mod);

        mod = new();
        mod.Name = "EnemyIncreaseArmor";
        mod.TierValues = new float[] { 50, 40, 30, 25, 20, 15, 10, 5 };
        mod.ModTags[0] = ModTag.Armor;
        mod.Description = (mod) => $"Increase enemies armor by {mod.TierValues[mod.Tier]}%";
        mod.ApplyMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.IncreaseArmor(mod.TierValues[mod.Tier]);
        mod.RemoveMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.IncreaseArmor(-mod.TierValues[mod.Tier]);
        mod.GetChallangeValue = (t) => 5 * (mod.TierValues.Length - t);
        currentMods.Add(mod);

        mod = new();
        mod.Name = "EnemyMoreArmor";
        mod.TierValues = new float[] { 20, 17, 15, 11, 9, 7, 5, 2 };
        mod.ModTags[0] = ModTag.Armor;
        mod.Description = (mod) => $"Enemies get +{mod.TierValues[mod.Tier]}% MORE armor";
        mod.ApplyMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.MoreArmor(mod.TierValues[mod.Tier]);
        mod.RemoveMod = (pStats, enemyModifiers) => enemyModifiers.ESC.DefanceSC.MoreArmor(-mod.TierValues[mod.Tier]);
        mod.GetChallangeValue = (t) => 10 * (mod.TierValues.Length - t);
        currentMods.Add(mod);
        //---------------------

        //---UTILITY---
        mod = new();
        mod.Name = "EnemyMovementSpeed";
        mod.TierValues = new float[] { 30, 27, 25, 20, 15, 10, 7, 5 };
        mod.ModTags[0] = ModTag.Speed;
        mod.ModTags[1] = ModTag.Movement;
        mod.Description = (mod) => $"Increase enemies movement speed by {mod.TierValues[mod.Tier]}%";
        mod.ApplyMod = (pStats, enemyModifiers) => enemyModifiers.ESC.UtilitySC.IncreaseMovementSpeed(mod.TierValues[mod.Tier]);
        mod.RemoveMod = (pStats, enemyModifiers) => enemyModifiers.ESC.UtilitySC.IncreaseMovementSpeed(-mod.TierValues[mod.Tier]);
        mod.GetChallangeValue = (t) => 5 * (mod.TierValues.Length - t);
        currentMods.Add(mod);
        //------------------------


        return currentMods;
    }
}
