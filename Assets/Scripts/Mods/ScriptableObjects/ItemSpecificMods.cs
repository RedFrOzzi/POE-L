using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Database
{
    [CreateAssetMenu(fileName = "ItemMods", menuName = "ScriptableObjects/ModsDatabase/ItemSpecificMods")]
    public class ItemSpecificMods : ScriptableObject
    {
        [SerializeField] private EquipmentType typeOfEquipment;
        public EquipmentType TypeOfEquipment { get
            {
                if (typeOfEquipment == EquipmentType.None)
                {
                    Debug.Log($"{name} does not have Equipment Type identifier");
                }

                return typeOfEquipment;
            } }

        [SerializeField] private List<string> prefixesModNames;
        [SerializeField] private List<float> prefixesModWeights;

        [SerializeField] private List<string> suffixesModNames;
        [SerializeField] private List<float> suffixesModWeights;


        public void InitializeDatabase()
        {
            if (prefixesModNames.Count != prefixesModWeights.Count)
                Debug.Log($"{name} prefixes have no correct weights");

            if (suffixesModNames.Count != suffixesModWeights.Count)
                Debug.Log($"{name} suffixes have no correct weights");
        }

        public ModBase GetWeightedMod(string[] currentModNames, ModType modType)
        {
            if (modType == ModType.Prefix)
            {
                return GetPrefix(currentModNames);
            }

            if (modType == ModType.Suffix)
            {
                return GetSuffix(currentModNames);
            }

            Debug.Log("Can not find Mod");

            return ModsDatabase.EmptyMod;
        }

        public (ModBase, ModType) GetWeightedModWithTag(string[] currentModNames, ModTag tag)
        {
            float[] selectChances = GetPrefAndSufWithTagSelectChances(currentModNames, tag);

            if (selectChances.Length == 0)
            {
                Debug.Log("There is no mods that satisfy required parameters");

                return (ModsDatabase.EmptyMod, ModType.None);
            }

            var rnd = UnityEngine.Random.value;

            float comulitiveChance = 0;
            int selectChancesIndex = 0;

            for (int i = 0; i < prefixesModWeights.Count; i++)
            {
                comulitiveChance += selectChances[selectChancesIndex];

                if (i == 0)
                {
                    if (rnd < comulitiveChance)
                    {
                        return (ModsDatabase.AllMods[prefixesModNames[i]].GetCopy(), ModType.Prefix);
                    }

                    selectChancesIndex++;

                    continue;
                }

                if (rnd < comulitiveChance && rnd >= selectChances[selectChancesIndex - 1])
                {
                    return (ModsDatabase.AllMods[prefixesModNames[i]].GetCopy(), ModType.Prefix);
                }

                selectChancesIndex++;
            }

            for (int i = 0; i < suffixesModWeights.Count; i++)
            {
                comulitiveChance += selectChances[selectChancesIndex];

                if (i == 0)
                {
                    if (rnd < comulitiveChance)
                    {
                        return (ModsDatabase.AllMods[suffixesModNames[i]].GetCopy(), ModType.Suffix);
                    }

                    selectChancesIndex++;

                    continue;
                }

                if (rnd < comulitiveChance && rnd >= selectChances[selectChancesIndex - 1])
                {
                    return (ModsDatabase.AllMods[suffixesModNames[i]].GetCopy(), ModType.Suffix);
                }

                selectChancesIndex++;
            }


            Debug.Log($"Can not find Mod with {tag} tag");

            return (ModsDatabase.EmptyMod, ModType.Prefix);
        }

        public (ModBase, ModType) GetRandomModWithTag(string[] exceptModNames, ModTag tag)
        {
            Dictionary<ModBase, ModType> mods = new();

            for (int i = 0; i < prefixesModNames.Count; i++)
            {
                if (exceptModNames.Contains(prefixesModNames[i]) || ModsDatabase.AllMods[prefixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    continue;
                }

                mods.Add(ModsDatabase.AllMods[prefixesModNames[i]], ModType.Prefix);
            }

            for (int i = 0; i < suffixesModNames.Count; i++)
            {
                if (exceptModNames.Contains(suffixesModNames[i]) || ModsDatabase.AllMods[suffixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    continue;
                }

                mods.Add(ModsDatabase.AllMods[suffixesModNames[i]], ModType.Suffix);
            }

            if (mods.Count > 0)
            {
                var mod = mods.PickRandom();

                return (mod.Key.GetCopy(), mod.Value);
            }


            Debug.Log($"Can not find Mod with {tag} tag");

            return (ModsDatabase.EmptyMod, ModType.Prefix);
        }

        public (string, ModType) GetRandomModNameInCurrentWithTag(string[] currentModNames, ModTag tag)
        {
            Dictionary<string, ModType> modNames = new();

            for (int i = 0; i < prefixesModNames.Count; i++)
            {
                if (currentModNames.Contains(prefixesModNames[i]) == false || ModsDatabase.AllMods[prefixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    continue;
                }

                modNames.Add(prefixesModNames[i], ModType.Prefix);
            }

            for (int i = 0; i < suffixesModNames.Count; i++)
            {
                if (currentModNames.Contains(suffixesModNames[i]) == false || ModsDatabase.AllMods[suffixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    continue;
                }

                modNames.Add(suffixesModNames[i], ModType.Suffix);
            }

            if (modNames.Count > 0)
            {
                var mod = modNames.PickRandom();

                return (mod.Key, mod.Value);
            }


            Debug.Log($"Can not find Mod with {tag} tag");

            return ("EmptyMod", ModType.Prefix);
        }

        //------------------------------------------------------------------------------------------------
        private ModBase GetPrefix(string[] currentPrefs)
        {
            float[] prefixesSelectChances = GetPrefixesSelectChances(currentPrefs);

            if (prefixesSelectChances.Length == 0)
            {
                Debug.Log("No mods left");

                return ModsDatabase.EmptyMod;
            }

            var rnd = UnityEngine.Random.value;

            float comulitiveChance = 0;

            for (int i = 0; i < prefixesSelectChances.Length; i++)
            {
                comulitiveChance += prefixesSelectChances[i];

                if (i == 0)
                {
                    if (rnd < comulitiveChance)
                    {
                        return ModsDatabase.AllMods[prefixesModNames[i]].GetCopy();
                    }

                    continue;
                }

                if (rnd < comulitiveChance && rnd >= prefixesSelectChances[i - 1])
                {
                    return ModsDatabase.AllMods[prefixesModNames[i]].GetCopy();
                }
            }

            Debug.Log("can not find weighted Mod");

            return ModsDatabase.EmptyMod;
        }

        private ModBase GetSuffix(string[] currentSufs)
        {
            float[] suffixesSelectChances = GetSuffixesSelectChances(currentSufs);

            if (suffixesSelectChances.Length == 0)
            {
                Debug.Log("No mods left");

                return ModsDatabase.EmptyMod;
            }

            var rnd = UnityEngine.Random.value;

            float comulitiveChance = 0;

            for (int i = 0; i < suffixesSelectChances.Length; i++)
            {
                comulitiveChance += suffixesSelectChances[i];

                if (i == 0)
                {
                    if (rnd < comulitiveChance)
                    {
                        return ModsDatabase.AllMods[suffixesModNames[i]].GetCopy();
                    }

                    continue;
                }

                if (rnd < comulitiveChance && rnd >= suffixesSelectChances[i - 1])
                {
                    return ModsDatabase.AllMods[suffixesModNames[i]].GetCopy();
                }
            }

            Debug.Log("can not find weighted Mod");

            return ModsDatabase.EmptyMod;
        }

        private float[] GetPrefixesSelectChances(string[] exceptModNames)
        {
            float totalWeight = 0;

            for (int i = 0; i < prefixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(prefixesModNames[i]))
                {
                    continue;
                }

                totalWeight += prefixesModWeights[i];
            }

            //≈сли все моды вз€ты, возврат пустого массива
            if (totalWeight == 0)
            {
                return Array.Empty<float>();
            }

            float[] prefSelectChances = new float[prefixesModWeights.Count];
            for (int i = 0; i < prefSelectChances.Length; i++)
            {
                if (exceptModNames.Contains(prefixesModNames[i]))
                {
                    continue;
                }

                prefSelectChances[i] = prefixesModWeights[i] / totalWeight;
            }

            return prefSelectChances;
        }

        private float[] GetSuffixesSelectChances(string[] exceptModNames)
        {
            float totalWeight = 0;

            for (int i = 0; i < suffixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(suffixesModNames[i]))
                {
                    continue;
                }

                totalWeight += suffixesModWeights[i];
            }

            //≈сли все моды вз€ты, возврат пустого массива
            if (totalWeight == 0)
            {
                return Array.Empty<float>();
            }

            float[] sufSelectChances = new float[suffixesModWeights.Count];
            for (int i = 0; i < sufSelectChances.Length; i++)
            {
                if (exceptModNames.Contains(suffixesModNames[i]))
                {
                    continue;
                }

                sufSelectChances[i] = suffixesModWeights[i] / totalWeight;
            }

            return sufSelectChances;
        }

        private float[] GetPrefAndSufSelectChances(string[] exceptModNames)
        {
            float totalWeight = 0;

            for (int i = 0; i < prefixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(prefixesModNames[i]))
                {
                    continue;
                }

                totalWeight += prefixesModWeights[i];
            }

            for (int j = 0; j < suffixesModWeights.Count; j++)
            {
                if (exceptModNames.Contains(suffixesModNames[j]))
                {
                    continue;
                }

                totalWeight += suffixesModWeights[j];
            }

            //≈сли все моды вз€ты, возврат пустого массива
            if (totalWeight == 0)
            {
                return Array.Empty<float>();
            }

            float[] selectChances = new float[prefixesModWeights.Count + suffixesModWeights.Count];
            int selectChancesIndex = 0;

            for (int i = 0; i < prefixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(prefixesModNames[i]))
                {
                    selectChancesIndex++;
                    continue;
                }

                selectChances[selectChancesIndex] = prefixesModWeights[i] / totalWeight;

                selectChancesIndex++;
            }

            for (int i = 0; i < suffixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(suffixesModNames[i]))
                {
                    selectChancesIndex++;
                    continue;
                }

                selectChances[selectChancesIndex] = suffixesModWeights[i] / totalWeight;

                selectChancesIndex++;
            }

            return selectChances;
        }

        private float[] GetPrefAndSufWithTagSelectChances(string[] exceptModNames, ModTag tag)
        {
            float totalWeight = 0;

            for (int i = 0; i < prefixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(prefixesModNames[i]) || ModsDatabase.AllMods[prefixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    continue;
                }

                totalWeight += prefixesModWeights[i];
            }

            for (int i = 0; i < suffixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(suffixesModNames[i]) || ModsDatabase.AllMods[suffixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    continue;
                }

                totalWeight += suffixesModWeights[i];
            }

            //≈сли все моды вз€ты, возврат пустого массива
            if (totalWeight == 0)
            {
                return Array.Empty<float>();
            }

            float[] selectChances = new float[prefixesModWeights.Count + suffixesModWeights.Count];
            int selectChancesIndex = 0;

            for (int i = 0; i < prefixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(prefixesModNames[i]) || ModsDatabase.AllMods[prefixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    selectChancesIndex++;
                    continue;
                }

                selectChances[selectChancesIndex] = prefixesModWeights[i] / totalWeight;

                selectChancesIndex++;
            }

            for (int i = 0; i < suffixesModWeights.Count; i++)
            {
                if (exceptModNames.Contains(suffixesModNames[i]) || ModsDatabase.AllMods[suffixesModNames[i]].ModTags.Contains(tag) == false)
                {
                    selectChancesIndex++;
                    continue;
                }

                selectChances[selectChancesIndex] = suffixesModWeights[i] / totalWeight;

                selectChancesIndex++;
            }

            return selectChances;
        }
    }
}