using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Database;
using System.Linq;

public class Buffs_UI_Element : MonoBehaviour
{
    [SerializeField] private List<Buff_UI_Icon> buff_UI_Icons;
    
    private CH_BuffManager buffManager;

    private void Start()
    {
        buff_UI_Icons = GameObject.FindGameObjectWithTag("BuffsUIParent").GetComponentsInChildren<Buff_UI_Icon>().ToList();
        buffManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_BuffManager>();

        foreach (Buff_UI_Icon icon in buff_UI_Icons)
        {
            icon.gameObject.SetActive(false);
        }

        buffManager.OnBuffsChange += OnBuffsChange;
    }

    private void OnDestroy()
    {
        buffManager.OnBuffsChange -= OnBuffsChange;
    }

    private void OnBuffsChange()
    {
        for (int i = 0; i < buffManager.Buffs.Count; i++)
        {
            if (buffManager.Buffs[i].ShouldShow)
            {
                buff_UI_Icons[i].buff = buffManager.Buffs[i];
                buff_UI_Icons[i].gameObject.SetActive(true);
            }
        }

        for (int i = buffManager.Buffs.Count; i < buff_UI_Icons.Count; i++)
        {
            buff_UI_Icons[i].OnHoverExit();
            buff_UI_Icons[i].gameObject.SetActive(false);
        }
    }
}
