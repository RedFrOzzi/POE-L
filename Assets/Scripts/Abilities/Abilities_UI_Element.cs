using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities_UI_Element : MonoBehaviour
{
    [SerializeField, Space(10)] private AbilitySlot_UI_Element[] UIAbilitySlots = new AbilitySlot_UI_Element[6];

    private CH_AbilitiesManager abilitiesManager;
    private AbilitySlot[] abilitySlots;

    private void Start()
    {
        UIAbilitySlots = GameObject.FindGameObjectWithTag("AbilitiesUIParent").GetComponentsInChildren<AbilitySlot_UI_Element>();
        abilitiesManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_AbilitiesManager>();

        abilitySlots = abilitiesManager.GetAbilitySlotRefs();

        abilitiesManager.OnAbilityChange += OnAbilityChange;
        abilitiesManager.CDOnAbilityActivation += CDOnAbilityActivation;
    }

    private void OnDestroy()
    {
        abilitiesManager.OnAbilityChange -= OnAbilityChange;
        abilitiesManager.CDOnAbilityActivation -= CDOnAbilityActivation;
    }

    private void OnAbilityChange(byte slotIndex)
    {
        SetAbilitiesReferances(slotIndex);
    }

    private void SetAbilitiesReferances(int slotIndex)
    {
        UIAbilitySlots[slotIndex].SetUpAbilitySlot(abilitySlots[slotIndex].Ability, abilitySlots[slotIndex]);
    }

    private void CDOnAbilityActivation(int slotIndex, float cooldown)
    {
        UIAbilitySlots[slotIndex].StartCooldown(cooldown);
    }
}
