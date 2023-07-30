using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Database;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "CardsDatabase", menuName = "ScriptableObjects/CardsDatabase")]
public class CardsDatabase : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites = new();

    public Dictionary<string, Sprite> CardsSprites { get; private set; } = new();

    public Dictionary<string, Card> Cards { get; private set; } = new(); //карты в словаре
    public List<Card> CardsList { get; private set; } = new(); //те же карты в листе

    public void InitializeCardsDatabase()
    {
        InitializeSprites();

        InitializeCards();
    }

    private void InitializeCards()
    {
        var types = Assembly.GetAssembly(typeof(Card)).GetTypes().Where(x => typeof(Card).IsAssignableFrom(x));

        foreach (Type type in types)
        {
            Card card = Activator.CreateInstance(type) as Card;
            if (card.Name != "Base")
            {
                Cards.Add(card.Name, card);
                CardsList.Add(card);
            }
        }
    }

    private void InitializeSprites()
    {
        foreach (Sprite sprite in sprites)
        {
            if (CardsSprites.ContainsKey(sprite.name))
                continue;

            CardsSprites.Add(sprite.name, sprite);
        }
    }
}
