using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Database;
using System.Linq;

public class CH_TalentTree : MonoBehaviour
{
    [ContextMenuItem("Rename", "Show"), Space(10)]
    public string Rename = "Show";

    public Dictionary<int, Talent> Talents { get; private set; } = new();
    public Dictionary<int, Talent> LockedTalents { get; private set; } = new();

    private readonly Dictionary<TalentGroup, bool> parentGroupUnlockMethods = new();
    private readonly Dictionary<TalentGroup, int> parentGroupTalentPointsDifferenceToUnlock = new();
    private readonly Dictionary<TalentGroup, int> parentGroupTalentPointsDifferenceMultiToUnlock = new();
    private readonly Dictionary<TalentGroup, int> unlockedTalentsInGroup = new();

    public int AvailableTalentPoints { get; private set; }
    public int TotalUnlockedTalents { get; private set; }

    private CH_Stats stats;

    private int index = 0;

    public event Action<bool, Talent> OnTalentInteraction;
    public event Action OnGetTalentPoints;

    private GameFlowManager gameFlowManager;

    private void Awake()
    {
        stats = GetComponent<CH_Stats>();
    }

    private void Start()
    {
        PopulateTalentLists();

        PopulateTalentGroupsDictionaries();

        AddTalentPoints(100);

        GameObject.FindGameObjectWithTag("TalentsUIParent").GetComponentInParent<Talents_UI_Element>().AfterTalentsSetUp(this);

        gameFlowManager = GameFlowManager.Instance;
    }

    public void AddTalentPoints(byte points)
    {
        AvailableTalentPoints += points;

        OnGetTalentPoints?.Invoke();
    }

    public bool TryTurnTalent(TalentGroup talentGroup, int indexID, bool toUnlock)
    {
        if (toUnlock)
        {
            return UnlockTalent(talentGroup, indexID);
        }
        else
        {
            return LockTalent(talentGroup, indexID);
        }
    }

    private void TurnTalent(TalentGroup talentGroup, int indexID, bool toUnlock)
    {
        if (gameFlowManager.IsPlayerAllowedToTakeTalents == false)
        {
            Debug.Log("Can not take talents while talent cards is open");
            return;
        }

        if (toUnlock && AvailableTalentPoints <= 0)
        {
            Debug.Log("No talent points available");
            return;
        }

        //Логика открытия/закрытия в клвссе Talent
        if (toUnlock)
        {
            Talents[indexID].UnlockTalent();
            Talents[indexID].TalentProperties.OnUnlock(stats);
            LockedTalents.Remove(indexID);

            AvailableTalentPoints--;
            TotalUnlockedTalents++;

            unlockedTalentsInGroup[talentGroup]++;

            OnTalentInteraction?.Invoke(toUnlock, Talents[indexID]);
        }
        else
        {
            Talents[indexID].LockTalent();
            Talents[indexID].TalentProperties.OnLock(stats);
            LockedTalents.Add(indexID, Talents[indexID]);

            TotalUnlockedTalents--;
            AvailableTalentPoints++;

            unlockedTalentsInGroup[talentGroup]--;

            OnTalentInteraction?.Invoke(toUnlock, Talents[indexID]);
        }
    }

    public void UnlockTalentByCard(int indexID, Talent talent)
    {
        Talents[indexID].UnlockTalent();
        Talents[indexID].TalentProperties.OnUnlock(stats);
        LockedTalents.Remove(indexID);

        TotalUnlockedTalents++;

        unlockedTalentsInGroup[talent.TalentGroup]++;

        OnTalentInteraction?.Invoke(true, Talents[indexID]);
    }


    private void PopulateTalentLists()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Talent");
        foreach (GameObject go in gameObjects)
        {
            var talent = go.GetComponent<Talent>();
            talent.InitializeTalent(index);

            LockedTalents.Add(index, talent);
            Talents.Add(index, talent);

            index++;
        }
    }

    private void PopulateTalentGroupsDictionaries()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("TalentGroup");
        foreach (var go in gameObjects)
        {
            var talentGroup = go.GetComponent<TalentGroup>();

            parentGroupTalentPointsDifferenceToUnlock.Add(talentGroup, talentGroup.PointsDifference);
            parentGroupTalentPointsDifferenceMultiToUnlock.Add(talentGroup, talentGroup.PointsDifferenceMilti);
            parentGroupUnlockMethods.Add(talentGroup, talentGroup.useDifferenceMulti);

            unlockedTalentsInGroup.Add(talentGroup, 0);
        }
    }

    private bool UnlockTalent(TalentGroup talentGroup, int indexID)
    {
        if (talentGroup.ParentTalentGroup == null)
        {
            TurnTalent(talentGroup, indexID, true);

            return true;
        }
        else
        {
            if (parentGroupUnlockMethods[talentGroup])
            {
                if (unlockedTalentsInGroup[talentGroup] < unlockedTalentsInGroup[talentGroup.ParentTalentGroup] / parentGroupTalentPointsDifferenceMultiToUnlock[talentGroup])
                {
                    TurnTalent(talentGroup, indexID, true);

                    return true;
                }
            }
            else
            {
                if (unlockedTalentsInGroup[talentGroup] <= unlockedTalentsInGroup[talentGroup.ParentTalentGroup] - parentGroupTalentPointsDifferenceToUnlock[talentGroup])
                {
                    TurnTalent(talentGroup, indexID, true);

                    return true;
                }
            }
        }

        OnFailedUnlock();

        return false;
    }

    private bool LockTalent(TalentGroup talentGroup, int indexID)
    {
        if (true)
        {
            TurnTalent(talentGroup, indexID, false);

            return true;
        }

        //OnFailedUnlock();

        //return false;
    }

    private void OnFailedUnlock()
    {
        Debug.Log("Group is locked");
    }
}
