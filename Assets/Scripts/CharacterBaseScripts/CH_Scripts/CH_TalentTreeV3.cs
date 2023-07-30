//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using Database;
//using System.Linq;

//public class CH_TalentTreeV3 : MonoBehaviour
//{
//    public Dictionary<int, Talent> Talents { get; private set; } = new();
//    public Dictionary<int, Talent> LockedTalents { get; private set; } = new();

//    public int AvailableTalentPoints { get; private set; }
//    public int TotalSpentTalentPoints { get; private set; }

//    private CH_Stats stats;

//    private int index = 0;

//    public event Action<bool, Talent> OnTalentInteraction;
//    public event Action OnGetTalentPoints;

//    private void Awake()
//    {
//        stats = GetComponent<CH_Stats>();
//    }

//    private void Start()
//    {
//        var gameObjects = GameObject.FindGameObjectsWithTag("Talent");
//        foreach (GameObject go in gameObjects)
//        {
//            var talent = go.GetComponent<Talent>();
//            talent.InitializeTalent(index, this);
            
//            LockedTalents.Add(index, talent);
//            Talents.Add(index, talent);            

//            if (talent.TalentID == TalentID.StartingPoint)
//            {
//                talent.SetStartingPointColor();
//                LockedTalents.Remove(index);
//            }

//            index++;
//        }

//        AddTalentPoints(100);

//        GameObject.FindGameObjectWithTag("TalentsUIParent").GetComponentInParent<Talents_UI_Element>().AfterTalentsSetUp(this);
//    }

//    public void AddTalentPoints(byte points)
//    {
//        AvailableTalentPoints += points;

//        OnGetTalentPoints?.Invoke();
//    }

//    public void TalentClick(int indexId, bool toUnlock, Talent talent)
//    {
//        if (toUnlock && AvailableTalentPoints <= 0) { return; }

//        //Логика открытия/закрытия в клвссе Talent
//        if (toUnlock)
//        {
//            Talents[indexId].TalentProperties.OnUnlock(stats);
//            LockedTalents.Remove(indexId);

//            AvailableTalentPoints--;
//            TotalSpentTalentPoints++;

//            OnTalentInteraction?.Invoke(toUnlock, talent);
//        }
//        else
//        {
//            Talents[indexId].TalentProperties.OnLock(stats);
//            LockedTalents.Add(indexId, talent);

//            TotalSpentTalentPoints--;
//            AvailableTalentPoints++;

//            OnTalentInteraction?.Invoke(toUnlock, talent);
//        }
//    }

//    public void UnlockTalentBySystem(int indexId)
//    {
//        Talent talent = Talents[indexId];

//        talent.UnlockBySystem();

//        Talents[indexId].TalentProperties.OnUnlock(stats);
//        LockedTalents.Remove(indexId);

//        OnTalentInteraction?.Invoke(true, talent);
//    }
//}
