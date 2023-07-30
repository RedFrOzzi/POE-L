using UnityEngine;
using Database;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TalentCard_UI_Element : MonoBehaviour, IPointerClickHandler
{
    public bool IsGivingTalentPoint { get; private set; }
    public byte TalentPoints { get; private set; }
    public int TalentIndex { get; private set; }
    public Talent Talent { get; private set; }

    private byte talentCardIndex;

    private TalentsCardsManager talentsCardsManager;

    public Image BGImage { get; private set; }
    private Image image;

    private const float hideAlpha = 0.67f;

    private void Awake()
    {
        BGImage = GetComponentsInChildren<Image>(true)[0];
        image = GetComponentsInChildren<Image>(true)[2];
    }    

    public void SetTalentCard(byte cardIndex, int talentIndex, Talent talent, TalentsCardsManager talentsCardsManager)
    {
        IsGivingTalentPoint = false;

        talentCardIndex = cardIndex;
        TalentIndex = talentIndex;
        this.Talent = talent;
        this.talentsCardsManager = talentsCardsManager;

        image.sprite = TalentsDatabase.TalentSprites[talent.Name];
        BGImage.color = RarityColors.Color[talent.TalentRarity];
        BGImage.SetTransparency(hideAlpha);
    }

    public void SetTalentCard(byte cardIndex, byte talentPoints, TalentsCardsManager talentsCardsManager)
    {
        IsGivingTalentPoint = true;

        talentCardIndex = cardIndex;
        TalentPoints = talentPoints;
        this.talentsCardsManager = talentsCardsManager;

        //image.sprite = TalentsDatabase.TalentSprites[TalentID.StartingPoint];
        BGImage.color = RarityColors.Color[Rarity.Common];
        BGImage.SetTransparency(hideAlpha);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsGivingTalentPoint == false)
        {
            talentsCardsManager.SelectCard(talentCardIndex, Talent.Description);
        }
        else
        {
            if (TalentPoints == 1)
                talentsCardsManager.SelectCard(talentCardIndex, $"Take one talent point");
            else
                talentsCardsManager.SelectCard(talentCardIndex, $"Take {TalentPoints} talent point's");
        }

    }
}
