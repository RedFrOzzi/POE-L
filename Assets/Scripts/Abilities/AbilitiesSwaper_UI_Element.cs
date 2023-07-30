using Database;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesSwaper_UI_Element : MonoBehaviour
{
    [SerializeField] private Image newAbilityIcon;
    [SerializeField] private Image[] oldAbilityIcons;

    [SerializeField, Space(10)] private GameObject panel;

    private CH_AbilitiesManager abilitiesManager;
    private CH_AbilitiesSwaper abilitiesSwaper;

    private Ability newAbility;
    private AbilitySlot[] slots;

    private byte? choosenAbilityIndex = null;

    public event Action OnAbilitySwaperShow;
    public event Action OnAbilitySwaperHide;

    private void Start()
    {
        abilitiesManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_AbilitiesManager>();
        abilitiesSwaper = abilitiesManager.GetComponent<CH_AbilitiesSwaper>();
        slots = abilitiesManager.GetAbilitySlotRefs();

        abilitiesSwaper.OnSwaperPanelActivation += ShowSwaperPanel;
    }

    private void OnDestroy()
    {
        abilitiesSwaper.OnSwaperPanelActivation -= ShowSwaperPanel;
    }

    public void ShowSwaperPanel(Ability newAbility)
    {
        choosenAbilityIndex = null;
        panel.SetActive(true);
        newAbilityIcon.sprite = newAbility.Sprite;

        for (byte i = 0; i < oldAbilityIcons.Length; i++)
        {
            oldAbilityIcons[i].sprite = slots[i].Ability.Sprite;
        }

        this.newAbility = newAbility;

        OnAbilitySwaperShow?.Invoke();
    }

    public void OnClickIcon01()
    {
        choosenAbilityIndex = 0;
    }

    public void OnClickIcon02()
    {
        choosenAbilityIndex = 1;
    }

    public void OnClickIcon03()
    {
        choosenAbilityIndex = 2;
    }

    public void OnClickIcon04()
    {
        choosenAbilityIndex = 3;
    }

    public void OnClickIcon05()
    {
        choosenAbilityIndex = 4;
    }

    public void OnClickIcon06()
    {
        choosenAbilityIndex = 5;
    }

    public void OnClickApply()
    {
        if (choosenAbilityIndex == null)
        {
            Debug.Log("Choose ability first");
            return;
        }

        abilitiesManager.SwapAbility((byte)choosenAbilityIndex, newAbility);

        panel.SetActive(false);

        OnAbilitySwaperHide?.Invoke();
    }
}
