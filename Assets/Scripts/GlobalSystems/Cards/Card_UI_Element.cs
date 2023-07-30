using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Card_UI_Element : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Card card;
    private CardsManager cardsManager;

    private Image BGImage;
    private Image image;
    private TextMeshProUGUI text;

    [SerializeField] private Color tier0Color;
    [SerializeField] private Color tier1Color;
    [SerializeField] private Color tier2Color;

    private const float hideAlpha = 0.67f;
    private const float showAlpha = 1;

    private void Awake()
    {
        BGImage = GetComponentsInChildren<Image>(true)[0];
        image = GetComponentsInChildren<Image>(true)[1];
        text = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cardsManager.TakeCard(card);
    }

    public void SetCard(Card card, byte tier, CH_Stats stats, CardsManager cardsManager)
    {
        this.card = card;
        this.cardsManager = cardsManager;
        card.SetStats(stats);
        card.SetTier(tier);

        image.sprite = card.Sprite;
        text.text = card.Description();

        SetBackgroundColor();
    }

    private void SetBackgroundColor()
    {
        switch (card.Tier)
        {
            case 0:
                BGImage.color = tier0Color;
                break;
            case 1:
                BGImage.color = tier1Color;
                break;
            case 2:
                BGImage.color = tier2Color;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BGImage.SetTransparency(showAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BGImage.SetTransparency(hideAlpha);
    }
}
