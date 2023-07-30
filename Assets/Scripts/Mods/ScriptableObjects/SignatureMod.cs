using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SignatureMod : ScriptableObject
{
    [field: SerializeField] public string Name { get; protected set; }
    [field: SerializeField] public byte Tier { get; protected set; }
    [field: SerializeField, TextArea(10, 15)] public string Description { get; protected set; }

    public void Initialize(IEquipmentItem item)
    {
        item.ID = Guid.NewGuid().ToString();
        item.Name = Name;
        item.Description = Description;
        item.Sprite = ((MonoBehaviour)item).GetComponent<SpriteRenderer>().sprite;
    }
    public virtual void ApplySignatureMod(IEquipmentItem item) { }

    public virtual void UpdateMod(byte currentTier) { }
}
