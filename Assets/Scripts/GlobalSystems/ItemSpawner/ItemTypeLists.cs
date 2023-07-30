using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypeLists
{
	private readonly List<List<IEquipmentItem>> itemLists = new();
    private readonly List<EquipmentType> existingTypes = new();

    public List<IEquipmentItem> GetItemList(EquipmentType type)
    {
        foreach (var l in itemLists)
        {
            if (l.Count <= 0)
            {
                Debug.Log("List have 0 elements in it");
                continue;
            }

            if (l[0].GetEquipmentType() == type)
            {
                return l;
            }
        }

        Debug.Log($"Can not find list with {type} type items");

        return new List<IEquipmentItem>(Array.Empty<IEquipmentItem>());
    }

    public void AddItemList(List<IEquipmentItem> list)
    {
        if (existingTypes.Contains(list[0].GetEquipmentType()))
        {
            Debug.Log($"List with type {list[0].GetEquipmentType()} already exist");
            return;
        }

        itemLists.Add(list);
        existingTypes.Add(list[0].GetEquipmentType());
    }
}
