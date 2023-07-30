//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class Talents_UI_Element2 : MonoBehaviour
//{
//    private GameObject talentsCanvasParent;
//    private GameObject UI_Parent;
//    private CanvasScaler canvasScaler;
//    private CH_TalentTree playerTalentTree;

//    [SerializeField] private float scrollSpeed = 1;
//    [SerializeField] private Image availablePointIconGameObject;
//    [SerializeField] private TextMeshProUGUI textMesh;

//    private Dictionary<int, Talent> allTalents;

//    private readonly List<TalentsPair> talentsPairs = new();

//    private Transform LineRendererParent;
//    [SerializeField] private UILineRenderer UILineRendererPrefab;
//    private readonly Dictionary<TalentsPair, UILineRenderer> talentsPairsLines = new();

//    private bool IsOpen;

//    private void Awake()
//    {
//        talentsCanvasParent = GameObject.FindGameObjectWithTag("TalentsUIParent");
//        canvasScaler = talentsCanvasParent.GetComponent<CanvasScaler>();

//        UI_Parent = GameObject.FindGameObjectWithTag("UI_Parent");

//        LineRendererParent = GameObject.FindGameObjectWithTag("LineRendererParent").transform;

//        UILineRendererPrefab = Resources.Load<UILineRenderer>("Talents/UILineRendererPrefab");
//    }

//    public void AfterTalentsSetUp(CH_TalentTree tree)
//    {
//        playerTalentTree = tree;
//        playerTalentTree.OnGetTalentPoints += ShowAvailablePointsIconIfShould;

//        playerTalentTree.OnTalentInteraction += OnTalentClick;
//        GameFlowManager.Instance.OpenTalents += OpenTalents;

//        talentsCanvasParent.SetActive(false);

//        allTalents = tree.Talents;

//        AddTalentsPairs();

//        DrawPairLines();

//        ShowAvailablePointsIconIfShould();
//    }

//    private void OnDestroy()
//    {
//        GameFlowManager.Instance.OpenTalents -= OpenTalents;
//        playerTalentTree.OnTalentInteraction -= OnTalentClick;
//        playerTalentTree.OnGetTalentPoints -= ShowAvailablePointsIconIfShould;
//    }


//    private void Update()
//    {
//        if (IsOpen == false) { return; }

//        if(Input.mouseScrollDelta.y > Vector2.zero.y)
//        {
//            canvasScaler.scaleFactor += 0.1f * scrollSpeed;
//            canvasScaler.scaleFactor = Mathf.Clamp(canvasScaler.scaleFactor, 0.2f, 2f);
//        }

//        if (Input.mouseScrollDelta.y < Vector2.zero.y)
//        {
//            canvasScaler.scaleFactor -= 0.1f * scrollSpeed;
//            canvasScaler.scaleFactor = Mathf.Clamp(canvasScaler.scaleFactor, 0.2f, 2f);
//        }
//    }

//    private void OpenTalents(bool isOpen)
//    {
//        IsOpen = isOpen;
//        talentsCanvasParent.SetActive(IsOpen);
//        UI_Parent.SetActive(!IsOpen);
//    }

//    private void OnTalentClick(bool toUnlock, Talent talent)
//    {
//        //RedrawPairLines(toUnlock, talent);

//        if (toUnlock)
//        {
//            redraw(talent, Color.green);
//        }
//        else
//        {
//            redraw(talent, Color.red);
//        }

//        ShowAvailablePointsIconIfShould();
//    }

//    private void ShowAvailablePointsIconIfShould()
//    {
//        if (playerTalentTree.AvailableTalentPoints > 0)
//        {
//            textMesh.text = playerTalentTree.AvailableTalentPoints.ToString();
//            availablePointIconGameObject.gameObject.SetActive(true);
//        }
//        else
//            availablePointIconGameObject.gameObject.SetActive(false);
//    }

//    private void RedrawPairLines(bool toUnlock, Talent talent)
//    {
//        if (toUnlock)
//        {
//            foreach (var pair in GetActivePairsContainingTalent(talent))
//            {
//                RedrawLine(pair, Color.green);
//            }
//        }
//        else
//        {
//            foreach (var pair in GetNotActivePairsContainingTalent(talent))
//            {
//                RedrawLine(pair, Color.red);
//            }
//        }
//    }

