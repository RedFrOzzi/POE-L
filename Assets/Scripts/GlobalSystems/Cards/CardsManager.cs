using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using System;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private GameObject cardsPanel;
    [SerializeField] private Card_UI_Element[] cards_UIs;

    private CH_Stats stats;
    private List<Card> takenCards = new();

    private void Awake()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Stats>();
    }

    private void Start()
    {
        cardsPanel.SetActive(false);

        stats.OnLevelUp += OnLevelUp;
    }

    private void OnDestroy()
    {
        stats.OnLevelUp -= OnLevelUp;
    }

    private void OnLevelUp()
    {
        GenerateCards();

        ShowCardsPanel();
    }

    private void GenerateCards()
    {
        //временное решение
        for (int i = 0; i < cards_UIs.Length; i++)
        {
            Card card = GameDatabasesManager.Instance.CardsDatabase.CardsList.PickRandom().CopyV2();
            byte tier = (byte)UnityEngine.Random.Range(0, card.TierValues.Length);
            cards_UIs[i].SetCard(card, tier, stats, this);
        }
    }

    private void ShowCardsPanel()
    {
        GameFlowManager.Instance.PauseGame(true);

        cardsPanel.SetActive(true);
    }

    private void HideCardsPanel()
    {
        cardsPanel.SetActive(false);

        GameFlowManager.Instance.PauseGame(false);
    }

    public void TakeCard(Card card)
    {
        if (card == null) { return; }

        card.ApplyCard(card.Tier);

        takenCards.Add(card);

        HideCardsPanel();
    }

    public void ShowTakenCards()
    {
        foreach (Card card in takenCards)
        {
            //do shit
        }
    }
}
