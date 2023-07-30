using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "ItemSignatureModsDatabase", menuName = "ScriptableObjects/ItemSignatureModsDatabase/Database")]
public class ItemSignatureModsDatabase : ScriptableObject
{
    public static List<SignatureMod> UniversalWeaponSignatureModsT0 { get; private set; } = new();
    public static List<SignatureMod> UniversalWeaponSignatureModsT1 { get; private set; } = new();
    public static List<SignatureMod> UniversalWeaponSignatureModsT2 { get; private set; } = new();
    public static List<SignatureMod> UniversalWeaponSignatureModsT3 { get; private set; } = new();
    public static List<SignatureMod> UniversalWeaponSignatureModsT4 { get; private set; } = new();

    public static List<SignatureMod> ShotgunWeaponSignatureModsT0 { get; private set; } = new();
    public static List<SignatureMod> ShotgunWeaponSignatureModsT1 { get; private set; } = new();
    public static List<SignatureMod> ShotgunWeaponSignatureModsT2 { get; private set; } = new();
    public static List<SignatureMod> ShotgunWeaponSignatureModsT3 { get; private set; } = new();
    public static List<SignatureMod> ShotgunWeaponSignatureModsT4 { get; private set; } = new();

    public static List<SignatureMod> RocketLauncherWeaponSignatureModsT0 { get; private set; } = new();
    public static List<SignatureMod> RocketLauncherWeaponSignatureModsT1 { get; private set; } = new();
    public static List<SignatureMod> RocketLauncherWeaponSignatureModsT2 { get; private set; } = new();
    public static List<SignatureMod> RocketLauncherWeaponSignatureModsT3 { get; private set; } = new();
    public static List<SignatureMod> RocketLauncherWeaponSignatureModsT4 { get; private set; } = new();

    public static List<SignatureMod> PistolWeaponSignatureModsT0 { get; private set; } = new();
    public static List<SignatureMod> PistolWeaponSignatureModsT1 { get; private set; } = new();
    public static List<SignatureMod> PistolWeaponSignatureModsT2 { get; private set; } = new();
    public static List<SignatureMod> PistolWeaponSignatureModsT3 { get; private set; } = new();
    public static List<SignatureMod> PistolWeaponSignatureModsT4 { get; private set; } = new();

    public static List<SignatureMod> AssaultRifleWeaponSignatureModsT0 { get; private set; } = new();
    public static List<SignatureMod> AssaultRifleWeaponSignatureModsT1 { get; private set; } = new();
    public static List<SignatureMod> AssaultRifleWeaponSignatureModsT2 { get; private set; } = new();
    public static List<SignatureMod> AssaultRifleWeaponSignatureModsT3 { get; private set; } = new();
    public static List<SignatureMod> AssaultRifleWeaponSignatureModsT4 { get; private set; } = new();
    //---------------------------------------------------------
    public static List<SignatureMod> UniversalArmorSignatureModsT0 { get; private set; } = new();
    public static List<SignatureMod> UniversalArmorSignatureModsT1 { get; private set; } = new();
    public static List<SignatureMod> UniversalArmorSignatureModsT2 { get; private set; } = new();
    public static List<SignatureMod> UniversalArmorSignatureModsT3 { get; private set; } = new();
    public static List<SignatureMod> UniversalArmorSignatureModsT4 { get; private set; } = new();


