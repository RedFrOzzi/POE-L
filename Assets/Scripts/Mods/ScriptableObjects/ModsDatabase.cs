using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Database;

[CreateAssetMenu(fileName = "ModsDatabase", menuName = "ScriptableObjects/ModsDatabase/ModsDatabase")]
public class ModsDatabase : ScriptableObject
{
    [SerializeField] private TypeSpecificMods typeSpecificModsScriptableObject;

    public static readonly Dictionary<string, ModBase> AllMods = new();

    public static readonly Dictionary<string, ModForGlobalStats> GlobalMods = new();
    public static readonly Dictionary<string, ModForWeapon> WeaponMods = new();
    public static readonly Dictionary<string, ModForArmor> ArmorMods = new();
    public static readonly Dictionary<string, ModForLeftHand> LeftHandMods = new();
    public static readonly Dictionary<string, ModForAbilityGem> AbilityGemMods = new();

    public static ModBase EmptyMod {
        get
        {
            if (GlobalMods.ContainsKey("EmptyMod"))
                return GlobalMods["EmptyMod"].GetCopy();
            
            ModForGlobalStats m = new();

            m.Name = "EmptyMod";
            m.IsLocal = false;
            m.ModTags[0] = ModTag.None;
            m.TierValues = Array.Empty<float>();
            m.Description = (m) => "Do nothing";
            m.ApplyMod = (m) => { return; };
            m.RemoveMod = (m) => { return; };
            return m;
        }
    }

    public void Initialize()
    {
        FindAllMods();

        typeSpecificModsScriptableObject.InitializeDatabase();
    }

    public ModBase GetWeightedMod(EquipmentType equipmentType, string[] exceptModNames, ModType modType)
    {
        return typeSpecificModsScriptableObject.GetWeightedMod(equipmentType, exceptModNames, modType);
    }

    public (ModBase, ModType) GetWeightedModWithTag(EquipmentType equipmentType, string[] exceptModNames, ModTag tag)
    {
        return typeSpecificModsScriptableObject.GetWeightedModWithTag(equipmentType, exceptModNames, tag);
    }

    public (ModBase, ModType) GetRandomModWithTag(EquipmentType equipmentType, string[] exceptModNames, ModTag tag)
    {
        return typeSpecificModsScriptableObject.GetRandomModWithTag(equipmentType, exceptModNames, tag);
    }

    public (string, ModType) GetRandomModNameInCurrentWithTag(EquipmentType equipmentType, string[] currentModNames, ModTag tag)
    {
        return typeSpecificModsScriptableObject.GetRandomModNameInCurrentWithTag(equipmentType, currentModNames, tag);
    }

    //----------------------------------------------------------------------------------------------------------
    private void FindAllMods()
    {
        AllMods.Clear();
        GlobalMods.Clear();
        WeaponMods.Clear();
        ArmorMods.Clear();
        LeftHandMods.Clear();
        AbilityGemMods.Clear();

        var mods = Database.AbilityGemMods.GetAbilityGemMods();
        foreach (var m in mods)
        {
            AbilityGemMods.Add(m.Name, m);
            AllMods.Add(m.Name, m);
        }

        var mods2 = Database.ArmorMods.GetArmorMods();
        foreach (var m in mods2)
        {
            ArmorMods.Add(m.Name, m);
            AllMods.Add(m.Name, m);
        }

        var mods3 = Database.WeaponMods.GetWeaponMods();
        foreach (var m in mods3)
        {
            WeaponMods.Add(m.Name, m);
            AllMods.Add(m.Name, m);
        }

        var mods4 = Database.GlobalMods.GetGlobalMods();
        foreach (var m in mods4)
        {
            GlobalMods.Add(m.Name, m);
            AllMods.Add(m.Name, m);
        }

        var mods5 = ChargingGunMods.GetWeaponMods();
        foreach (var m in mods5)
        {
            WeaponMods.Add(m.Name, m);
            AllMods.Add(m.Name, m);
        }

        var mods6 = LaunchersMods.GetWeaponMods();
        foreach (var m in mods6)
        {
            WeaponMods.Add(m.Name, m);
            AllMods.Add(m.Name, m);
        }
    }
}