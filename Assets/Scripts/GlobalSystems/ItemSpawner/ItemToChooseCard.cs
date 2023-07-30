using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;
using Database;

public class ItemToChooseCard : MonoBehaviour, IPointerClickHandler
{
	public int CardIndex { get; private set; }
	public IEquipmentItem EquipmentItem { get; set; }
	public Ability Ability { get; set; }
	public string Text { get; private set; }

	private byte tier;

	public Image CardBackground { get; private set; }
	[SerializeField] private Image itemIcon;

	private const float normalAlpha = 0.67f;

	private ItemSpawner itemSpawner;

	private readonly StringBuilder strBldr = new();

    private void Start()
    {
		itemSpawner = ItemSpawner.Instance;
		CardBackground = GetComponent<Image>();
    }

    public void SetUpCard(int index, IEquipmentItem equipmentItem)
    {
		CardIndex = index;
		EquipmentItem = equipmentItem;
		tier = 0;
		CardBackground.SetTransparency(normalAlpha);

		itemIcon.sprite = equipmentItem.Sprite;
		itemIcon.color = (equipmentItem as Item).GetComponent<SpriteRenderer>().color;

		switch (tier)
        {
			case 0:
				CardBackground.color = RarityColors.Color[Rarity.Common];
				break;
			case 1:
				CardBackground.color = RarityColors.Color[Rarity.Uncommon];
				break;
			case 2:
				CardBackground.color = RarityColors.Color[Rarity.Rare];
				break;
			case 3:
				CardBackground.color = RarityColors.Color[Rarity.Epic];
				break;
			case 4:
				CardBackground.color = RarityColors.Color[Rarity.Mythic];
				break;
			case 5:
				CardBackground.color = RarityColors.Color[Rarity.Legendary];
				break;
		}

		strBldr.Clear();

		if (equipmentItem is Weapon weapon)
			strBldr.Append(GetTextForWeaponLocalStats(weapon));

		if (equipmentItem is Armor armor)
			strBldr.Append(GetTextForArmorLocalStats(armor));

		strBldr.Append($"\n");

		strBldr.Append($"{equipmentItem.Description}\n");

		strBldr.Append("\n");

		foreach (var prefix in equipmentItem.ModsHolder.Prefixes)
        {
			if (prefix.Name == "EmptyMod") { continue; }

			strBldr.Append($"{prefix.Description(prefix)}\n");
		}

		strBldr.Append("\n");

		foreach (var suffix in equipmentItem.ModsHolder.Suffixes)
		{
			if (suffix.Name == "EmptyMod") { continue; }

			strBldr.Append($"{suffix.Description(suffix)}\n");
		}

		Text = strBldr.ToString();
	}

	public void SetUpCard(int index, Ability ability)
	{
        CardIndex = index;
		tier = 0;
		Ability = ability;

        CardBackground.SetTransparency(normalAlpha);

        itemIcon.sprite = ability.Sprite;

        switch (tier)
        {
            case 0:
                CardBackground.color = RarityColors.Color[Rarity.Common];
                break;
            case 1:
                CardBackground.color = RarityColors.Color[Rarity.Uncommon];
                break;
            case 2:
                CardBackground.color = RarityColors.Color[Rarity.Rare];
                break;
            case 3:
                CardBackground.color = RarityColors.Color[Rarity.Epic];
                break;
            case 4:
                CardBackground.color = RarityColors.Color[Rarity.Mythic];
                break;
            case 5:
                CardBackground.color = RarityColors.Color[Rarity.Legendary];
                break;
        }

        strBldr.Clear();

		strBldr.Append(ability.Tip);

		Text = strBldr.ToString();
    }

	public void DestroyNotUsedItem()
    {
		if (EquipmentItem != null)
			Destroy((EquipmentItem as Item).gameObject);

		EquipmentItem = null;
		Ability = null;
	}

    public void OnPointerClick(PointerEventData eventData)
    {
		itemSpawner.SelectItemCard(CardIndex);
    }

	private string GetTextForWeaponLocalStats(Weapon item)
    {
		return $"Damage: {item.MinDamage} - {item.MaxDamage}\n" +
            $"Attack speed: {item.AttackSpeed}/s\n" +
            $"Crit chance: {item.CritChance}%\n" +
            $"Crit Multi: {item.CritMultiplier}%\n" +
            $"Reload speed: {item.ReloadSpeed}\n" +
            $"Ammo capacity: {item.AmmoCapacity}\n" +
            $"Accuracy: {item.Accuracy}\n" +
            $"Projectiles: {item.ProjectileAmount}\n" +
            $"Pierce: {item.PierceAmount}\n" +
            $"Chains: {item.ChainsAmount}\n";
    }

	private string GetTextForArmorLocalStats(Armor item)
	{
		return $"Health: {item.HP}\n" +
            $"Armor: {item.LocalArmor}\n" +
            $"Magic resist: {item.MagicResist}\n";
	}
}
