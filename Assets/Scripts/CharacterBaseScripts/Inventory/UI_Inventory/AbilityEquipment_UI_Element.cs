using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityEquipment_UI_Element : MonoBehaviour
{
    [SerializeField] private GameObject ability01EquipmentGO;
    [SerializeField] private GameObject ability02EquipmentGO;
    [SerializeField] private GameObject ability03EquipmentGO;
    [SerializeField] private GameObject ability04EquipmentGO;
    [SerializeField] private GameObject ability05EquipmentGO;
    [SerializeField] private GameObject ability06EquipmentGO;

    private GameObject[] abilitiesEquipmentParents;
    private bool[] shouldShowSlotsGems;

    private AbilityGem_UI_Element[] ability01GemsArray;
    private AbilityGem_UI_Element[] ability02GemsArray;
    private AbilityGem_UI_Element[] ability03GemsArray;
    private AbilityGem_UI_Element[] ability04GemsArray;
    private AbilityGem_UI_Element[] ability05GemsArray;
    private AbilityGem_UI_Element[] ability06GemsArray;

    private AbilityGem_UI_Element[][] abilityGemUIElements;

    private GameObject player;
    private CH_Stats stats;
    private CH_AbilitiesEquipment abilitiesEquipment;
    private CH_AbilitiesManager abilitiesManager;
    private AbilitySlot[] abilitySlots;
    private Inventory inventory;
    private CharacterController2D characterController;

    private AbilityGem[][] gemsArrays;

    private AbilityGem pointerGem;
    private byte pointerGemAbilityNum;
    private byte pointerGemSlotNum;

    private bool equipmentIsClosed;

    private void Awake()
    {
        ability01GemsArray = ability01EquipmentGO.GetComponentsInChildren<AbilityGem_UI_Element>();
        ability02GemsArray = ability02EquipmentGO.GetComponentsInChildren<AbilityGem_UI_Element>();
        ability03GemsArray = ability03EquipmentGO.GetComponentsInChildren<AbilityGem_UI_Element>();
        ability04GemsArray = ability04EquipmentGO.GetComponentsInChildren<AbilityGem_UI_Element>();
        ability05GemsArray = ability05EquipmentGO.GetComponentsInChildren<AbilityGem_UI_Element>();
        ability06GemsArray = ability06EquipmentGO.GetComponentsInChildren<AbilityGem_UI_Element>();

        abilitiesEquipmentParents = new GameObject[6];
        abilitiesEquipmentParents[0] = ability01EquipmentGO;
        abilitiesEquipmentParents[1] = ability02EquipmentGO;
        abilitiesEquipmentParents[2] = ability03EquipmentGO;
        abilitiesEquipmentParents[3] = ability04EquipmentGO;
        abilitiesEquipmentParents[4] = ability05EquipmentGO;
        abilitiesEquipmentParents[5] = ability06EquipmentGO;
        shouldShowSlotsGems = new bool[6];

        abilityGemUIElements = new AbilityGem_UI_Element[6][] { ability01GemsArray, ability02GemsArray,
        ability03GemsArray,ability04GemsArray,ability05GemsArray,ability06GemsArray};

        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<CH_Stats>();
        characterController = player.GetComponent<CharacterController2D>();
        abilitiesEquipment = player.GetComponent<CH_AbilitiesEquipment>();
        abilitiesManager = player.GetComponent<CH_AbilitiesManager>();
        abilitySlots = abilitiesManager.GetAbilitySlotRefs();
        inventory = player.GetComponent<Inventory>();
        gemsArrays = abilitiesEquipment.AbilitiesEquipment;
    }

    private void Start()
    {
        SetupEquipmentSlots();

        ability01EquipmentGO.SetActive(false);
        ability02EquipmentGO.SetActive(false);
        ability03EquipmentGO.SetActive(false);
        ability04EquipmentGO.SetActive(false);
        ability05EquipmentGO.SetActive(false);
        ability06EquipmentGO.SetActive(false);

        characterController.OpenInventory += OnOpenInventory;
        characterController.MouseDownRaycastTargets += StickGemToPointer;
        characterController.MouseUpRaycastTargets += ReleaseGemFromPointer;
        abilitiesEquipment.OnAbilityGemChange += OnAbilityGemChange;
        abilitiesManager.OnAbilityChange += SetAbilitySlotsActiveIfNeeded;
    }

    private void OnDestroy()
    {
        characterController.OpenInventory -= OnOpenInventory;
        characterController.MouseDownRaycastTargets -= StickGemToPointer;
        characterController.MouseUpRaycastTargets -= ReleaseGemFromPointer;
        abilitiesEquipment.OnAbilityGemChange -= OnAbilityGemChange;
        abilitiesManager.OnAbilityChange -= SetAbilitySlotsActiveIfNeeded;
    }

    private void OnOpenInventory()
    {
        if (equipmentIsClosed)
        {
            foreach (var go in abilitiesEquipmentParents)
            {
                go.SetActive(false);
            }
        }
        else
        {
            for (byte i = 0; i < abilitiesEquipmentParents.Length; i++)
            {
                abilitiesEquipmentParents[i].SetActive(shouldShowSlotsGems[i]);
            }
        }

        equipmentIsClosed = !equipmentIsClosed;
    }

    private void OnAbilityGemChange(byte abilityNum, byte slotNum, AbilityGem abilityGem)
    {
        if (abilityGem == null)
        {
            abilityGemUIElements[abilityNum][slotNum].ClearAbilityGem();
            return;
        }

        abilityGemUIElements[abilityNum][slotNum].SetAbilityGem(abilityGem);
    }

    private void SetupEquipmentSlots()
    {
        //сетап для расстановки EquipmentSlot - ов
        int equipmentSlotIndex = 0;

        for (byte i = 0; i < abilityGemUIElements.Length; i++)
        {
            for (byte j = 0; j < abilityGemUIElements[i].Length; j++)
            {
                if (j == abilityGemUIElements[i].Length - 1)
                    abilityGemUIElements[i][j].SetUpAbilityGemUI(EquipmentSlot.SuperAbilityGem, abilitiesEquipment, inventory, i, j);
                else
                    abilityGemUIElements[i][j].SetUpAbilityGemUI(EquipmentSlot.AbilityGem, abilitiesEquipment, inventory, i, j);
            }

            equipmentSlotIndex += abilityGemUIElements[i].Length; //колличество слотов в экипировке абилок
        }
    }

    private void StickGemToPointer(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.TryGetComponent<AbilityGem_UI_Element>(out AbilityGem_UI_Element abilityGem_UI_Element))
            {
                if (abilityGem_UI_Element.AbilityGem != null)
                {
                    pointerGem = abilityGem_UI_Element.AbilityGem;
                    pointerGemAbilityNum = abilityGem_UI_Element.AbilityNumber;
                    pointerGemSlotNum = abilityGem_UI_Element.SlotNumber;
                }
            }
        }
    }
    private void ReleaseGemFromPointer(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (pointerGem != null && raycastResult.gameObject.TryGetComponent<AbilityGem_UI_Element>(out AbilityGem_UI_Element abilityGem_UI))
            {
                if (pointerGem.EquipmentSlot != abilityGem_UI.EquipmentSlot)
                {
                    Debug.Log("Gem has different type");
                    pointerGem = null;
                    pointerGemAbilityNum = 0;
                    pointerGemSlotNum = 0;
                    return;
                }

                abilitiesEquipment.UnEquipGem(pointerGemAbilityNum, pointerGemSlotNum);
                abilitiesEquipment.EquipGem(abilityGem_UI.AbilityNumber, abilityGem_UI.SlotNumber, pointerGem);

                pointerGem = null;
                pointerGemAbilityNum = 0;
                pointerGemSlotNum = 0;
            }
            else if (raycastResult.gameObject.TryGetComponent<Item_UI_Element>(out _))
            {
                if (pointerGem != null)
                {
                    abilitiesEquipment.UnEquipGem(pointerGemAbilityNum, pointerGemSlotNum);

                    pointerGem = null;
                    pointerGemAbilityNum = 0;
                    pointerGemSlotNum = 0;
                }
            }
        }
    }

    private void SetAbilitySlotsActiveIfNeeded(byte slotIndex)
    {
        if (abilitySlots[slotIndex].Ability.ID != Database.AbilityID.None && abilitySlots[slotIndex].Ability.ID != Database.AbilityID.Void)
        {
            shouldShowSlotsGems[slotIndex] = true;
        }

        if (abilitySlots[slotIndex].Ability.ID == Database.AbilityID.None || abilitySlots[slotIndex].Ability.ID == Database.AbilityID.Void)
        {
            shouldShowSlotsGems[slotIndex] = false;
        }
    }
}
