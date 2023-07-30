using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_UI_Element : MonoBehaviour
{
    [SerializeField] private Equipment_Item_UI_Element itemUIPrefab;

    private Equipment equipment;

    private Image weaponSlot;
    private Equipment_Item_UI_Element UIweaponSlot;
    private Image leftHandSlot;
    private Equipment_Item_UI_Element UIleftHandSlot;
    private Image helmetSlot;
    private Equipment_Item_UI_Element UIhelmetSlot;
    private Image glovesSlot;
    private Equipment_Item_UI_Element UIglovesSlot;
    private Image bootsSlot;
    private Equipment_Item_UI_Element UIbootsSlot;
    private Image bodyArmorSlot;
    private Equipment_Item_UI_Element UIbodyArmorSlot;
    private Image weaponSlot2;
    private Equipment_Item_UI_Element UIweaponSlot2;
    private Image leftHandSlot2;
    private Equipment_Item_UI_Element UIleftHandSlot2;

    private void Awake()
    {
        equipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();

        //-----------------------------------------------------------------------------------------

        UIweaponSlot = Instantiate(itemUIPrefab, GameObject.Find("Weapon_Slot").transform);
        weaponSlot = UIweaponSlot.GetComponent<Image>();
        UIweaponSlot.EquipmentSlot = EquipmentSlot.Weapon;
        UIweaponSlot.Equipment = equipment;
        weaponSlot.color = Color.black;

        UIleftHandSlot = Instantiate(itemUIPrefab, GameObject.Find("Left_Hand_Slot").transform);
        leftHandSlot = UIleftHandSlot.GetComponent<Image>();
        UIleftHandSlot.EquipmentSlot = EquipmentSlot.LeftHand;
        UIleftHandSlot.Equipment = equipment;
        leftHandSlot.color = Color.black;

        UIbootsSlot = Instantiate(itemUIPrefab, GameObject.Find("Boots_Slot").transform);
        bootsSlot = UIbootsSlot.GetComponent<Image>();
        UIbootsSlot.EquipmentSlot = EquipmentSlot.Boots;
        UIbootsSlot.Equipment = equipment;
        bootsSlot.color = Color.black;

        UIglovesSlot = Instantiate(itemUIPrefab, GameObject.Find("Gloves_Slot").transform);
        glovesSlot = UIglovesSlot.GetComponent<Image>();
        UIglovesSlot.EquipmentSlot = EquipmentSlot.Gloves;
        UIglovesSlot.Equipment = equipment;
        glovesSlot.color = Color.black;

        UIhelmetSlot = Instantiate(itemUIPrefab, GameObject.Find("Helmet_Slot").transform);
        helmetSlot = UIhelmetSlot.GetComponent<Image>();
        UIhelmetSlot.EquipmentSlot = EquipmentSlot.Helmet;
        UIhelmetSlot.Equipment = equipment;
        helmetSlot.color = Color.black;

        UIbodyArmorSlot = Instantiate(itemUIPrefab, GameObject.Find("BodyArmor_Slot").transform);
        bodyArmorSlot = UIbodyArmorSlot.GetComponent<Image>();
        UIbodyArmorSlot.EquipmentSlot = EquipmentSlot.BodyArmor;
        UIbodyArmorSlot.Equipment = equipment;
        bodyArmorSlot.color = Color.black;

        UIweaponSlot2 = Instantiate(itemUIPrefab, GameObject.Find("Weapon_Slot_2").transform);
        weaponSlot2 = UIweaponSlot2.GetComponent<Image>();
        UIweaponSlot2.EquipmentSlot = EquipmentSlot.Weapon;
        UIweaponSlot2.Equipment = equipment;
        weaponSlot2.color = Color.black;

        UIleftHandSlot2 = Instantiate(itemUIPrefab, GameObject.Find("Left_Hand_Slot_2").transform);
        leftHandSlot2 = UIleftHandSlot2.GetComponent<Image>();
        UIleftHandSlot2.EquipmentSlot = EquipmentSlot.LeftHand;
        UIleftHandSlot2.Equipment = equipment;
        leftHandSlot2.color = Color.black;

        GameObject canvasGameObject = GameObject.Find("Player_Inventory_Parent_UI");
        canvasGameObject.SetActive(false);
    }

    private void Start()
    {
        equipment.OnEquipmentChange += OnEquipmentChange;
    }

    private void OnDestroy()
    {
        equipment.OnEquipmentChange -= OnEquipmentChange;
    }

    private void OnEquipmentChange(EquipmentItem newItem, EquipmentItem oldItem)
    {
        Item item = null;

        item = equipment.EqupipmentList[EquipmentSlot.Weapon];
        if (item != null)
        {
            weaponSlot.sprite = item.Sprite;
            weaponSlot.color = item.GetComponent<SpriteRenderer>().color;
            UIweaponSlot.Item = item;
        }
        else 
        {
            weaponSlot.sprite = null;
            weaponSlot.color = Color.black;
            UIweaponSlot.Item = null;
        }

        item = equipment.EqupipmentList[EquipmentSlot.LeftHand];
        if (item != null)
        {
            leftHandSlot.sprite = item.Sprite;
            leftHandSlot.color = item.GetComponent<SpriteRenderer>().color;
            UIleftHandSlot.Item = item;
        }
        else
        {
            leftHandSlot.sprite = null;
            leftHandSlot.color = Color.black;
            UIleftHandSlot.Item = null;
        }

        item = equipment.EqupipmentList[EquipmentSlot.Boots];
        if (item != null)
        {
            bootsSlot.sprite = item.Sprite;
            bootsSlot.color = item.GetComponent<SpriteRenderer>().color;
            UIbootsSlot.Item = item;
        }
        else 
        {
            bootsSlot.sprite = null;
            bootsSlot.color = Color.black;
            UIbootsSlot.Item = null;
        }

        item = equipment.EqupipmentList[EquipmentSlot.Gloves];
        if (item != null)
        {
            glovesSlot.sprite = item.Sprite;
            glovesSlot.color = item.GetComponent<SpriteRenderer>().color;
            UIglovesSlot.Item = item;
        }
        else
        {
            glovesSlot.sprite = null;
            glovesSlot.color = Color.black;
            UIglovesSlot.Item = null;
        }

        item = equipment.EqupipmentList[EquipmentSlot.Helmet];
        if (item != null)
        {
            helmetSlot.sprite = item.Sprite;
            helmetSlot.color = item.GetComponent<SpriteRenderer>().color;
            UIhelmetSlot.Item = item;
        }
        else
        {
            helmetSlot.sprite = null;
            helmetSlot.color = Color.black;
            UIhelmetSlot.Item = null;
        }

        item = equipment.EqupipmentList[EquipmentSlot.BodyArmor];
        if (item != null)
        {
            bodyArmorSlot.sprite = item.Sprite;
            bodyArmorSlot.color = item.GetComponent<SpriteRenderer>().color;
            UIbodyArmorSlot.Item = item;
        }
        else
        {
            bodyArmorSlot.sprite = null;
            bodyArmorSlot.color = Color.black;
            UIbodyArmorSlot.Item = null;
        }

        item = equipment.SecondEquipmentSet[EquipmentSlot.Weapon];
        if (item != null)
        {
            weaponSlot2.sprite = item.Sprite;
            weaponSlot2.color = item.GetComponent<SpriteRenderer>().color;
            UIweaponSlot2.Item = item;
        }
        else
        {
            weaponSlot2.sprite = null;
            weaponSlot2.color = Color.black;
            UIweaponSlot2.Item = null;
        }

        item = equipment.SecondEquipmentSet[EquipmentSlot.LeftHand];
        if (item != null)
        {
            leftHandSlot2.sprite = item.Sprite;
            leftHandSlot2.color = item.GetComponent<SpriteRenderer>().color;
            UIleftHandSlot2.Item = item;
        }
        else
        {
            leftHandSlot2.sprite = null;
            leftHandSlot2.color = Color.black;
            UIleftHandSlot2.Item = null;
        }
    }
}
