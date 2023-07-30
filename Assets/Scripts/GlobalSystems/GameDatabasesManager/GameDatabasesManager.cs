using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDatabasesManager : MonoBehaviour
{
    public static GameDatabasesManager Instance = null;


    [field:SerializeField] public BuffsDatabase BuffsDatabase { get; private set; }
    [field:SerializeField] public ItemSignatureModsDatabase ItemSignatureModsDatabase { get; private set; }
    [field:SerializeField] public ModsDatabase ModsDatabase { get; private set; }
    [field:SerializeField] public SpawnChancesDatabase SpawnChancesDatabase { get; private set; }
    [field:SerializeField] public CardsDatabase CardsDatabase { get; private set; }
    [field:SerializeField] public ChallengeModsDatabase ChalengeModsDatabase { get; private set; }
    [field:SerializeField] public ChallengeRewardsDatabase ChallengeRewardsDatabase { get; private set; }
    [field:SerializeField] public ChallengeConditionsDatabase ChallengeConditionsDatabase { get; private set; }
    [field:SerializeField] public AnimationsDatabase AnimationsDatabase { get; private set; }
    [field:SerializeField] public TalentsWeightings TalentsWeightings { get; private set; }
    [field:SerializeField] public AbilitiesDatabase AbilitiesDatabase { get; private set; }
    [field:SerializeField] public AbilityWeights AbilityWeights { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        BuffsDatabase.AddAllBuffsToDatabase();
        ItemSignatureModsDatabase.AddAllSignatureModsToDatabase();
        SpawnChancesDatabase.Initialize();
        ModsDatabase.Initialize();
        CardsDatabase.InitializeCardsDatabase();
        ChalengeModsDatabase.InitializeDatabase();
        ChallengeRewardsDatabase.Initialize();
        ChallengeConditionsDatabase.Initialize();
        AnimationsDatabase.Initialize();
        TalentsWeightings.InitializeWeights();
        AbilitiesDatabase.Initialize();
        AbilityWeights.Initialize(AbilitiesDatabase);
    }
}
