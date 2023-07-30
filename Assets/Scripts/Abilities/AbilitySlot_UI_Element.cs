using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Database;

public class AbilitySlot_UI_Element : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    private Ability ability;
    private AbilitySlot slot;

    private void Awake()
    {
        image = GetComponentsInChildren<Image>()[1];
        image.fillAmount = 0;
    }

    public void RemoveAbility()
    {
        slot.AbilitiesManager.RemoveAbility(slot.SlotIndex);
    }

    public void SetUpAbilitySlot(Ability ability, AbilitySlot slot)
    {
        this.ability = ability;
        this.slot = slot;
    }

    public void StartCooldown(float cooldown)
    {
        StartCoroutine(CooldownVisuals(cooldown));
    }

    private IEnumerator CooldownVisuals(float cooldown)
    {
        for (float i = 1; i > 0; i -= 1 / cooldown * Time.deltaTime)
        {
            image.fillAmount = i;            
            yield return null;
        }

        image.fillAmount = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ability == null) { return; }

        Tooltip.Show($"{ability.Name}", $"Cooldown: {slot.GetAbilityCooldown()}" +
            $"\nLevel: {ability.Level + slot.LSC.MagicSC.FlatAbilityLevelValue + slot.Stats.GSC.MagicSC.FlatAbilityLevelValue}" +
            $"\nManacost: {slot.GetAbilityManacost()}" +
            $"\nTags: {ability.Tags[0]} " +
            $"{ability.Tags[1]} " +
            $"{ability.Tags[2]} " +
            $"{ability.Tags[3]} " +
            $"{ability.Tags[4]}" +
            $"\nDescription: {ability.Description()}", 16, 12);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Hide();
    }
}
