using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mana_UI_Element : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CH_Stats stats;

    public void SetStats(CH_Stats stats)
    {
        this.stats = stats;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Show("", $"{stats.CurrentMana} / {stats.MaxMana}", 16, 16);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Hide();
    }
}
