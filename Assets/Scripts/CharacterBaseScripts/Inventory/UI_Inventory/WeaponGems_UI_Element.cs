using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponGems_UI_Element : MonoBehaviour
{
    [SerializeField] private GameObject Weapon01EquipmentGO;
    [SerializeField] private GameObject Weapon02EquipmentGO;

    private GameObject[] weaponEquipmentParent;
    private bool[] shouldShowSlotsGems;

    private W_Gem_UI_Element[] weapon01GemsArray;
    private W_Gem_UI_Element[] weapon02GemsArray;

    private W_Gem_UI_Element[][] weaponGemUIElements;

    private GameObject player;
    private CharacterController2D characterController;
    private Equipment equipment;

    private WeaponGem pointerGem;
    private byte pointerGemWeaponNum;
    private byte pointerGemSlotNum;
    private bool isGrabedFromInventory;

    private bool equipmentIsClosed;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterController = player.GetComponent<CharacterController2D>();
        equipment = player.GetComponent<Equipment>();

        weapon01GemsArray = Weapon01EquipmentGO.GetComponentsInChildren<W_Gem_UI_Element>();
        weapon02GemsArray = Weapon02EquipmentGO.GetComponentsInChildren<W_Gem_UI_Element>();

        weaponEquipmentParent = new GameObject[2];
        weaponEquipmentParent[0] = Weapon01EquipmentGO;
        weaponEquipmentParent[1] = Weapon02EquipmentGO;

        weaponGemUIElements = new W_Gem_UI_Element[2][] { weapon01GemsArray, weapon02GemsArray };

        shouldShowSlotsGems = new bool[2];
    }

    private void Start()
    {
        SetupGemSlots();

        Weapon01EquipmentGO.SetActive(false);
        Weapon01EquipmentGO.SetActive(false);

        characterController.OpenInventory += OnOpenInventory;
        characterController.MouseDownRaycastTargets += StickGemToPointer;
        characterController.MouseUpRaycastTargets += ReleaseGemFromPointer;
        equipment.OnWeaponGemChange += OnAbilityGemChange;
        equipment.OnEquipmentChange += SetWeaponSlotsActiveIfNeeded;
    }

    private void OnDestroy()
    {
        characterController.OpenInventory -= OnOpenInventory;
        characterController.MouseDownRaycastTargets -= StickGemToPointer;
        characterController.MouseUpRaycastTargets -= ReleaseGemFromPointer;
        equipment.OnWeaponGemChange -= OnAbilityGemChange;
        equipment.OnEquipmentChange -= SetWeaponSlotsActiveIfNeeded;
    }

    private void SetupGemSlots()
    {
        for (byte i = 0; i < weaponGemUIElements.Length; i++)
        {
            for (byte j = 0; j < weaponGemUIElements[i].Length; j++)
            {
                weaponGemUIElements[i][j].SetUpWeaponGemUI(i, j);
            }
        }
    }

    private void OnOpenInventory()
    {
        if (equipmentIsClosed)
        {
            foreach (var go in weaponEquipmentParent)
            {
                go.SetActive(false);
            }
        }
        else
        {
            for (byte i = 0; i < weaponEquipmentParent.Length; i++)
            {
                weaponEquipmentParent[i].SetActive(shouldShowSlotsGems[i]);
            }
        }

        equipmentIsClosed = !equipmentIsClosed;
    }

    private void StickGemToPointer(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.TryGetComponent(out W_Gem_UI_Element weaponGem_UI_Element))
            {
                if (weaponGem_UI_Element.WeaponGem != null)
                {
                    pointerGem = weaponGem_UI_Element.WeaponGem;
                    pointerGemWeaponNum = weaponGem_UI_Element.WeaponNumber;
                    pointerGemSlotNum = weaponGem_UI_Element.SlotNumber;
                    isGrabedFromInventory = false;
                }
            }
            else if (raycastResult.gameObject.TryGetComponent(out Item_UI_Element weaponGem_UI))
            {
                if (weaponGem_UI.Item is WeaponGem weaponGem)
                {
                    pointerGem = weaponGem;
                    isGrabedFromInventory = true;
                }
            }
        }
    }

    private void ReleaseGemFromPointer(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (pointerGem != null && raycastResult.gameObject.TryGetComponent(out W_Gem_UI_Element weaponGem_UI))
            {
                if (isGrabedFromInventory)
                {
                    equipment.EquipGem(weaponGem_UI.WeaponNumber, weaponGem_UI.SlotNumber, pointerGem);

                    pointerGem = null;
                    pointerGemWeaponNum = 0;
                    pointerGemSlotNum = 0;
                }
                else
                {
                    equipment.UnEquipGem(pointerGemWeaponNum, pointerGemSlotNum);
                    equipment.EquipGem(weaponGem_UI.WeaponNumber, weaponGem_UI.SlotNumber, pointerGem);

                    pointerGem = null;
                    pointerGemWeaponNum = 0;
                    pointerGemSlotNum = 0;
                }
            }
            else if (raycastResult.gameObject.TryGetComponent<Item_UI_Element>(out _))
            {
                if (pointerGem != null)
                {
                    equipment.UnEquipGem(pointerGemWeaponNum, pointerGemSlotNum);

                    pointerGem = null;
                    pointerGemWeaponNum = 0;
                    pointerGemSlotNum = 0;
                }
            }
        }
    }

    private void OnAbilityGemChange(byte weaponNum, byte slotNum, WeaponGem weaponGem)
    {
        if (weaponGem == null)
        {
            weaponGemUIElements[weaponNum][slotNum].ClearWeaponGem();
            return;
        }

        weaponGemUIElements[weaponNum][slotNum].SetWeaponGem(weaponGem);
    }

    private void SetWeaponSlotsActiveIfNeeded(EquipmentItem w1, EquipmentItem w2)
    {
        if (equipment.EqupipmentList[EquipmentSlot.Weapon] == null)
        {
            shouldShowSlotsGems[0] = false;
        }
        else
        {
            shouldShowSlotsGems[0] = true;
        }

        if (equipment.SecondEquipmentSet[EquipmentSlot.Weapon] == null)
        {
            shouldShowSlotsGems[1] = false;
        }
        else
        {
            shouldShowSlotsGems[1] = true;
        }

        for (byte i = 0; i < weaponEquipmentParent.Length; i++)
        {
            weaponEquipmentParent[i].SetActive(shouldShowSlotsGems[i]);
        }
    }
}
