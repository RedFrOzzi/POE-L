using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System;

public class ModsHolder
{
    public EquipmentSlot EquipmentSlot { get; private set; }
    //public EquipmentItem EquipmentItem { get; private set; }
    //public AbilityGem AbilityGem { get; private set; }
    public IEquipmentItem EquipmentItem { get; private set; }

    public ModBase[] Implicits { get; private set; }
    public ModCollection Prefixes { get; private set; }
    public ModCollection Suffixes { get; private set; }

    private readonly ModsGenerator modsGenerator;


    public ModsHolder(EquipmentSlot equipmentSlot, IEquipmentItem equipmentItem)
    {
        EquipmentSlot = equipmentSlot;
        EquipmentItem = equipmentItem;
        Implicits = Array.Empty<ModBase>();
        modsGenerator = new(this);
    }

    //-------------------------------------------------------------------------

    public void SetImplicits(params ModBase[] implicits)
    {
        if (implicits.Length <= 0) { return; }

        Implicits = new ModBase[implicits.Length];

        for (byte i = 0; i < implicits.Length; i++)
        {
            Implicits[i] = implicits[i];

            if (Implicits[i].IsLocal)
            {
                HandleLocalMod(Implicits[i]);
            }
        }
    }

    public void GenerateInitialMods()
    {
        Prefixes = new (modsGenerator.GetPrefixesAmount());
        Suffixes = new (modsGenerator.GetSuffixesAmount());

        modsGenerator.GenerateInitialMods(Prefixes, Suffixes);

        ApplyLocalModsModifiers();
    }

    public void AddWeightedMod(ModType modType)
    {
        modsGenerator.AddWeightedMod(Prefixes, Suffixes, modType);
    }

    public void RemoveMod(string name)
    {
        modsGenerator.RemoveMod(name, Prefixes, Suffixes);
    }

    public void AddWeightedModWithTag(ModTag tag)
    {
        modsGenerator.AddWeightedModWithTag(Prefixes, Suffixes, tag);
    }

    public void RemoveRandomModWithTag(ModTag tag)
    {
        modsGenerator.RemoveRandomModWithTag(Prefixes, Suffixes, tag);
    }

    private void ApplyLocalModsModifiers()
    {
        foreach (ModBase mod in Implicits)
        {
            if (mod != null && mod.IsLocal)
            {
                mod.ApplyMod(mod);
            }
        }

        foreach (ModBase mod in Prefixes)
        {
            if (mod != null && mod.IsLocal)
            {
                mod.ApplyMod(mod);
            }
        }

        foreach (ModBase mod in Suffixes)
        {
            if (mod != null && mod.IsLocal)
            {
                mod.ApplyMod(mod);
            }
        }
    }

    public void ApplyGlobalModsModifiers(CH_Stats stats)
    {
        foreach (ModBase mod in Implicits)
        {
            ApplyGlobalModModifiers(stats, mod);
        }

        foreach (ModBase mod in Prefixes)
        {
            ApplyGlobalModModifiers(stats, mod);
        }

        foreach (ModBase mod in Suffixes)
        {
            ApplyGlobalModModifiers(stats, mod);
        }
    }


    public void RemoveLocalModsModifiers()
    {
        foreach (ModBase mod in Implicits)
        {
            if (mod != null && mod.IsLocal)
            {
                mod.RemoveMod(mod);
            }
        }

        foreach (ModBase mod in Prefixes)
        {
            if (mod != null && mod.IsLocal)
            {
                mod.RemoveMod(mod);
            }
        }
        
        foreach (ModBase mod in Suffixes)
        {
            if (mod != null && mod.IsLocal)
            {
                mod.RemoveMod(mod);
            }
        }
    }

    public void RemoveGlobalModsModifiers()
    {
        foreach (ModBase mod in Implicits)
        {
            if (mod != null && mod.IsLocal == false)
            {
                mod.RemoveMod(mod);
            }
        }

        foreach (ModBase mod in Prefixes)
        {
            if (mod != null && mod.IsLocal == false)
            {
                mod.RemoveMod(mod);
            }
        }
        
        foreach (ModBase mod in Suffixes)
        {
            if (mod != null && mod.IsLocal == false)
            {
                mod.RemoveMod(mod);
            }
        }
    }


    private void ApplyGlobalModModifiers(CH_Stats stats, ModBase mod)
    {
        if (mod != null && mod.IsLocal == false)
        {
            if (mod is ModForGlobalStats modForGlobalStats)
                modForGlobalStats.SetStats(stats);

            if (mod is ModForLeftHand modForLeftHand)
                modForLeftHand.SetStats(stats);

            mod.ApplyMod(mod);
        }
    }

    private void HandleLocalMod(ModBase mod)
    {
        if (mod is ModForArmor modForArmor)
        {
            modForArmor.SetArmor((Armor)EquipmentItem);
        }
        else if (mod is IWeaponMod modForIWeapon)
        {
            modForIWeapon.SetItem((IWeaponItem)EquipmentItem);
        }
        else if (mod is ModForAbilityGem modForAbilityGem)
        {
            modForAbilityGem.SetAbilityGem((AbilityGem)EquipmentItem);
        }
    }
}
