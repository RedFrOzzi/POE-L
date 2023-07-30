using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources_UI_Element : MonoBehaviour
{
    private GameObject resourcesGO;
    private CH_Stats playerStats;
    private Image healthImage;
    private Image manaImage;
    private Health_UI_Element health;
    private Mana_UI_Element mana;

    private void Awake()
    {
        resourcesGO = GameObject.FindGameObjectWithTag("ResourcesUIParent");
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Stats>();
        health = resourcesGO.GetComponentInChildren<Health_UI_Element>();
        mana = resourcesGO.GetComponentInChildren<Mana_UI_Element>();

        health.SetStats(playerStats);
        mana.SetStats(playerStats);

        Image[] images = resourcesGO.GetComponentsInChildren<Image>();

        healthImage = images[1];
        healthImage.fillAmount = 1;
        manaImage = images[3];
        manaImage.fillAmount = 1;
    }

    private void Start()
    {
        playerStats.Health.OnHealthChange += OnHealthChange;
        playerStats.ManaComponent.OnManaChange += OnManaChange;
    }

    private void OnDestroy()
    {
        playerStats.Health.OnHealthChange -= OnHealthChange;
        playerStats.ManaComponent.OnManaChange -= OnManaChange;
    }

    private void OnHealthChange(float damage)
    {
        healthImage.fillAmount = playerStats.CurrentHP / playerStats.MaxHP;
    }

    private void OnManaChange()
    {
        manaImage.fillAmount = playerStats.CurrentMana / playerStats.MaxMana;
    }
}
