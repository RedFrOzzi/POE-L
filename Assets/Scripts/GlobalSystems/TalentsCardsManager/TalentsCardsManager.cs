using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class TalentsCardsManager : MonoBehaviour
{
    [SerializeField] private GameObject talentCardsPanel;
    [SerializeField] private TalentCard_UI_Element[] talentCards_UIs;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private EventTrigger acceptButton;

    private CH_Stats stats;
    private CH_TalentTree talentTree;
    private TalentsWeightings tw;
    private GameFlowManager flowManager;

    private int? selectedCard = null;

    private const float hideAlpha = 0.67f;
    private const float showAlpha = 1;

    private void Awake()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Stats>();
        talentTree = stats.GetComponent<CH_TalentTree>();
    }

    private void Start()
    {
        talentCardsPanel.SetActive(false);

        tw = GameDatabasesManager.Instance.TalentsWeightings;
        flowManager = GameFlowManager.Instance;

        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => AcceptCard());
        acceptButton.triggers.Add(entry);
    }

    public void OnLevelUp()
    {
        GenerateTalentCards();

        ShowTalentCardsPanel();
    }

    private void GenerateTalentCards()
    {
        textMesh.text = string.Empty;
        selectedCard = null;

        Dictionary<int, Talent> tempLockedTalents = new(talentTree.LockedTalents);

        for (byte i = 0; i < talentCards_UIs.Length; i++)
        {
            if (tempLockedTalents.Count <= 0)
            {
                byte rndNmbr = (byte)Mathf.Floor(Mathf.Sqrt(Random.Range(1.1f, 10f)));
                talentCards_UIs[i].SetTalentCard(i, rndNmbr, this);

                continue;
            }

            if (i == 0)
            {
                byte rndNmbr = (byte)Mathf.Floor(Mathf.Sqrt(Random.Range(1.1f, 10f)));
                talentCards_UIs[0].SetTalentCard(i, rndNmbr, this);

                continue;
            }


            (int index, Talent talent) = GetWeightedTalent(tempLockedTalents);
            if (talent == null)
            {
                Debug.Log($"Talent for {i} card not found");

                byte rndNmbr = (byte)Mathf.Floor(Mathf.Sqrt(Random.Range(1.1f, 10f)));
                talentCards_UIs[i].SetTalentCard(i, rndNmbr, this);

                continue;
            }

            tempLockedTalents.Remove(index);

            talentCards_UIs[i].SetTalentCard(i, index, talent, this);
        }
    }

    public void SelectCard(byte index, string text)
    {
        UnselectCards(index);

        selectedCard = index;

        talentCards_UIs[index].BGImage.SetTransparency(showAlpha);

        textMesh.text = text;
    }

    private void UnselectCards(byte exceptCardIndex)
    {
        foreach (var t in talentCards_UIs)
        {
            t.BGImage.SetTransparency(hideAlpha);
        }
    }

    private void AcceptCard()
    {
        if (selectedCard == null)
        {
            Debug.Log("Card is not selected");
            return;
        }

        if (talentCards_UIs[(int)selectedCard].IsGivingTalentPoint)
        {
            TakeTalentPoints(talentCards_UIs[(int)selectedCard].TalentPoints);
        }
        else
        {
            TakeTalentCard(talentCards_UIs[(int)selectedCard].TalentIndex, talentCards_UIs[(int)selectedCard].Talent);
        }
    }

    public void TakeTalentCard(int talentIndex, Talent talent)
    {
        talentTree.UnlockTalentByCard(talentIndex, talent);

        HideCardsPanel();
    }

    public void TakeTalentPoints(byte amount)
    {
        talentTree.AddTalentPoints(amount);

        HideCardsPanel();
    }

    private void ShowTalentCardsPanel()
    {
        talentCardsPanel.SetActive(true);
    }

    private void HideCardsPanel()
    {
        talentCardsPanel.SetActive(false);

        flowManager.OnTalentsCardsClose();
    }

    private (int, Talent) GetWeightedTalent(Dictionary<int, Talent> tempLockedTalents)
    {
        var rnd = Random.value;
        int index;
        Talent tal;
        //LEGENDARY
        if (rnd > tw.MythicTalentShowPercent + tw.EpicTalentShowPercent + tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent + tw.RareTalentShowPercent)
        {
            (index, tal) = GetLegendaryTalent(tempLockedTalents);
            if (tal == null)
            {
                (index, tal) = GetMythicTalent(tempLockedTalents);
                if (tal == null)
                {
                    (index, tal) = GetEpicTalent(tempLockedTalents);
                    if (tal == null)
                    {
                        (index, tal) = GetRareTalent(tempLockedTalents);
                        if (tal == null)
                        {
                            (index, tal) = GetUncommonTalent(tempLockedTalents);
                            if (tal == null)
                            {
                                return GetCommonOrAbove(tempLockedTalents);
                            }
                        }
                    }
                }
            }

            return (index, tal);
        }
        //MYTHIC
        if (rnd > tw.EpicTalentShowPercent + tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent + tw.RareTalentShowPercent)
        {
            (index, tal) = GetMythicTalent(tempLockedTalents);
            if (tal == null)
            {
                (index, tal) = GetEpicTalent(tempLockedTalents);
                if (tal == null)
                {
                    (index, tal) = GetRareTalent(tempLockedTalents);
                    if (tal == null)
                    {
                        (index, tal) = GetUncommonTalent(tempLockedTalents);
                        if (tal == null)
                        {
                            return GetCommonOrAbove(tempLockedTalents);
                        }
                    }
                }
            }

            return (index, tal);
        }
        //EPIC
        if (rnd > tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent + tw.RareTalentShowPercent)
        {
            (index, tal) = GetEpicTalent(tempLockedTalents);
            if (tal == null)
            {
                (index, tal) = GetRareTalent(tempLockedTalents);
                if (tal == null)
                {
                    (index, tal) = GetUncommonTalent(tempLockedTalents);
                    if (tal == null)
                    {
                        return GetCommonOrAbove(tempLockedTalents);
                    }
                }
            }

            return (index, tal);
        }
        //RARE
        if (rnd > tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent)
        {
            (index, tal) = GetRareTalent(tempLockedTalents);
            if (tal == null)
            {
                (index, tal) = GetUncommonTalent(tempLockedTalents);
                if (tal == null)
                {
                    return GetCommonOrAbove(tempLockedTalents);
                }
            }

            return (index, tal);
        }
        //UNCOMMON
        if (rnd > tw.CommonTalentShowPercent)
        {
            (index, tal) = GetUncommonTalent(tempLockedTalents);
            if (tal == null)
            {
                return GetCommonOrAbove(tempLockedTalents);
            }

            return (index, tal);
        }
        //COMMON
        if (rnd > 0)
        {
            return GetCommonOrAbove(tempLockedTalents);
        }

        return (0, null);
    }

    private (int index, Talent talent) GetLegendaryTalent(Dictionary<int, Talent> tempLockedTalents)
    {
        try
        {
            (int index, Talent talent) = tempLockedTalents
                .Where(x => x.Value.TalentRarity == Rarity.Legendary)
                .PickRandom();

            return (index, talent);
        }
        catch
        {
            return (0, null);
        }
    }

    private (int index, Talent talent) GetMythicTalent(Dictionary<int, Talent> tempLockedTalents)
    {
        try
        {
            (int index, Talent talent) = tempLockedTalents
                .Where(x => x.Value.TalentRarity == Rarity.Mythic)
                .PickRandom();

            return (index, talent);
        }
        catch
        {
            return (0, null);
        }
    }

    private (int index, Talent talent) GetEpicTalent(Dictionary<int, Talent> tempLockedTalents)
    {
        try
        {
            (int index, Talent talent) = tempLockedTalents
                .Where(x => x.Value.TalentRarity == Rarity.Epic)
                .PickRandom();

            return (index, talent);
        }
        catch
        {
            return (0, null);
        }
    }

    private (int index, Talent talent) GetRareTalent(Dictionary<int, Talent> tempLockedTalents)
    {
        try
        {
            (int index, Talent talent) = tempLockedTalents
                .Where(x => x.Value.TalentRarity == Rarity.Rare)
                .PickRandom();

            return (index, talent);
        }
        catch
        {
            return (0, null);
        }
    }

    private (int index, Talent talent) GetUncommonTalent(Dictionary<int, Talent> tempLockedTalents)
    {
        try
        {
            (int index, Talent talent) = tempLockedTalents
                .Where(x => x.Value.TalentRarity == Rarity.Uncommon)
                .PickRandom();

            return (index, talent);
        }
        catch
        {
            return (0, null);
        }
    }

    private (int index, Talent talent) GetCommonTalent(Dictionary<int, Talent> tempLockedTalents)
    {
        try
        {
            (int index, Talent talent) = tempLockedTalents
                .Where(x => x.Value.TalentRarity == Rarity.Common)
                .PickRandom();

            return (index, talent);
        }
        catch
        {
            return (0, null);
        }
    }

    private (int index, Talent talent) GetCommonOrAbove(Dictionary<int, Talent> tempLockedTalents)
    {
        Talent tal;
        int index;

        (index, tal) = GetCommonTalent(tempLockedTalents);
        if (tal == null)
        {
            (index, tal) = GetUncommonTalent(tempLockedTalents);
            if (tal == null)
            {
                (index, tal) = GetRareTalent(tempLockedTalents);
                if (tal == null)
                {
                    (index, tal) = GetEpicTalent(tempLockedTalents);
                    if (tal == null)
                    {
                        (index, tal) = GetMythicTalent(tempLockedTalents);
                        if (tal == null)
                        {
                            (index, tal) = GetLegendaryTalent(tempLockedTalents);
                            if (tal == null)
                            {
                                return (0, null);
                            }
                        }
                    }
                }
            }
        }

        return (index, tal);
    }
}
