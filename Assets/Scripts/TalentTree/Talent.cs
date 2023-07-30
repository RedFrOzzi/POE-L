using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Database;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Linq;
using DG.Tweening;

public class Talent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [ContextMenuItem("Rename", "RenameGameObject")]
    public string Rename = "Right click to rename";

    [field: SerializeField, Space(10)] public Rarity TalentRarity { get; private set; }

    [field: SerializeField, Space(10)] public string Name { get; private set; }

    [field: SerializeField, TextArea(5, 10)] public string Description { get; private set; }

    [field: SerializeField, Space(10)] public TalentGroup TalentGroup { get; private set; }

    [field: SerializeField, Space(10)] public int IndexID { get; private set; }

    public TalentProperties TalentProperties { get; private set; }

    private Dictionary<Rarity, Vector3> rarityToScale = new()
    {
        { Rarity.Common, new Vector3(0.7f, 0.7f, 0.7f) },
        { Rarity.Uncommon, new Vector3(1f, 1f, 1f) },
        { Rarity.Rare, new Vector3(1.2f, 1.2f, 1.2f) },
        { Rarity.Epic, new Vector3(1.5f, 1.5f, 1.5f) },
        { Rarity.Mythic, new Vector3(1.7f, 1.7f, 1.7f) },
        { Rarity.Legendary, new Vector3(2f, 2f, 2f) },
        { Rarity.None, new Vector3(10f, 10f, 10f) }
    };


    //talent fields
    private Image image;  

    //color fields
    private const float lockedColorShadePercent = 70;
    private Color initialColor;
    private Color shadedColor;

    private void Awake()
    {
        TalentGroup = GetComponentInParent<TalentGroup>();

        image = GetComponentsInChildren<Image>()[1];

        initialColor = image.color;

        shadedColor = new Color(image.color.r - image.color.r / 100 * lockedColorShadePercent
            , image.color.g - image.color.g / 100 * lockedColorShadePercent
            , image.color.b - image.color.b / 100 * lockedColorShadePercent
            , 255);

        image.color = shadedColor;

        if (TalentsDatabase.TalentProperties == null)
            TalentsDatabase.Initialize();

        TalentProperties = TalentsDatabase.TalentProperties[Name];

        Name = TalentProperties.Name;
        Description = TalentProperties.Description;
        image.sprite = TalentsDatabase.TalentSprites[TalentProperties.Name];
        TalentRarity = TalentProperties.TalentRarity;
        transform.localScale = rarityToScale[TalentRarity];
    }

    public void InitializeTalent(int id)
    {
        IndexID = id;
    }

    public void SetAttributes(string name, string description, Rarity talentRarity)
    {
        Name = name;
        Description = description;
        TalentRarity = talentRarity;
    }

    public void SetStartingPointColor()
    {
        image.color = initialColor;
    }

    public void UnlockTalent()
    {
        image.color = initialColor;
    }

    public void LockTalent()
    {
        image.color = shadedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Show($"{Name}\n", $"{Description}", 16, 14);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Hide();
    }

    private void RenameGameObject()
    {
        gameObject.name = TalentsDatabase.PropsData.PropsDictionary[Name].Name + "_" + transform.GetSiblingIndex().ToString();

        Description = TalentsDatabase.PropsData.PropsDictionary[Name].Description;
        TalentRarity = TalentsDatabase.PropsData.PropsDictionary[Name].TalentRarity;

        transform.localScale = TalentRarity switch
        {
            Rarity.Common => new Vector3(0.7f, 0.7f, 0.7f),
            Rarity.Uncommon => new Vector3(1f, 1f, 1f),
            Rarity.Rare => new Vector3(1.2f, 1.2f, 1.2f),
            Rarity.Epic => new Vector3(1.5f, 1.5f, 1.5f),
            Rarity.Mythic => new Vector3(1.7f, 1.7f, 1.7f),
            Rarity.Legendary => new Vector3(2f, 2f, 2f),
            _ => new Vector3(10f, 10f, 10f)
        };
    }
}