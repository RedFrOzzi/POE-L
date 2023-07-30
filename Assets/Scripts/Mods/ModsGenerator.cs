using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System.Linq;
using System;

public class ModsGenerator
{
    private readonly ModsHolder modsHolder;
    private readonly GameFlowManager gameFlowManager;
    private readonly ModsDatabase modsDatabase;

    public ModsGenerator(ModsHolder modsHolder)
    {
        this.modsHolder = modsHolder;
        gameFlowManager = GameFlowManager.Instance;
        modsDatabase = GameDatabasesManager.Instance.ModsDatabase;
    }

    //-------------------------------

    public byte GetPrefixesAmount()
    {
        int difficulty = gameFlowManager.GetDifficultyForTimedRound();

        return difficulty switch
        {
            0 => 1,
            1 => 1,
            2 => 2,
            3 => 2,
            4 => 3,
            5 => 3,
            _ => 3
        };
    }

    public byte GetSuffixesAmount()
    {
        int difficulty = gameFlowManager.GetDifficultyForTimedRound();

        return difficulty switch
        {
            0 => 1,
            1 => 1,
            2 => 2,
            3 => 2,
            4 => 3,
            5 => 3,
            _ => 3
        };
    }

    public void GenerateInitialMods(ModCollection prefixesToModify, ModCollection suffixesToModify)
    {
        DefaultModGenerator(prefixesToModify, suffixesToModify);

        foreach (ModBase mod in prefixesToModify)
        {
            if (mod == null)
                continue;

            if (mod.IsLocal)
            {
                HandleLocalMod(mod, modsHolder);
            }
        }

        foreach (ModBase mod in suffixesToModify)
        {
            if (mod == null)
                continue;

            if (mod.IsLocal)
            {
                HandleLocalMod(mod, modsHolder);
            }
        }
    }

    public void AddWeightedMod(ModCollection prefixes, ModCollection suffixes, ModType modType)
    {
        if (modType == ModType.Prefix)
        {
            if (prefixes.All(x => x.Name != "EmptyMod"))
            {
                Debug.Log("Prefixes is full");

                return;
            }

            for (int i = 0; i < prefixes.Length; i++)
            {
                if (prefixes[i].Name == "EmptyMod")
                {
                    prefixes[i] = modsDatabase.GetWeightedMod(modsHolder.EquipmentItem.EquipmentType, GetExceptionListOfModNames(prefixes), ModType.Prefix);

                    //Get weighted tier for this mod
                    if (prefixes[i].Name != "EmptyMod")
                        prefixes[i].SetTier(GetWeightedTier(prefixes[i]));

                    //Apply mod if it is local
                    if (prefixes[i].IsLocal)
                    {
                        HandleLocalMod(prefixes[i], modsHolder);

                        prefixes[i].ApplyMod(prefixes[i]);
                    }

                    return;
                }
            }
        }

        if (modType == ModType.Suffix)
        {
            if (suffixes.All(x => x.Name != "EmptyMod"))
            {
                Debug.Log("Suffixes is full");

                return;
            }

            for (int i = 0; i < suffixes.Length; i++)
            {
                if (suffixes[i].Name == "EmptyMod")
                {
                    //Get weighted mod
                    suffixes[i] = modsDatabase.GetWeightedMod(modsHolder.EquipmentItem.EquipmentType, GetExceptionListOfModNames(suffixes), ModType.Suffix);

                    //Get weighted tier for this mod
                    if (suffixes[i].Name != "EmptyMod")
                        suffixes[i].SetTier(GetWeightedTier(suffixes[i]));

                    //Apply mod if it is local
                    if (suffixes[i].IsLocal)
                    {
                        HandleLocalMod(suffixes[i], modsHolder);

                        suffixes[i].ApplyMod(suffixes[i]);
                    }

                    return;
                }
            }
        }
    }

    public void RemoveMod(string name, ModCollection prefixes, ModCollection suffixes)
    {
        bool nameIsPresentInPrefixes = false;
        foreach (var mod in prefixes)
        {
            if (mod.Name == name)
                nameIsPresentInPrefixes = true;
        }

        if (nameIsPresentInPrefixes)
        {
            for (int i = 0; i < prefixes.Length; i++)
            {
                if (prefixes[i].Name == name)
                {
                    if (prefixes[i].IsLocal)
                    {
                        prefixes[i].RemoveMod(prefixes[i]);
                    }

                    prefixes[i] = ModsDatabase.EmptyMod;

                    return;
                }
            }
        }

        if (nameIsPresentInPrefixes == false)
        {
            for (int i = 0; i < suffixes.Length; i++)
            {
                if (suffixes[i].Name == name)
                {
                    if (suffixes[i].IsLocal)
                    {
                        suffixes[i].RemoveMod(suffixes[i]);
                    }

                    suffixes[i] = ModsDatabase.EmptyMod;

                    return;
                }
            }
        }

        Debug.Log($"There is no mod with {name} name");
    }

