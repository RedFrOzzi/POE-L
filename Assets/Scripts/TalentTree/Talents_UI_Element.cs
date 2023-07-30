using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Talents_UI_Element : MonoBehaviour
{
    private GameObject talentsCanvasParent;
    private GameObject UI_Parent;
    private CanvasScaler canvasScaler;
    private CH_TalentTree playerTalentTree;

    [SerializeField] private float scrollSpeed = 1;
    [SerializeField] private Image availablePointIconGameObject;
    [SerializeField] private TextMeshProUGUI textMeshUvailablePoints;

    private bool IsOpen;

    private void Awake()
    {
        talentsCanvasParent = GameObject.FindGameObjectWithTag("TalentsUIParent");
        canvasScaler = talentsCanvasParent.GetComponent<CanvasScaler>();

        UI_Parent = GameObject.FindGameObjectWithTag("UI_Parent");
    }

    public void AfterTalentsSetUp(CH_TalentTree tree)
    {
        playerTalentTree = tree;
        playerTalentTree.OnGetTalentPoints += ShowAvailablePointsIconIfShould;

        playerTalentTree.OnTalentInteraction += OnTalentInteraction;
        GameFlowManager.Instance.OpenTalents += OpenTalents;

        talentsCanvasParent.SetActive(false);

        ShowAvailablePointsIconIfShould();
    }

    private void OnDestroy()
    {
        GameFlowManager.Instance.OpenTalents -= OpenTalents;
        playerTalentTree.OnTalentInteraction -= OnTalentInteraction;
        playerTalentTree.OnGetTalentPoints -= ShowAvailablePointsIconIfShould;
    }


    private void Update()
    {
        if (IsOpen == false) { return; }

        if(Input.mouseScrollDelta.y > Vector2.zero.y)
        {
            canvasScaler.scaleFactor += 0.1f * scrollSpeed;
            canvasScaler.scaleFactor = Mathf.Clamp(canvasScaler.scaleFactor, 0.2f, 2f);
        }

        if (Input.mouseScrollDelta.y < Vector2.zero.y)
        {
            canvasScaler.scaleFactor -= 0.1f * scrollSpeed;
            canvasScaler.scaleFactor = Mathf.Clamp(canvasScaler.scaleFactor, 0.2f, 2f);
        }
    }

    private void OpenTalents(bool isOpen)
    {
        IsOpen = isOpen;
        talentsCanvasParent.SetActive(IsOpen);
        UI_Parent.SetActive(!IsOpen);
    }

    private void OnTalentInteraction(bool toUnlock, Talent talent)
    {
        ShowAvailablePointsIconIfShould();
    }

    private void ShowAvailablePointsIconIfShould()
    {
        if (playerTalentTree.AvailableTalentPoints > 0)
        {
            textMeshUvailablePoints.text = playerTalentTree.AvailableTalentPoints.ToString();
            availablePointIconGameObject.gameObject.SetActive(true);
        }
        else
            availablePointIconGameObject.gameObject.SetActive(false);
    }
}
