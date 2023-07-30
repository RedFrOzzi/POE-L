using UnityEngine;
using Database;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AbilityWeights", menuName = "ScriptableObjects/Weights/AbilityWeights")]
public class AbilityWeights : ScriptableObject
{
    private AbilitiesDatabase abilitiesDatabase;

    [SerializeField] private float auraOfDecay;
    [SerializeField] private float blink;
    [SerializeField] private float bomb;
    [SerializeField] private float chainLightning;
    [SerializeField] private float imunity;
    [SerializeField] private float dash;
    [SerializeField] private float fireBall;
    [SerializeField] private float fireStorm;
    [SerializeField] private float lightningStorm;
    [SerializeField] private float molotov;
    [SerializeField] private float poisonTrail;
    [SerializeField] private float repel;
    [SerializeField] private float shuriken;
    [SerializeField] private float stomp;

    public float AuraOfDecayChance { get; private set; }
    public float BlinkChance { get; private set; }
    public float BombChance { get; private set; }
    public float ChainLightningChance { get; private set; }
    public float ImunityChance { get; private set; }
    public float DashChance { get; private set; }
    public float FireBallChance { get; private set; }
    public float FireStormChance { get; private set; }
    public float LightningStormChance { get; private set; }
    public float MolotovChance { get; private set; }
    public float PoisonTrailChance { get; private set; }
    public float RepelChance { get; private set; }
    public float ShurikenChance { get; private set; }
    public float StompChance { get; private set; }

    public void Initialize(AbilitiesDatabase abilitiesDatabase)
    {
        CheckIfAllAbilitiesHaveWeight();

        this.abilitiesDatabase = abilitiesDatabase;

        var totalWeight = auraOfDecay + blink + bomb + chainLightning + imunity + dash + fireBall +
            fireStorm + lightningStorm + molotov + poisonTrail + repel + shuriken + stomp;

        AuraOfDecayChance = auraOfDecay / totalWeight;
        BlinkChance = blink / totalWeight;
        BombChance = bomb / totalWeight;
        ChainLightningChance = chainLightning / totalWeight;
        ImunityChance = imunity / totalWeight;
        DashChance = dash / totalWeight;
        FireBallChance = fireBall / totalWeight;
        FireStormChance = fireStorm / totalWeight;
        LightningStormChance = lightningStorm / totalWeight;
        MolotovChance = molotov / totalWeight;
        PoisonTrailChance = poisonTrail / totalWeight;
        RepelChance = repel / totalWeight;
        ShurikenChance = shuriken / totalWeight;
        StompChance = stomp / totalWeight;
    }

    //Копии не нужны, абилка копируется внутри менеджера
    public Ability GetWeightedAbility()
    {
        var rnd = UnityEngine.Random.value;

        if (rnd > ShurikenChance + RepelChance + PoisonTrailChance + MolotovChance + LightningStormChance + FireStormChance +
            FireBallChance + DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Stomp];
        }

        if (rnd > RepelChance + PoisonTrailChance + MolotovChance + LightningStormChance + FireStormChance +
            FireBallChance + DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Shuriken];
        }

        if (rnd > PoisonTrailChance + MolotovChance + LightningStormChance + FireStormChance +
           FireBallChance + DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Repel];
        }

        if (rnd > MolotovChance + LightningStormChance + FireStormChance +
           FireBallChance + DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.PoisonTrail];
        }

        if (rnd > LightningStormChance + FireStormChance +
           FireBallChance + DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Molotov];
        }

        if (rnd > FireStormChance + FireBallChance + DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.LightningStorm];
        }

        if (rnd > FireBallChance + DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.FireStorm];
        }

        if (rnd > DashChance + ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.FireBall];
        }

        if (rnd > ImunityChance + ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Dash];
        }

        if (rnd > ChainLightningChance + BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Immunity];
        }

        if (rnd > BombChance + BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.ChainLightning];
        }

        if (rnd > BlinkChance + AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Bomb];
        }

        if (rnd > AuraOfDecayChance)
        {
            return abilitiesDatabase.Abilities[AbilityID.Blink];
        }

        return abilitiesDatabase.Abilities[AbilityID.AuraOfDecay];
    }

    private void CheckIfAllAbilitiesHaveWeight()
    {
        List<Ability> abilities = new();
        byte amountOfHidenProperties = 2;

        var types = Assembly.GetAssembly(typeof(Ability)).GetTypes().Where(x => typeof(Ability).IsAssignableFrom(x));

        var t = Assembly.GetAssembly(typeof(AbilityWeights)).GetType("AbilityWeights");

        var info = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (var type in types)
        {
            var clone = Activator.CreateInstance(type) as Ability;

            if (clone.ID == AbilityID.None || clone.ID == AbilityID.Void)
                continue;

            abilities.Add(clone);
        }

        if (info.Length - amountOfHidenProperties != abilities.Count)
        {
            throw new Exception("Not every ability have weight");
        }
    }
}