    public void AddWeightedModWithTag(ModCollection prefixes, ModCollection suffixes, ModTag tag)
    {
        bool prefixesAreFull = prefixes.All(x => x.Name != "EmptyMod");
        bool suffixesAreFull = suffixes.All(x => x.Name != "EmptyMod");

        if (prefixesAreFull && suffixesAreFull)
        {
            Debug.Log("Mods are full");

            return;
        }

        //Get weighted mod and its position type
        (ModBase mod, ModType type) = modsDatabase.GetWeightedModWithTag(modsHolder.EquipmentItem.EquipmentType, GetExceptionListOfModNames(prefixes.Concat(suffixes).ToArray()), tag);

        //Get weighted tier for this mod
        if (mod.Name != "EmptyMod")
            mod.SetTier(GetWeightedTier(mod));

        if (type == ModType.Prefix)
        {
            for (int i = 0; i < prefixes.Length; i++)
            {
                //Set first empty slot to choosen mod
                if (prefixes[i].Name == "EmptyMod")
                {
                    prefixes[i] = mod;

                    //Apply mod if it is local
                    if (prefixes[i].IsLocal)
                    {
                        HandleLocalMod(prefixes[i], modsHolder);

                        prefixes[i].ApplyMod(prefixes[i]);
                    }

                    return;
                }
            }

            Debug.Log("Prefixes are full");
        }

        if (type == ModType.Suffix)
        {
            for (int i = 0; i < suffixes.Length; i++)
            {
                if (suffixes[i].Name == "EmptyMod")
                {
                    suffixes[i] = mod;

                    if (suffixes[i].IsLocal)
                    {
                        HandleLocalMod(suffixes[i], modsHolder);

                        suffixes[i].ApplyMod(suffixes[i]);
                    }

                    return;
                }
            }

            Debug.Log("Suffixes are full");
        }
    }

    public void RemoveRandomModWithTag(ModCollection prefixes, ModCollection suffixes, ModTag tag)
    {
        bool prefixesAreEmpty = prefixes.All(x => x.Name == "EmptyMod");
        bool suffixesAreEmpty = suffixes.All(x => x.Name == "EmptyMod");

        if (prefixesAreEmpty && suffixesAreEmpty)
        {
            Debug.Log("Mods are empty");

            return;
        }

        (string modName, _) = modsDatabase.GetRandomModNameInCurrentWithTag(modsHolder.EquipmentItem.EquipmentType, GetCurrentListOfModNames(prefixes.Concat(suffixes).ToArray()), tag);

        if (modName == "EmptyMod")
        {
            Debug.Log("No mods with this tag found");

            return;
        }

        RemoveMod(modName, prefixes, suffixes);
    }


    //-----------------------------------------------------------------------------------
    private void DefaultModGenerator(ModCollection prefixes, ModCollection suffixes)
    {
        if (prefixes.Length > 0)
        {
            for (int i = 0; i < prefixes.Length; i++)
            {
                float rnd = UnityEngine.Random.value;
                float chanceToGetEmptyMod = 0.4f;

                if (rnd < chanceToGetEmptyMod)
                {
                    prefixes[i] = ModsDatabase.AllMods["EmptyMod"].GetCopy();
                    continue;
                }

                prefixes[i] = modsDatabase.GetWeightedMod(modsHolder.EquipmentItem.EquipmentType, GetExceptionListOfModNames(prefixes), ModType.Prefix);

                if (prefixes[i].Name != "EmptyMod")
                    prefixes[i].SetTier(GetWeightedTier(prefixes[i]));
            }
        }

        if (suffixes.Length > 0)
        {
            for (int i = 0; i < suffixes.Length; i++)
            {
                float rnd = UnityEngine.Random.value;
                float chanceToGetEmptyMod = 0.4f;

                if (rnd < chanceToGetEmptyMod)
                {
                    suffixes[i] = ModsDatabase.AllMods["EmptyMod"].GetCopy();
                    continue;
                }

                suffixes[i] = modsDatabase.GetWeightedMod(modsHolder.EquipmentItem.EquipmentType, GetExceptionListOfModNames(suffixes), ModType.Suffix);

                if (suffixes[i].Name != "EmptyMod")
                    suffixes[i].SetTier(GetWeightedTier(suffixes[i]));
            }
        }
    }

