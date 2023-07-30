using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class TalentGroup : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField, Space(20)] public TalentGroup ParentTalentGroup { get; private set; }
    [field: SerializeField] public TalentGroup[] ChildTalentGroups { get; private set; }

    [field: SerializeField, Space(5)] public bool useDifferenceMulti { get; private set; }
    [field: SerializeField] public int PointsDifference { get; private set; }
    [field: SerializeField] public int PointsDifferenceMilti { get; private set; }

    [SerializeField, Space(20)] private List<Talent> talents;

    [ContextMenuItem("Rename", "RenameGameObjects"), Space(10)]
    public string Rename = "Right click to rename every talent in group";

    private CH_TalentTree talentTree;

    private TalentsWeightings tw;

    //tooltip fields
    private float nextClickTime;
    private const float clickCooldown = 0.2f;


    private void Awake()
    {
        talents = new(GetComponentsInChildren<Talent>());
        talentTree = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_TalentTree>();

        var GOs = GameObject.FindGameObjectsWithTag("TalentGroup");
        var groups = new TalentGroup[GOs.Length];
        for (int i = 0; i < GOs.Length; i++)
        {
            groups[i] = GOs[i].GetComponent<TalentGroup>();
        }

        ChildTalentGroups = groups.Where(x => x.ParentTalentGroup == this).ToArray();
    }

    private void Start()
    {
        tw = GameDatabasesManager.Instance.TalentsWeightings;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (nextClickTime > Time.unscaledTime) { return; }
        nextClickTime = Time.unscaledTime + clickCooldown;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var tal = GetWeightedTalent();

            if (tal == null)
            {
                Debug.Log("There is no locked talents left");
                return;
            }

            talentTree.TryTurnTalent(this, tal.IndexID, true);
        }
    }    

    private Talent GetWeightedTalent()
    {
        var lockedTalents = talentTree.LockedTalents.Where(x => x.Value.TalentGroup == this).ToArray();

        if (lockedTalents.Length <= 0)
        {
            Debug.Log("Locked Talents list is 0 length");
            return null;
        }

        var rnd = UnityEngine.Random.value;

        //LEGENDARY
        if (rnd > tw.MythicTalentShowPercent + tw.EpicTalentShowPercent + tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent + tw.RareTalentShowPercent)
        {
            Talent tal = GetLegendaryTalent(lockedTalents);
            if (tal == null)
            {
                tal = GetMythicTalent(lockedTalents);
                if (tal == null)
                {
                    tal = GetEpicTalent(lockedTalents);
                    if (tal == null)
                    {
                        tal = GetRareTalent(lockedTalents);
                        if (tal == null)
                        {
                            tal = GetUncommonTalent(lockedTalents);
                            if (tal == null)
                            {
                                return GetCommonOrAbove(lockedTalents);
                            }
                        }
                    }
                }
            }

            return tal;
        }
        //MYTHIC
        if (rnd > tw.EpicTalentShowPercent + tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent + tw.RareTalentShowPercent)
        {
            Talent tal = GetMythicTalent(lockedTalents);
            if (tal == null)
            {
                tal = GetEpicTalent(lockedTalents);
                if (tal == null)
                {
                    tal = GetRareTalent(lockedTalents);
                    if (tal == null)
                    {
                        tal = GetUncommonTalent(lockedTalents);
                        if (tal == null)
                        {
                            return GetCommonOrAbove(lockedTalents);
                        }
                    }
                }
            }

            return tal;
        }
        //EPIC
        if (rnd > tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent + tw.RareTalentShowPercent)
        {
            Talent tal = GetEpicTalent(lockedTalents);
            if (tal == null)
            {
                tal = GetRareTalent(lockedTalents);
                if (tal == null)
                {
                    tal = GetUncommonTalent(lockedTalents);
                    if (tal == null)
                    {
                        return GetCommonOrAbove(lockedTalents);
                    }
                }
            }

            return tal;
        }
        //RARE
        if (rnd > tw.CommonTalentShowPercent + tw.UncommonTalentShowPercent)
        {
            Talent tal = GetRareTalent(lockedTalents);
            if (tal == null)
            {
                tal = GetUncommonTalent(lockedTalents);
                if (tal == null)
                {
                    return GetCommonOrAbove(lockedTalents);
                }
            }

            return tal;
        }
        //UNCOMMON
        if (rnd > tw.CommonTalentShowPercent)
        {
            Talent tal = GetUncommonTalent(lockedTalents);
            if (tal == null)
            {
                return GetCommonOrAbove(lockedTalents);
            }

            return tal;
        }
        //COMMON
        if (rnd > 0)
        {
            return GetCommonOrAbove(lockedTalents);
        }

        return null;
    }

    private Talent GetLegendaryTalent(KeyValuePair<int, Talent>[] lockedTalents)
    {
        var legendary = lockedTalents.Where(x => x.Value.TalentRarity == Rarity.Legendary);
        if (legendary.Count() <= 0)
            return null;
        
        return legendary.PickRandom().Value;
    }

    private Talent GetMythicTalent(KeyValuePair<int, Talent>[] lockedTalents)
    {
        var mythic = lockedTalents.Where(x => x.Value.TalentRarity == Rarity.Mythic);
        if (mythic.Count() <= 0)
            return null;

        return mythic.PickRandom().Value;
    }

    private Talent GetEpicTalent(KeyValuePair<int, Talent>[] lockedTalents)
    {
        var Epic = lockedTalents.Where(x => x.Value.TalentRarity == Rarity.Epic);
        if (Epic.Count() <= 0)
            return null;

        return Epic.PickRandom().Value;
    }

    private Talent GetRareTalent(KeyValuePair<int, Talent>[] lockedTalents)
    {
        var Rare = lockedTalents.Where(x => x.Value.TalentRarity == Rarity.Rare);
        if (Rare.Count() <= 0)
            return null;

        return Rare.PickRandom().Value;
    }

    private Talent GetUncommonTalent(KeyValuePair<int, Talent>[] lockedTalents)
    {
        var Uncommon = lockedTalents.Where(x => x.Value.TalentRarity == Rarity.Uncommon);
        if (Uncommon.Count() <= 0)
            return null;

        return Uncommon.PickRandom().Value;
    }

    private Talent GetCommonTalent(KeyValuePair<int, Talent>[] lockedTalents)
    {
        var Common = lockedTalents.Where(x => x.Value.TalentRarity == Rarity.Common);
        if (Common.Count() <= 0)
            return null;

        return Common.PickRandom().Value;
    }

    private Talent GetCommonOrAbove(KeyValuePair<int, Talent>[] lockedTalents)
    {
        Talent tal = GetCommonTalent(lockedTalents);
        if (tal == null)
        {
            tal = GetUncommonTalent(lockedTalents);
            if (tal == null)
            {
                tal = GetRareTalent(lockedTalents);
                if (tal == null)
                {
                    tal = GetEpicTalent(lockedTalents);
                    if (tal == null)
                    {
                        tal = GetMythicTalent(lockedTalents);
                        if (tal == null)
                        {
                            return GetLegendaryTalent(lockedTalents);
                        }
                    }
                }
            }
        }

        return tal;
    }




    private void RenameGameObjects()
    {
        var tals = GetComponentsInChildren<Talent>();
        for (int i = 0; i < tals.Length; i++)
        {
            tals[i].gameObject.name = TalentsDatabase.PropsData.PropsDictionary[tals[i].Name].Name + "_" + tals[i].transform.GetSiblingIndex().ToString();

            tals[i].SetAttributes(TalentsDatabase.PropsData.PropsDictionary[tals[i].Name].Name, TalentsDatabase.PropsData.PropsDictionary[tals[i].Name].Description, TalentsDatabase.PropsData.PropsDictionary[tals[i].Name].TalentRarity);

            tals[i].transform.localScale = tals[i].TalentRarity switch
            {
                Rarity.Common => new Vector3(0.7f, 0.7f, 0.7f),
                Rarity.Uncommon => new Vector3(1f, 1f, 1f),
                Rarity.Rare => new Vector3(1.2f, 1.2f, 1.2f),
                Rarity.Epic => new Vector3(1.5f, 1.5f, 1.5f),
                Rarity.Mythic => new Vector3(1.7f, 1.7f, 1.7f),
                Rarity.Legendary => new Vector3(2f, 2f, 2f),
                _ => new Vector3(1f, 1f, 1f)
            };
        }

        var height = transform.GetComponent<RectTransform>().rect.height;
        var width = transform.GetComponent<RectTransform>().rect.width;
        int indx = 0;
        for (float i = -width / 2 + 50f; i < width / 2 - 50f; i += 75f)
        {
            for (float j = height / 2 - 50f; j > -height / 2 + 50f; j -= 75f)
            {
                tals[indx].GetComponent<RectTransform>().localPosition = new Vector2(i, j);

                indx++;
                if (indx >= tals.Length)
                {
                    return;
                }
            }
        }
    }
}
