using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;
using Database;

public class Item_UI_Element : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item Item { get; private set; }
    private Inventory Inventory;
    private Sprite Sprite;
    private StringBuilder strBldr = new();
    private StringBuilder tagStrBldr = new();

    public void SetUpItemUIElement(Sprite sprite, Item item, Inventory inventory)
    {
        Sprite = sprite;
        Item = item;
        Inventory = inventory;
    }

    public void SetUpFieldsToNull()
    {
        Sprite = null;
        Item = null;
        Inventory = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item == null) { return; }

        if (Item is Weapon weapon)
        {
            WeaponTooltip(weapon);
        }
        else if (Item is Armor armor)
        {
            ArmorTooltip(armor);
        }
        else if(Item is AbilityGem abilityGem)
        {
            AbilityGemTooltip(abilityGem);
        }
        else
        {
            Tooltip.Show($"Name: {Item.Name}", $"{Item.Description}", 16, 12);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        strBldr.Clear();
        tagStrBldr.Clear();

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

    private void AbilityGemTooltip(AbilityGem abilityGem)
    {
        if (abilityGem.Description != null && abilityGem.Description != string.Empty)
            strBldr.Append($"\nDescription: {abilityGem.Description}");

        strBldr.Append("\n");

        foreach (ModBase mod in abilityGem.ModsHolder.Prefixes)
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

        foreach (ModBase mod in abilityGem.ModsHolder.Suffixes)
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

        Tooltip.Show($"{abilityGem.Name}", strBldr.ToString(), 16, 12);
    }
}