    private byte GetWeightedTier(ModBase mod)
    {
        (int lowerTier, int upperTier) = GetLowerAndUpperPosibleTier(mod.TierValues.Length);

        float totalWeigh = 0;

        for (int i = lowerTier; i <= upperTier; i++)
        {
            try
            {
                totalWeigh += mod.TierWeights[i];
            }
            catch
            {
                Debug.Log(mod.Name);
            }
        }

        float[] dropChance = new float[upperTier + 1 - lowerTier];
        int index = 0;
        for (int i = lowerTier; i <= upperTier; i++)
        {
            dropChance[index] = mod.TierWeights[i] / totalWeigh;
            index++;
        }

        var rnd = UnityEngine.Random.value;

        float comulitiveChance = 0;
        for (int i = 0; i < dropChance.Length; i++)
        {
            comulitiveChance += dropChance[i];

            if (i == 0)
            {
                if (rnd < comulitiveChance)
                {
                    return (byte)lowerTier;
                }

                continue;
            }

            if (rnd < comulitiveChance && rnd >= dropChance[i - 1])
            {
                return (byte)(i + lowerTier);
            }
        }

        Debug.Log("can not find weighted tier");

        return 0;
    }

    private (int lower, int upper) GetLowerAndUpperPosibleTier(int tiersAmount)
    {
        int currentDifficulty = gameFlowManager.GetDifficultyForTimedRound();
        int window = GetTiersWindow(tiersAmount, currentDifficulty);

        if (currentDifficulty == 5)
        {
            int lowerLimit = 0;
            int upperLimit = window - 1;

            return (lowerLimit, upperLimit);
        }

        if (currentDifficulty == 4)
        {
            int lowerLimit = (int)((tiersAmount - window) * 0.125f);
            int upperLimit = lowerLimit + window - 1;

            return (lowerLimit, upperLimit);
        }

        if (currentDifficulty == 3)
        {
            int lowerLimit = (int)((tiersAmount - window) * 0.25f);
            int upperLimit = lowerLimit + window - 1;

            return (lowerLimit, upperLimit);
        }

        if (currentDifficulty == 2)
        {
            int lowerLimit = (int)((tiersAmount - window) * 0.375f);
            int upperLimit = lowerLimit + window - 1;

            return (lowerLimit, upperLimit);
        }

        if (currentDifficulty == 1)
        {
            int lowerLimit = (int)((tiersAmount - window) * 0.5f);
            int upperLimit = lowerLimit + window - 1;

            return (lowerLimit, upperLimit);
        }

        if (currentDifficulty == 0)
        {
            int lowerLimit = tiersAmount - window;
            int upperLimit = tiersAmount - 1;

            return (lowerLimit, upperLimit);
        }

        Debug.Log("Tier can not be choosen");

        return (tiersAmount - 1, tiersAmount - 1);
    }

    private int GetTiersWindow(int tiersAmount, int currentDiff)
    {
        if (currentDiff < 1)
        {
            return 1;
        }

        if (currentDiff < 3)
        {
            if (tiersAmount >= 4)
                return 2;
            if (tiersAmount >= 1)
                return 1;
        }

        if (currentDiff < 6)
        {
            if (tiersAmount >= 10)
                return 5;
            if (tiersAmount >= 8)
                return 4;
            if (tiersAmount >= 6)
                return 3;
            if (tiersAmount >= 4)
                return 2;
            if (tiersAmount >= 1)
                return 1;
        }

        Debug.Log("could not get window for tiers");
        return 0;
    }

    private string[] GetExceptionListOfModNames(ModBase[] mods)
    {
        string[] names = new string[mods.Length];
        for (var i = 0; i < names.Length; i++)
        {
            names[i] = (mods[i] != null) ? mods[i].Name : string.Empty;
        }
        return names;
    }

    private string[] GetExceptionListOfModNames(ModCollection mods)
    {
        string[] names = new string[mods.Length];
        for (var i = 0; i < names.Length; i++)
        {
            names[i] = (mods[i] != null) ? mods[i].Name : string.Empty;
        }
        return names;
    }

    private string[] GetCurrentListOfModNames(ModBase[] mods)
    {
        string[] names = new string[mods.Length];
        for (var i = 0; i < names.Length; i++)
        {
            names[i] = (mods[i] != null) ? mods[i].Name : string.Empty;
        }
        return names;
    }


    //---------------------------------------------------------------------
    private void HandleLocalMod(ModBase mod, ModsHolder modsHolder)
    {
        if (mod is ModForArmor modForArmor)
        {
            modForArmor.SetArmor((Armor)modsHolder.EquipmentItem);
        }
        else if (mod is IWeaponMod modForIWeapon)
        {
            modForIWeapon.SetItem((IWeaponItem)modsHolder.EquipmentItem);
        }
        else if (mod is ModForAbilityGem modForAbilityGem)
        {
            modForAbilityGem.SetAbilityGem((AbilityGem)modsHolder.EquipmentItem);
        }
    }
}
