using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{

    public List<Item> InventoryList = new();

    public event Action OnInventoryChange;

    [HideInInspector] public Equipment equipment;
    [HideInInspector] public CH_Stats stats;
    [HideInInspector] public CH_Health health;

    private CharacterController2D characterController;
    private CH_AbilitiesEquipment abilityEquipment;

    public Item PointerItem { get; private set; }

    
    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        stats = GetComponent<CH_Stats>();
        characterController = GetComponent<CharacterController2D>();
        abilityEquipment = GetComponent<CH_AbilitiesEquipment>();
    }

    private void Start()
    {
        characterController.MouseDownRaycastTargets += StickItemToPointer;
        characterController.MouseUpRaycastTargets += ReleaseItemFromPointer;
    }

    private void OnDestroy()
    {
        characterController.MouseDownRaycastTargets -= StickItemToPointer;
        characterController.MouseUpRaycastTargets -= ReleaseItemFromPointer;
    }


    public bool AddItem(Item item)
    {
        if (item != null)
        {
            InventoryList.Add(item);

            OnInventoryChange?.Invoke();

            return true;
        }
        else
        {
            return false;
        }        
    }

    public bool RemoveItem(Item item)
    {
        if (item != null && InventoryList.Contains(item))
        {
            InventoryList.Remove(item);            

            OnInventoryChange?.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }

    public void EquipItem(Item item)
    {
        if (item is EquipmentItem equipmentItem)
        {
            if (equipment.EqupipmentList[equipmentItem.EquipmentSlot] == null)
            {
                equipment.EquipItem(equipmentItem);
            }
            else
            {
                equipment.SwapItemFromInventory(equipmentItem);
            }
        }       
    }

    public void UseItem(Item item)
    {
        if (item is ConsumableItem consumable)
        {
            consumable.UseItem(this);

            RemoveItem(item);
        }
    }

    public void StickItemToPointer(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.TryGetComponent<Item_UI_Element>(out Item_UI_Element item_UI_Element))
            {
                if (item_UI_Element.Item != null)
                {
                    PointerItem = item_UI_Element.Item;
                }
            }
        }
    }

    public void ReleaseItemFromPointer(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult raycastResult in raycastResults)
        {
            if (raycastResult.gameObject.TryGetComponent<Equipment_Item_UI_Element>(out Equipment_Item_UI_Element equipment_Item_UI))
            {
                if (PointerItem != null)
                {
                    EquipItem(PointerItem);

                    PointerItem = null;
                }
            }
            else if (raycastResult.gameObject.TryGetComponent<AbilityGem_UI_Element>(out AbilityGem_UI_Element abilityGem_UI))
            {
                if (PointerItem != null && PointerItem is AbilityGem abilityGem)
                {
                    if (abilityGem.EquipmentSlot != abilityGem_UI.EquipmentSlot)
                    {
                        Debug.Log("Gem has different type");
                        PointerItem = null;
                        return;
                    }

                    abilityEquipment.EquipGem(abilityGem_UI.AbilityNumber, abilityGem_UI.SlotNumber, abilityGem);

                    PointerItem = null;
                }
            }
        }
    }
}
