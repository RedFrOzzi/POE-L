using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemStatsTooltip_UI_Element : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateStats(string text)
    {
        textMesh.text = text;
    }

    public void UpdateStats(string text, int maxFontSize)
    {
        textMesh.fontSizeMax = maxFontSize;
        textMesh.text = text;
    }
}
