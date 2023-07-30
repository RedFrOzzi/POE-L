using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Item : MonoBehaviour
{
    [field:SerializeField] public string Name { get; set; }
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public Sprite Sprite { get; set; }
}
