using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Pool;
using TMPro;
using UnityEngine.EventSystems;
using Database;
using UnityEditorInternal.Profiling.Memory.Experimental;
using System;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance = null;

    [SerializeField] private Transform parentForDropedItems;
    [SerializeField] private Transform parentForCollectedItems;

    [Space(10)][Header("Spawnable Item")]
    [SerializeField] private GameObject spawnableItemPrefab;

    [Space(10), Header("Gold Prefab")]
    [SerializeField] private GoldCoin goldPrefab;

    [Space(10), Header("UI")]
    [SerializeField] private GameObject itemsCardsPanel;
    [SerializeField] private ItemToChooseCard[] itemToChooseCards;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private EventTrigger acceptButton;
    private int? selectedItemCard = null;

    private const float hilightAlpha = 1f;
    private const float normalAlpha = 0.67f;

    [Space(10), Header("Ability show chance")]
    [SerializeField] private float weaponToAbilityTransformChance;
    [SerializeField] private AbilityWeights abilityWeights;

    [Space(10), Header("Added weapon damage based on difficulty")]
    [SerializeField] private List<int> addedDamage;

    [Space(10), Header("Ability gem tier based on difficulty")]
    [SerializeField] private List<byte> tiers;

    [Space(10), Header("Experience values")]
    [SerializeField] private float meleeBaseExp;
    [SerializeField] private float rangeBaseExp;
    [SerializeField] private float specialBaseExp;
    [SerializeField] private float eliteBaseExp;
    [SerializeField] private float bossBaseExp;

    [Space(10), Header("Gold values")]
    [SerializeField] private int meleeBaseGold;
    [SerializeField] private int rangeBaseGold;
    [SerializeField] private int specialBaseGold;
    [SerializeField] private int eliteBaseGold;
    [SerializeField] private int bossBaseGold;

    private GameObject player;
    private Inventory playerInventory;
    private CH_AbilitiesSwaper abilitiesSwaper;
    private CH_Experience playerExp;
    private CH_Stats playerStats;
    private ItemSpawnProbabilityCalculator spawnCalculator;
    private EnemySpawner enemySpawner;
    private GameFlowManager gameFlowManager;

    private List<Weapon> weapons; 
    private List<Armor> armors; 
    private List<LeftHand> leftHands;
    private List<AbilityGem> abilityGems;

    public ItemTypeLists ItemTypeLists { get; private set; } = new();

    private ObjectPool<GoldCoin> goldCoinsPool;

    private bool itemsGeneratedByChallenge = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance = this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<Inventory>();
        abilitiesSwaper = player.GetComponent<CH_AbilitiesSwaper>();
        playerStats = player.GetComponent<CH_Stats>();
        playerExp = player.GetComponent<CH_Experience>();
        gameFlowManager = GameFlowManager.Instance;

        weapons = new(Resources.LoadAll<Weapon>("Weapons"));
        armors = new(Resources.LoadAll<Armor>("Armors"));
        leftHands = new(Resources.LoadAll<LeftHand>("LeftHands"));
        abilityGems = new(Resources.LoadAll<AbilityGem>("AbilityGems"));

        FormAbilityGemLists();
        FormArmorLists();
        FormLeftHandLists();
        FormWeaponLists();

        itemsCardsPanel.SetActive(false);

        spawnCalculator = new(GameDatabasesManager.Instance.SpawnChancesDatabase);

        enemySpawner = EnemySpawner.Instance;

        goldCoinsPool = new(CreateGoldObject, OnTakeFromPool, OnReturnToPool);

        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => AcceptCard());
        acceptButton.triggers.Add(entry);

        textMesh.text = string.Empty;
    }

    public void OnEnemyDeath(CharacterType characterType, Vector2 position)
    {
        //---DROPS-------------------------------------------
        float exp = 0;
        bool toSpawn = false;

        switch (characterType)
        {
            case CharacterType.Melee:
                exp = meleeBaseExp;
                break;

            case CharacterType.Ranged: 
                exp = rangeBaseExp;
                break;

            case CharacterType.Special:
                exp = specialBaseExp;
                break;

            case CharacterType.Elite:
                exp = eliteBaseExp;
                toSpawn = true;
                break;

            case CharacterType.Boss:
                exp = bossBaseExp;
                break;

            default:
                Debug.Log("Unexpected Character type");
                break;
        };

        if (toSpawn)
        {
            SpawnItem(position);
        }
        //--------------------------------------------------------

        //---EXP--------------------------------------------------
        playerExp.GainExp(exp * enemySpawner.SpawnPhase);
        //--------------------------------------------------------

        //---GOLD-------------------------------------------------
        var coin = goldCoinsPool.Get();
        coin.Set(GetGoldAmount(characterType, enemySpawner.SpawnPhase), position);
        //--------------------------------------------------------
    }

    public void SpawnItem(Vector2 position)
    {
        Instantiate(spawnableItemPrefab, position, Quaternion.identity, parentForDropedItems);
    }

    public void GiveRandomItem(byte itemTier)
    {
        var eI = GetItemBasedOnTier(itemTier) as Item;
        eI.transform.SetParent(parentForCollectedItems);

        playerInventory.AddItem(eI);
    }

    public void SelectItemCard(int cardIndex)
    {
        UnselectItemCards(cardIndex);

        itemToChooseCards[cardIndex].CardBackground.SetTransparency(hilightAlpha);

        textMesh.text = itemToChooseCards[cardIndex].Text;

        selectedItemCard = cardIndex;
    }

    private void UnselectItemCards(int exceptIndex)
    {
        foreach (var card in itemToChooseCards)
        {
            if (card.CardIndex == exceptIndex)
                continue;

            card.CardBackground.SetTransparency(normalAlpha);
        }
    }

    private void AcceptCard()
    {
        if (selectedItemCard == null)
        {
            Debug.Log("Select item first!");
            return;
        }

        if (itemToChooseCards[(int)selectedItemCard].EquipmentItem != null)
            GiveItem(itemToChooseCards[(int)selectedItemCard].EquipmentItem);
        else
            GiveAbility(itemToChooseCards[(int)selectedItemCard].Ability);

        foreach (var card in itemToChooseCards)
        {
            if (card.CardIndex != selectedItemCard)
                card.DestroyNotUsedItem();

            card.EquipmentItem = null;
            card.Ability = null;
        }

        selectedItemCard = null;

        HideItemCards();
    }

    public void GiveItem(IEquipmentItem item)
    {
        var itm = item as Item;
        itm.transform.SetParent(parentForCollectedItems);
        playerInventory.AddItem(itm);
    }

    public void GiveAbility(Ability ability)
    {
        abilitiesSwaper.SetUpAbility(ability);
    }

    public void ShowItemCards(bool isGeneratedByChallenge)
    {
        itemsGeneratedByChallenge = isGeneratedByChallenge;

        textMesh.text = string.Empty;

        GenerateItemForCards();

        itemsCardsPanel.SetActive(true);
    }

    public void HideItemCards()
    {
        itemsCardsPanel.SetActive(false);

        if (itemsGeneratedByChallenge)
            gameFlowManager.OnItemChoosenChallenge();
        else
            gameFlowManager.OnItemChoosen();
    }

    private void GenerateItemForCards()
    {
        for (int i = 0; i < itemToChooseCards.Length; i++)
        {
            var item = GetItem(out EquipmentSlot slot);

            //≈сли прошел шанс, то оружие замен€етс€ на абилку
            if (slot == EquipmentSlot.Weapon && UnityEngine.Random.value < weaponToAbilityTransformChance)
            {
                itemToChooseCards[i].SetUpCard(i, GetAbility());
            }
            else
            {
                itemToChooseCards[i].SetUpCard(i, item);
            }
        }
    }

    private IEquipmentItem GetItemBasedOnTier(byte itemTier)
    {
        return GetItem(out _);
    }

    private IEquipmentItem GetItem(out EquipmentSlot slot)
    {
        EquipmentSlot _slot = spawnCalculator.GetWeightedEquipmentSlotType();
        EquipmentType type = spawnCalculator.GetWeightedEquipmentType(_slot);
        var list = ItemTypeLists.GetItemList(type);

        if (list.Count <= 0)
        {
            slot = _slot;
            return weapons.PickRandom();
        }

        IEquipmentItem item = list.PickRandom();

        var go = Instantiate(item as Item, new Vector2(10000, 10000), Quaternion.identity, parentForDropedItems);
        IEquipmentItem et = (IEquipmentItem)go;

        if (et is Weapon weapon)
            UpdateWeaponStats(weapon);
        else if (et is AbilityGem gem)
            UpdateAbilityGemStats(gem);

        et.Initialize();

        slot = _slot;
        return et;
    }

    public void GetItem(EquipmentSlot slot, float cost)
    {
        EquipmentType type = spawnCalculator.GetWeightedEquipmentType(slot);
        var list = ItemTypeLists.GetItemList(type);

        if (list.Count <= 0)
        {
            Debug.LogException(new Exception("List of this item type is empty"));
        }

        IEquipmentItem item = list.PickRandom();

        var go = Instantiate(item as Item, new Vector2(10000, 10000), Quaternion.identity, parentForDropedItems);
        IEquipmentItem et = (IEquipmentItem)go;

        if (et is Weapon weapon)
            UpdateWeaponStats(weapon);
        else if (et is AbilityGem gem)
            UpdateAbilityGemStats(gem);

        et.Initialize();

        playerInventory.AddItem(et as Item);
        playerStats.SpentGold((int)cost);
    }

    private Ability GetAbility()
    {
        return abilityWeights.GetWeightedAbility();
    }

    private int GetGoldAmount(CharacterType characterType, int spawnPhase)
    {
        return characterType switch
        {
            CharacterType.Melee => meleeBaseGold * spawnPhase,
            CharacterType.Ranged => rangeBaseGold * spawnPhase,
            CharacterType.Special => specialBaseGold * spawnPhase,
            CharacterType.Elite => eliteBaseGold * spawnPhase,
            CharacterType.Boss => bossBaseGold * spawnPhase,

            _ => 0
        };
    }

    private void UpdateWeaponStats(Weapon weapon)
    {
        weapon.AddBaseDamage(addedDamage[gameFlowManager.GetDifficultyForTimedRound()]);
    }

    private void UpdateAbilityGemStats(AbilityGem gem)
    {
        gem.SignatureMod.UpdateMod(tiers[gameFlowManager.GetDifficultyForTimedRound()]);
    }

    private GoldCoin CreateGoldObject()
    {
        var AO = Instantiate(goldPrefab, transform);
        AO.SetPool(goldCoinsPool);
        return AO;
    }

    private void OnTakeFromPool(GoldCoin gold)
    {
        gold.gameObject.SetActive(true);
    }

    private void OnReturnToPool(GoldCoin gold)
    {
        gold.gameObject.SetActive(false);
    }

    private void FormWeaponLists()
    {
        List<EquipmentType> types = new();
        foreach (var item in weapons)
        {
            if (types.Contains(item.GetEquipmentType()))
            {
                continue;
            }

            types.Add(item.GetEquipmentType());
        }

        foreach (var t in types)
        {
            if (t == EquipmentType.None)
                continue;

            ItemTypeLists.AddItemList(weapons.Where(x => x.GetEquipmentType() == t).ToList<IEquipmentItem>());
        }
    }

    private void FormArmorLists()
    {
        List<EquipmentType> types = new();
        foreach (var item in armors)
        {
            if (types.Contains(item.GetEquipmentType()))
            {
                continue;
            }

            types.Add(item.GetEquipmentType());
        }

        foreach (var t in types)
        {
            if (t == EquipmentType.None)
                continue;

            ItemTypeLists.AddItemList(armors.Where(x => x.GetEquipmentType() == t).ToList<IEquipmentItem>());
        }
    }

    private void FormLeftHandLists()
    {
        List<EquipmentType> types = new();
        foreach (var item in leftHands)
        {
            if (types.Contains(item.GetEquipmentType()))
            {
                continue;
            }

            types.Add(item.GetEquipmentType());
        }

        foreach (var t in types)
        {
            if (t == EquipmentType.None)
                continue;

            ItemTypeLists.AddItemList(leftHands.Where(x => x.GetEquipmentType() == t).ToList<IEquipmentItem>());
        }
    }

    private void FormAbilityGemLists()
    {
        List<EquipmentType> types = new();
        foreach (var item in abilityGems)
        {
            if (types.Contains(item.GetEquipmentType()))
            {
                continue;
            }

            types.Add(item.GetEquipmentType());
        }

        foreach (var t in types)
        {
            if (t == EquipmentType.None)
                continue;

            ItemTypeLists.AddItemList(abilityGems.Where(x => x.GetEquipmentType() == t).ToList<IEquipmentItem>());
        }
    }
}
