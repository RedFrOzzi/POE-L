using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI_Element : MonoBehaviour
{
    private Inventory inventory;
    private CharacterController2D controller;
    private CH_Stats playerStats;
    private bool state;

    [SerializeField] private GameObject inventoryParentGameObject;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject itemUIPrefab;
    [SerializeField] private GameObject verticalItemsGroup;
    [SerializeField] private TextMeshProUGUI goldText;
    
    [SerializeField]private GameObject[] inventoryItemSlots;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        inventory = player.GetComponent<Inventory>();
        controller = player.GetComponent<CharacterController2D>();
        playerStats = player.GetComponent<CH_Stats>();

        PrepareItemUISlots();

        inventory.OnInventoryChange += OnInventoryChange;
        controller.OpenInventory += OnOpenInventory;
        playerStats.OnGoldChange += OnGoldChange;
    }
    private void OnDestroy()
    {
        inventory.OnInventoryChange -= OnInventoryChange;
        controller.OpenInventory -= OnOpenInventory;
        playerStats.OnGoldChange -= OnGoldChange;
    }

    private void OnInventoryChange()
    {
        for (int i = 0; i < inventoryItemSlots.Length; i++)  //Item item in inventory.InventoryList)
        {
            if (i >= inventory.InventoryList.Count)
            {
                Item_UI_Element item_UI_Element = inventoryItemSlots[i].GetComponent<Item_UI_Element>();

                item_UI_Element.SetUpFieldsToNull();

                inventoryItemSlots[i].GetComponent<Image>().sprite = null;
                inventoryItemSlots[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                Item_UI_Element item_UI_Element = inventoryItemSlots[i].GetComponent<Item_UI_Element>();

                item_UI_Element.SetUpItemUIElement(inventory.InventoryList[i].Sprite, inventory.InventoryList[i], inventory);

                inventoryItemSlots[i].GetComponent<Image>().sprite = inventory.InventoryList[i].Sprite;
                
                if (inventory.InventoryList[i].TryGetComponent(out SpriteRenderer sr))
                {
                    inventoryItemSlots[i].GetComponent<Image>().color = sr.color;
                }
            }
        }
    }

    public void OnGoldChange(int currentGold)
    {
        goldText.text = currentGold.ToString();
    }

    private void OnOpenInventory()
    {
        inventoryParentGameObject.SetActive(!state);
        state = !state;
    }

    private void PrepareItemUISlots()
    {
        int count = 0;

        for (int i = 0; i < verticalItemsGroup.transform.childCount; i++)
        {
            for (int j = 0; j < verticalItemsGroup.transform.GetChild(i).transform.childCount; j++)
            {
                count++;
            }
        }

        inventoryItemSlots = new GameObject[count];

        count = 0;

        for (int i = 0; i < verticalItemsGroup.transform.childCount; i++)
        {
            for (int j = 0; j < verticalItemsGroup.transform.GetChild(i).transform.childCount; j++)
            {
                inventoryItemSlots[count] = verticalItemsGroup.transform.GetChild(i).transform.GetChild(j).gameObject;
                count++;
            }
        }
    }
}
