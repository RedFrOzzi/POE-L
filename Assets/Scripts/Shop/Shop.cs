using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
	[SerializeField] private GameObject shopPanel;
    [SerializeField] private Shop_ItemType_UI_Element[] item_UI_Elemets;
    [SerializeField] private Transform parentForDropedItems;

    [SerializeField, Space(20)] private float weaponCost;
    [SerializeField] private float leftHandCost;
    [SerializeField] private float bodyArmorCost;
    [SerializeField] private float helmetCost;
    [SerializeField] private float glovesCost;
    [SerializeField] private float bootsCost;
    [SerializeField] private float abilityGemCost;
    [SerializeField] private float superAbilityGemCost;
    [SerializeField, Space(10)] private float onBuyGoldMultiplier = 1.2f;

    [SerializeField, Space(10)] private Sprite weaponSprite;
    [SerializeField] private Sprite leftHandSprite;
    [SerializeField] private Sprite bodyArmorSprite;
    [SerializeField] private Sprite helmetSprite;
    [SerializeField] private Sprite glovesSprite;
    [SerializeField] private Sprite bootsSprite;
    [SerializeField] private Sprite abilityGemSprite;
    [SerializeField] private Sprite superAbilityGemSprite;

    private ItemSpawner itemSpawner;
    private CH_Stats playerStats;

    private Dictionary<EquipmentSlot, Sprite> sprites;
    private Dictionary<EquipmentSlot, float> itemCosts;
    private Dictionary<EquipmentSlot, Shop_ItemType_UI_Element> item_Elements;

    private void Awake()
    {
        item_UI_Elemets = shopPanel.GetComponentsInChildren<Shop_ItemType_UI_Element>();

        SetUpSprites();
        SetUpPrices();

        shopPanel.SetActive(false);
    }

    private void Start()
    {
        itemSpawner = ItemSpawner.Instance;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Stats>();

        SetUpItemsToBuy();
    }

    public void OpenShop()
	{
        shopPanel.SetActive(true);
	}

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void TryBuyItem(EquipmentSlot slot)
    {
        if (itemCosts[slot] > playerStats.CurrentGold)
        {
            Debug.Log("Not enough gold");
            return;
        }

        itemSpawner.GetItem(slot, itemCosts[slot]);

        itemCosts[slot] = (int)(itemCosts[slot] * onBuyGoldMultiplier);

        item_Elements[slot].NewPrice(itemCosts[slot]);
    }

    private void SetUpItemsToBuy()
    {
        item_Elements = new();

        int index = 0;
        int maxValue = Enum.GetNames(typeof(EquipmentSlot)).Length;

        foreach (var el in item_UI_Elemets)
        {
            if (index >= maxValue) { break; }

            el.SetUp(this, (EquipmentSlot)index, sprites[(EquipmentSlot)index], itemCosts[(EquipmentSlot)index]);

            item_Elements.Add((EquipmentSlot)index, el);

            index++;
        }
    }

    private void SetUpSprites()
    {
        sprites = new()
        {
            { EquipmentSlot.Weapon, weaponSprite},
            { EquipmentSlot.LeftHand, leftHandSprite},
            { EquipmentSlot.BodyArmor, bodyArmorSprite},
            { EquipmentSlot.Helmet, helmetSprite},
            { EquipmentSlot.Gloves, glovesSprite},
            { EquipmentSlot.Boots, bootsSprite},
            { EquipmentSlot.AbilityGem, abilityGemSprite},
            { EquipmentSlot.SuperAbilityGem, superAbilityGemSprite}
        };
    }

    private void SetUpPrices()
    {
        itemCosts = new()
        {
            { EquipmentSlot.Weapon, weaponCost},
            { EquipmentSlot.LeftHand, leftHandCost},
            { EquipmentSlot.BodyArmor, bodyArmorCost},
            { EquipmentSlot.Helmet, helmetCost},
            { EquipmentSlot.Gloves, glovesCost},
            { EquipmentSlot.Boots, bootsCost},
            { EquipmentSlot.AbilityGem, abilityGemCost},
            { EquipmentSlot.SuperAbilityGem, superAbilityGemCost}
        };
    }
}
