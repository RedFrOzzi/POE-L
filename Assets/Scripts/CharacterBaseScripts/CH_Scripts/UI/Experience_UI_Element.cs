using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Experience_UI_Element : MonoBehaviour
{
    [SerializeField] private Image expBar;
    private CH_Stats stats;

    private float currentExp;
    private float expTillNextLvl;

    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Stats>();

        expBar.fillAmount = 0f;

        stats.Experience.OnExpGain += OnExpGain;
    }

    private void OnDestroy()
    {
        stats.Experience.OnExpGain -= OnExpGain;
    }

    private void OnExpGain(float exp, float expTillLvlUp, float prevLvlExp)
    {
        expBar.fillAmount = 1 - expTillLvlUp / (exp - prevLvlExp + expTillLvlUp);

        currentExp = exp;
        expTillNextLvl = expTillLvlUp;
    }

    //вызывается из expBar gameObj
    public void ShowExpTooltip()
    {
        Tooltip.Show("", $"{Mathf.FloorToInt(currentExp)} / {Mathf.CeilToInt(currentExp + expTillNextLvl)}", 16, 16);
    }

    public void HideExpTooltip()
    {
        Tooltip.Hide();
    }
}
