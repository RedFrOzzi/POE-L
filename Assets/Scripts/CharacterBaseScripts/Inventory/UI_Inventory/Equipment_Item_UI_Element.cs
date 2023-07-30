using Database;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class Equipment_Item_UI_Element : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Item Item { get; set; }
    [HideInInspector] public Equipment Equipment;
    [HideInInspector] public EquipmentSlot EquipmentSlot;

    private StringBuilder strBldr = new();
    private StringBuilder tagStrBldr = new();
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UnequipItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //on right click
        }
    }

    public void UnequipItem()
    {
        Equipment.UnequipItemFromSlot(EquipmentSlot);
    }    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item is Weapon weapon)
        {
            WeaponTooltip(weapon);
        }

        else if (Item is Armor armor)
        {
            ArmorTooltip(armor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        strBldr.Clear();

        Tooltip.Hide();
    }

    private void WeaponTooltip(Weapon weapon)
    {
        strBldr.Append($"Damage: {weapon.MinDamage} - {weapon.MaxDamage}\n" +
                $"Attack speed: {weapon.AttackSpeed}/s\n" +
                $"Crit chance: {weapon.CritChance}%\n" +
                $"Ammo: {weapon.AmmoCapacity}\n" +
                $"Accuracy: {weapon.Accuracy} / 1000\n" +
                $"Reload speed: {1 / weapon.ReloadSpeed}s\n" +
                $"Projectiles: {weapon.ProjectileAmount}\n" +
                $"Chains: {weapon.ChainsAmount}\n" +
                $"Pierce: {weapon.PierceAmount}\n" +
                $"Descriprion:\n{weapon.Description}\n");

        foreach (ModBase mod in weapon.ModsHolder.Prefixes)
        {
            if (mod != null)
            {
                foreach (ModTag tag in mod.ModTags)
                {
                    if (tag != ModTag.None)
                        tagStrBldr.Append($"{tag} ");
                }

                strBldr.Append($"\nPrfx {tagStrBldr}\n{mod.Description(mod)}");

                tagStrBldr.Clear();
            }
        }

        foreach (ModBase mod in weapon.ModsHolder.Suffixes)
        {
            if (mod != null)
            {
                foreach (ModTag tag in mod.ModTags)
                {
                    if (tag != ModTag.None)
                        tagStrBldr.Append($"{tag} ");
                }

                strBldr.Append($"\nSfx {tagStrBldr}\n{mod.Description(mod)}");

                tagStrBldr.Clear();
            }
        }

        Tooltip.Show($"{weapon.Name}", strBldr.ToString(), 16, 12);
    }

    private void ArmorTooltip(Armor armor)
    {
        strBldr.Append($"Health: {armor.HP}\n" +
                $"Armor: {armor.LocalArmor}\n" +
                $"Magic resist: {armor.MagicResist}\n" +
                $"Description:\n{armor.Description}\n");

        foreach (ModBase mod in armor.ModsHolder.Prefixes)
        {
            if (mod != null)
            {
                foreach (ModTag tag in mod.ModTags)
                {
                    if (tag != ModTag.None)
                        tagStrBldr.Append($"{tag} ");
                }

                strBldr.Append($"\nPrfx {tagStrBldr}\n{mod.Description(mod)}");

                tagStrBldr.Clear();
            }
        }

        foreach (ModBase mod in armor.ModsHolder.Suffixes)
        {
            if (mod != null)
            {
                foreach (ModTag tag in mod.ModTags)
                {
                    if (tag != ModTag.None)
                        tagStrBldr.Append($"{tag} ");
                }

                strBldr.Append($"\nSfx {tagStrBldr}\n{mod.Description(mod)}");

                tagStrBldr.Clear();
            }
        }

        Tooltip.Show($"{armor.Name}", strBldr.ToString(), 16, 12);
    }
}
