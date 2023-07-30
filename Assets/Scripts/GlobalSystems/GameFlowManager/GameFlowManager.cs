using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    [SerializeField] private bool enableChallenges;

    [SerializeField, Space(10)] ChallengesManager challengesManager;
    [SerializeField] ItemSpawner itemSpawner;
    [SerializeField] TalentsCardsManager talentsCardsManager;
    [SerializeField] AbilitiesSwaper_UI_Element swaper_UI;

    private CH_Stats playerStats;

    private readonly Timer Timer = new();
    private TextMeshProUGUI timerUI;

    private bool inventoryIsOpen = false;
    private bool gameIsPausedByPlayer = false;
    private bool pausePeriod = false;
    private bool talentsIsOpen = false;
    private bool talentCardsIsOpen = false;
    private bool abilitiesSwapIsOpen = false;

    private readonly WaitForSecondsRealtime waitForHalfSecondRealtime = new(0.5f);

    public bool IsPlayerAllowedToPause { get; private set; } = true;
    public bool IsPlayerAllowedToOpenInventory { get; private set; } = true;
    public bool IsPlayerAllowedToTakeTalents { get; private set; } = true;

    private bool isPlayerAllowedToOpenTalents = true;
    public bool IsGamePaused { get; private set; } = false;

    public event Action<bool> OpenTalents;

    private void Awake()
    {
        if (Instance == null)        
            Instance = this;        
        else if (Instance != null)
            Destroy(gameObject);

        timerUI = GameObject.FindGameObjectWithTag("Timer_UI").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CH_Stats>();

        playerStats.ItemCollector.OnItemPickUp += OnItemPickUp;
        Timer.OnCountdownEnd += ShowFirstChallenge;
        playerStats.OnLevelUp += OnPlayerLevelUp;
        swaper_UI.OnAbilitySwaperShow += OnAbilitySwap;
        swaper_UI.OnAbilitySwaperHide += OnAbilitySwapEnd;

        Timer.SetTimerActive(true);

        if (enableChallenges == false) { return; }

        Timer.SetCountdouwnActive(3f);
    }

    private void OnDestroy()
    {
        Timer.OnCountdownEnd -= ShowFirstChallenge;
        playerStats.OnLevelUp -= OnPlayerLevelUp;
        playerStats.ItemCollector.OnItemPickUp -= OnItemPickUp;
        swaper_UI.OnAbilitySwaperShow -= OnAbilitySwap;
        swaper_UI.OnAbilitySwaperHide -= OnAbilitySwapEnd;
    }

    private void Update()
    {
        if (IsGamePaused) { return; }

        Timer.UpdateTime();
        timerUI.text = Timer.GetCurrentTimeInString();
    }

    public void PauseGame(bool shouldPause)
    {
        if (shouldPause)        
            Time.timeScale = 0;
        else        
            Time.timeScale = 1;
       
        IsGamePaused = shouldPause;
    }

    public float GetCurrentRoundTimeInMinutes()
    {
        return Timer.GetCurrentTimeInMinutes();
    }

    public float GetCurrentRoundTimeInSeconds()
    {
        return Timer.GetCurrentTimeInSeconds();
    }

    public int GetDifficultyForTimedRound()
    {
        if (Timer.GetCurrentTimeInMinutes() >= 60f)
            return 12;

        if (Timer.GetCurrentTimeInMinutes() >= 55f)
            return 11;

        if (Timer.GetCurrentTimeInMinutes() >= 50f)
            return 10;

        if (Timer.GetCurrentTimeInMinutes() >= 45f)
            return 9;

        if (Timer.GetCurrentTimeInMinutes() >= 40f)
            return 8;

        if (Timer.GetCurrentTimeInMinutes() >= 35f)
            return 7;

        if (Timer.GetCurrentTimeInMinutes() >= 30f)
            return 6;

        if (Timer.GetCurrentTimeInMinutes() >= 25f)
            return 5;

        if (Timer.GetCurrentTimeInMinutes() >= 20f)
            return 4;

        if (Timer.GetCurrentTimeInMinutes() >= 15f)
            return 3;

        if (Timer.GetCurrentTimeInMinutes() >= 10f)
            return 2;

        if (Timer.GetCurrentTimeInMinutes() >= 5f)
            return 1;

        return 0;
    }

    private void ManageGamePause()
    {
        //Убрать текущие подсказки
        Tooltip.Hide();

        if (pausePeriod)
        {
            IsPlayerAllowedToOpenInventory = false;
            isPlayerAllowedToOpenTalents = false;
            IsPlayerAllowedToPause = false;
            PauseGame(true);
        }
        else
        {
            IsPlayerAllowedToOpenInventory = true;
            isPlayerAllowedToOpenTalents = true;
            IsPlayerAllowedToPause = true;
            PauseGame(false);
        }

        //Блокировка управления в конце раунда
        if (pausePeriod) { return; }

        if (abilitiesSwapIsOpen)
        {
            IsPlayerAllowedToOpenInventory = false;
            isPlayerAllowedToOpenTalents = false;
            IsPlayerAllowedToPause = false;
            PauseGame(true);
        }
        else
        {
            IsPlayerAllowedToOpenInventory = true;
            isPlayerAllowedToOpenTalents = true;
            IsPlayerAllowedToPause = true;
            PauseGame(false);
        }

        //Блокировка управления при смене абилки
        if (abilitiesSwapIsOpen) { return; }

        if (talentCardsIsOpen)
        {
            IsPlayerAllowedToTakeTalents = false;
            PauseGame(true);
        }
        else
        {
            IsPlayerAllowedToTakeTalents = true;
            PauseGame(false);
        }

        //Пауза для талантов
        if (talentCardsIsOpen) { return; }

        if (gameIsPausedByPlayer)
        {
            IsPlayerAllowedToOpenInventory = false;
            isPlayerAllowedToOpenTalents = false;
            PauseGame(true);
        }
        else
        {
            IsPlayerAllowedToOpenInventory = true;
            isPlayerAllowedToOpenTalents = true;
            PauseGame(false);
        }

        //Блокировка управления при паузе
        if (gameIsPausedByPlayer) { return; }

        if (inventoryIsOpen == false && talentsIsOpen == false)
        {
            PauseGame(false);
        }
        else
        {
            PauseGame(true);
        }
    }

    public void OnOpenTalents()
    {
        if (isPlayerAllowedToOpenTalents == false) { return; }

        talentsIsOpen = !talentsIsOpen;

        OpenTalents?.Invoke(talentsIsOpen);

        ManageGamePause();
    }

    public void OnOpenInventory()
    {
        inventoryIsOpen = !inventoryIsOpen;

        ManageGamePause();
    }

    public void OnPauseButtonPress()
    {
        gameIsPausedByPlayer = !gameIsPausedByPlayer;

        ManageGamePause();
    }

    //----------------GAME_FLOW----------------

    //---------------ON-LEVEL-UP---------------
    private void OnPlayerLevelUp()
    {
        talentCardsIsOpen = true;

        ManageGamePause();

        StartCoroutine(ShowTalentsToChooseCoroutine());
    }

    public void OnTalentsCardsClose()
    {
        talentCardsIsOpen = false;

        ManageGamePause();
    }

    //--------------ON-ITEM-PICK-UP--------------
    private void OnItemPickUp()
    {
        talentCardsIsOpen = true;
        ManageGamePause();

        StartCoroutine(ShowItemsToChoosePickUpCoroutine());
    }

    public void OnItemChoosen()
    {
        talentCardsIsOpen = false;

        ManageGamePause();
    }

    //---------------)N-ABILITY-SWAP-----------
    private void OnAbilitySwap()
    {
        abilitiesSwapIsOpen = true;
        ManageGamePause();
    }

    private void OnAbilitySwapEnd()
    {
        abilitiesSwapIsOpen = false;
        ManageGamePause();
    }

    //---------------CHALLENGES----------------
    public void ShowFirstChallenge()
    {
        pausePeriod = true;

        ManageGamePause();

        StartCoroutine(ShowChallengesCoroutine());
    }
    //---01---тригерится после выполнения условий испытания---
    public void OnChallangeComplete()
    {
        pausePeriod = true;

        ManageGamePause();
    }
    //---02---тригерится после взятия нового испытания---
    public void OnChallengeChoosen()
    {
        StartCoroutine(ShowItemsToChooseCoroutine());
    }
    //---03---тригерится после взятия предмета---
    public void OnItemChoosenChallenge()
    {
        OnInBetweenRoundsTimeEnd();
    }
    //---04---тригерится в самом конце, выходя из меню выбора---
    private void OnInBetweenRoundsTimeEnd()
    {
        pausePeriod = false;

        ManageGamePause();
    }

    //---------------InBeteenRoundsTransitions------------------
    private IEnumerator ShowChallengesCoroutine()
    {
        yield return waitForHalfSecondRealtime;
        
        challengesManager.ShowChallenges();
    }

    private IEnumerator ShowItemsToChooseCoroutine()
    {
        yield return waitForHalfSecondRealtime;

        itemSpawner.ShowItemCards(true);
    }

    private IEnumerator ShowItemsToChoosePickUpCoroutine()
    {
        yield return waitForHalfSecondRealtime;

        itemSpawner.ShowItemCards(false);
    }

    private IEnumerator ShowTalentsToChooseCoroutine()
    {
        yield return waitForHalfSecondRealtime;

        talentsCardsManager.OnLevelUp();
    }
    //----------------------------------------------------------
}