//    private void redraw(Talent talent, Color color)
//    {
//        var talPairs = GetLinkedTalentsPairs(talent);

//        foreach (var pair in talPairs)
//        {
//            if (pair.Talent1 == talent && pair.Talent2 == pair.Talent1.ParentTalent)
//            {
//                if (pair.Talent1.IsUnlocked || pair.Talent1.IsUnlockedBySystem)
//                {
//                    RedrawLine(pair, Color.green);
//                }
//                else
//                {
//                    RedrawLine(pair, Color.red);
//                }
//            }

//            if (pair.Talent2 == talent && pair.Talent1 == pair.Talent2.ParentTalent)
//            {
//                if (pair.Talent2.IsUnlocked || pair.Talent2.IsUnlockedBySystem)
//                {
//                    RedrawLine(pair, Color.green);
//                }
//                else
//                {
//                    RedrawLine(pair, Color.red);
//                }
//            }
//        }

        
//    }


//    private bool CheckIfListGotTalentPair(Talent talent1, Talent talent2)
//    {
//        for (int i = 0; i < talentsPairs.Count; i++)
//        {
//            if ((talentsPairs[i].Talent1 == talent1 && talentsPairs[i].Talent2 == talent2) || (talentsPairs[i].Talent1 == talent2 && talentsPairs[i].Talent2 == talent1))
//            {
//                return true;
//            }
//        }

//        return false;
//    }

//    private TalentsPair[] GetActivePairsContainingTalent(Talent talent)
//    {
//        return talentsPairs.Where(x => (x.Talent1 == talent && x.Talent1.IsUnlocked && x.Talent2.IsUnlocked) || (x.Talent2 == talent && x.Talent2.IsUnlocked && x.Talent1.IsUnlocked))
//            .ToArray();
//    }

//    private TalentsPair[] GetNotActivePairsContainingTalent(Talent talent)
//    {
//        return talentsPairs.Where(x => (x.Talent1 == talent && ((x.Talent1.IsUnlocked && x.Talent2.IsUnlocked == false) || (x.Talent1.IsUnlocked == false && x.Talent2.IsUnlocked)))
//                || (x.Talent2 == talent && ((x.Talent1.IsUnlocked && x.Talent2.IsUnlocked == false) || (x.Talent1.IsUnlocked == false && x.Talent2.IsUnlocked))))
//            .ToArray();
//    }

//    private TalentsPair GetTalentParentPair(Talent talent)
//    {
//        return talentsPairs.Where(x => (x.Talent1 == talent && x.Talent2 == talent.ParentTalent) || (x.Talent2 == talent && x.Talent1 == talent.ParentTalent)).First();
//    }

//    private TalentsPair[] GetLinkedTalentsPairs(Talent talent)
//    {
//        return talentsPairs.Where(x => x.Talent1 == talent || x.Talent2 == talent).ToArray();
//    }

//    private void AddTalentsPairs()
//    {
//        foreach (var (_, talent) in allTalents)
//        {
//            foreach (var t in talent.GetList())
//            {
//                if (CheckIfListGotTalentPair(talent, t) == false)
//                {
//                    talentsPairs.Add(new(talent, t));
//                }
//            }
//        }
//    }

//    private void DrawPairLines()
//    {
//        foreach (var pair in talentsPairs)
//        {
//            var lr = Instantiate(UILineRendererPrefab, LineRendererParent);
//            talentsPairsLines.Add(pair, lr);

//            lr.ClearPoints();

//            lr.AddPoint(pair.Talent1.transform.position, Color.red);
//            lr.AddPoint(pair.Talent2.transform.position, Color.red);
//        }
//    }

//    private void RedrawLine(TalentsPair pair, Color color)
//    {
//        talentsPairsLines[pair].ClearPoints();
//        talentsPairsLines[pair].AddPoint(pair.Talent1.transform.position, color);
//        talentsPairsLines[pair].AddPoint(pair.Talent2.transform.position, color);
//        talentsPairsLines[pair].SetAllDirty();
//    }
//}
