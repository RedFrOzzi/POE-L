using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;

public class AbilityGem_UI_Element : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbilityGem AbilityGem { get; private set; }

    private Image gemImage;
    private Image gemBackgroundImage;

    private CH_AbilitiesEquipment abilitiesEquipment;
    private Inventory inventory;

    public EquipmentSlot EquipmentSlot;
    public byte AbilityNumber { get; private set; }
    public byte SlotNumber { get; private set; }

    private void Awake()
    {
        gemBackgroundImage = GetComponentsInChildren<Image>()[0];
        gemImage = GetComponentsInChildren<Image>()[1];
    }

    public void SetUpAbilityGemUI(EquipmentSlot equipmentSlot, CH_AbilitiesEquipment abilitiesEquipment, Inventory inventory, byte abilityNum, byte slotNum)
    {
        this.EquipmentSlot = equipmentSlot;
        this.abilitiesEquipment = abilitiesEquipment;
        this.inventory = inventory;
        AbilityNumber = abilityNum;
        SlotNumber = slotNum;
    }

    public void SetAbilityGem(AbilityGem abilityGem)
    {
        AbilityGem = abilityGem;
        gemImage.sprite = abilityGem.Sprite;
    }

    public void ClearAbilityGem()
    {
        AbilityGem = null;
        gemImage.sprite = null;
    }    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AbilityGem == null) { return; }

        var strBuilder = new StringBuilder();

        foreach (var pref in AbilityGem.ModsHolder.Prefixes)
        {
            strBuilder.Append(pref.Description(pref) + "\n");
        }

        foreach (var suf in AbilityGem.ModsHolder.Suffixes)
        {
            strBuilder.Append(suf.Description(suf) + "\n");
        }

        if (AbilityGem.Description != null && AbilityGem.Description != string.Empty)
            strBuilder.Append($"Description: {AbilityGem.Description}");

        Tooltip.Show($"{AbilityGem.Name}", strBuilder.ToString(), 16, 12);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Hide();
    }
}