    public void AddAllSignatureModsToDatabase()
    {
        var universalWeaponMods = Resources.LoadAll<SignatureMod>("SignatureMods/Weapon/Universal");
        UniversalWeaponSignatureModsT0 = new(universalWeaponMods.Where(x => x.Tier == 0));
        UniversalWeaponSignatureModsT1 = new(universalWeaponMods.Where(x => x.Tier == 1));
        UniversalWeaponSignatureModsT2 = new(universalWeaponMods.Where(x => x.Tier == 2));
        UniversalWeaponSignatureModsT3 = new(universalWeaponMods.Where(x => x.Tier == 3));
        UniversalWeaponSignatureModsT4 = new(universalWeaponMods.Where(x => x.Tier == 4));

        var shotgunWeaponMods = Resources.LoadAll<SignatureMod>("SignatureMods/Weapon/Shotgun");
        ShotgunWeaponSignatureModsT0 = new(shotgunWeaponMods.Where(x => x.Tier == 0));
        ShotgunWeaponSignatureModsT1 = new(shotgunWeaponMods.Where(x => x.Tier == 1));
        ShotgunWeaponSignatureModsT2 = new(shotgunWeaponMods.Where(x => x.Tier == 2));
        ShotgunWeaponSignatureModsT3 = new(shotgunWeaponMods.Where(x => x.Tier == 3));
        ShotgunWeaponSignatureModsT4 = new(shotgunWeaponMods.Where(x => x.Tier == 4));

        var rocketLauncherWeaponMods = Resources.LoadAll<SignatureMod>("SignatureMods/Weapon/RocketLauncher");
        RocketLauncherWeaponSignatureModsT0 = new(rocketLauncherWeaponMods.Where(x => x.Tier == 0));
        RocketLauncherWeaponSignatureModsT1 = new(rocketLauncherWeaponMods.Where(x => x.Tier == 1));
        RocketLauncherWeaponSignatureModsT2 = new(rocketLauncherWeaponMods.Where(x => x.Tier == 2));
        RocketLauncherWeaponSignatureModsT3 = new(rocketLauncherWeaponMods.Where(x => x.Tier == 3));
        RocketLauncherWeaponSignatureModsT4 = new(rocketLauncherWeaponMods.Where(x => x.Tier == 4));

        var pistolWeaponMods = Resources.LoadAll<SignatureMod>("SignatureMods/Weapon/Pistol");
        PistolWeaponSignatureModsT0 = new(pistolWeaponMods.Where(x => x.Tier == 0));
        PistolWeaponSignatureModsT1 = new(pistolWeaponMods.Where(x => x.Tier == 1));
        PistolWeaponSignatureModsT2 = new(pistolWeaponMods.Where(x => x.Tier == 2));
        PistolWeaponSignatureModsT3 = new(pistolWeaponMods.Where(x => x.Tier == 3));
        PistolWeaponSignatureModsT4 = new(pistolWeaponMods.Where(x => x.Tier == 4));

        var assaultRifleWeaponMods = Resources.LoadAll<SignatureMod>("SignatureMods/Weapon/AssaultRifle");
        AssaultRifleWeaponSignatureModsT0 = new(assaultRifleWeaponMods.Where(x => x.Tier == 0));
        AssaultRifleWeaponSignatureModsT0 = new(assaultRifleWeaponMods.Where(x => x.Tier == 1));
        AssaultRifleWeaponSignatureModsT0 = new(assaultRifleWeaponMods.Where(x => x.Tier == 2));
        AssaultRifleWeaponSignatureModsT0 = new(assaultRifleWeaponMods.Where(x => x.Tier == 3));
        AssaultRifleWeaponSignatureModsT0 = new(assaultRifleWeaponMods.Where(x => x.Tier == 4));

        var universalArmorMods = Resources.LoadAll<SignatureMod>("SignatureMods/Armor/Universal");
        UniversalArmorSignatureModsT0 = new(universalArmorMods.Where(x => x.Tier == 0));
        UniversalArmorSignatureModsT0 = new(universalArmorMods.Where(x => x.Tier == 1));
        UniversalArmorSignatureModsT0 = new(universalArmorMods.Where(x => x.Tier == 2));
        UniversalArmorSignatureModsT0 = new(universalArmorMods.Where(x => x.Tier == 3));
        UniversalArmorSignatureModsT0 = new(universalArmorMods.Where(x => x.Tier == 4));
    }
}
