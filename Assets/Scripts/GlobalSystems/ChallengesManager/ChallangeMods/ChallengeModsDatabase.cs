using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Database;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "ChallengeModsDatabase", menuName = "ScriptableObjects/Challenges/ChallengeModsDatabase")]
public class ChallengeModsDatabase : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites = new();
    [Space(20)]
    [SerializeField] private List<ChallengeModsSO> ModsSO;

    public Dictionary<string, Sprite> ChallengeModsSprites { get; private set; } = new();

    public Dictionary<string, ChallengeMod> ChallengeMods { get; private set; } = new(); //моды в словаре
    public List<ChallengeMod> ChallengeModsList { get; private set; } = new(); //те же моды в листе

    public void InitializeDatabase()
    {
        InitializeSprites();

        Initialize();
    }

    private void Initialize()
    {
        ChallengeModsList.Clear();
        ChallengeMods.Clear();

        foreach (var modsList in ModsSO)
        {
            var list = modsList.GetMods();

            foreach (var item in list)
            {
                ChallengeMods.Add(item.Name, item);
            }

            list = list.Where(x => x.Name != "Empty").ToList();

            ChallengeModsList.AddRange(list);
        }
    }

    private void InitializeSprites()
    {
        ChallengeModsSprites.Clear();

        foreach (Sprite sprite in sprites)
        {
            if (ChallengeModsSprites.ContainsKey(sprite.name))
                continue;

            ChallengeModsSprites.Add(sprite.name, sprite);
        }
    }
}