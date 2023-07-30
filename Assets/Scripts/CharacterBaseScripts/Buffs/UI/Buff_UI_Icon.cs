using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Database;
using TMPro;

public class Buff_UI_Icon : MonoBehaviour
{    
    [HideInInspector] public Buff buff = new();

    public void OnHoverEnter()
    {
        if (buff == null) { return; }

        Tooltip.Show(buff.Name, buff.Description(), 14, 12);
    }

    public void OnHoverExit()
    {
        Tooltip.Hide();
    }
}
