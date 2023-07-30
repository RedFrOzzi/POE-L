using UnityEngine;
using Database;
using System;
using System.Collections;

public class CH_AbilitiesSwaper : MonoBehaviour
{
	private AbilitiesDatabase abilitiesDatabase;
    private AbilityWeights aw;
    private CH_AbilitiesManager abilitiesManager;

    private AbilitySlot[] oldAbilitySlots;

    public event Action<Ability> OnSwaperPanelActivation;

    private void Start()
    {
        abilitiesDatabase = GameDatabasesManager.Instance.AbilitiesDatabase;
        aw = GameDatabasesManager.Instance.AbilityWeights;
        abilitiesManager = GetComponent<CH_AbilitiesManager>();
    }

    public void SetUpAbility(Ability ability)
    {
        if (TryGetFirstFreeSlot(out byte index))
        {
            abilitiesManager.SetUpAbility(index, ability);
        }
        else
        {
            OnSwaperPanelActivation?.Invoke(ability);
        }
    }

    public void SetUpAbility()
    {
        if (TryGetFirstFreeSlot(out byte index))
        {
            var ability = GetWeightedAbility();

            abilitiesManager.SetUpAbility(index, ability);
        }
        else
        {
            var ability = GetWeightedAbility();

            OnSwaperPanelActivation?.Invoke(ability);
        }
    }

    private Ability GetWeightedAbility()
    {
        return aw.GetWeightedAbility();
    }

    private Ability GetRandomAbility()
    {
        return abilitiesDatabase.AbilitiesList.PickRandom();
    }

    private bool TryGetFirstFreeSlot(out byte index)
    {
        oldAbilitySlots ??= abilitiesManager.GetAbilitySlotRefs();

        for (byte i = 0; i < oldAbilitySlots.Length; i++)
        {
            if (oldAbilitySlots[i].Ability.ID == AbilityID.None || oldAbilitySlots[i].Ability.ID == AbilityID.Void)
            {
                index = i;
                return true;
            }
        }

        Debug.Log("There is no free ability slots");

        index = 0;
        return false;
    }
}
