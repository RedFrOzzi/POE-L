using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private PlayerInputSettings inputSettings;

    [HideInInspector] public Vector2 LastForwardDirection { get; private set; }
    [HideInInspector] public Vector2 CurrentForwardDirection { get; private set; }

    private float moveSpeed = 10f;
    private Rigidbody2D rb2D;
    private Vector2 moveDir;
    private CH_Stats stats;
    private CH_AbilitiesManager abilitiesManager;
    private Equipment equipment;
    private SpriteRenderer spriteRenderer;
    private CH_Animation cH_Animation;
    private GameFlowManager gameFlowManager;

    private bool inventoryIsOpen = false;

    public event Action OpenInventory;
    public event Action OnPauseButtonPress;

    //triggered in Inventory
    public event Action<List<RaycastResult>> MouseDownRaycastTargets;
    public event Action<List<RaycastResult>> MouseUpRaycastTargets;

    private float moveX = 0f;
    private float moveY = 0f;

    private bool turnedRight = true;
    private bool isMoving = false;

    private readonly float equipmentSetChangeCD = 0.5f;
    private float nextEquipmentChange;

    private void Awake() {
        
        rb2D = GetComponent<Rigidbody2D>();
        stats = GetComponent<CH_Stats>();
        equipment = GetComponent<Equipment>();
        abilitiesManager = GetComponent<CH_AbilitiesManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cH_Animation = GetComponent<CH_Animation>();
    }

    private void Start()
    {
        gameFlowManager = GameFlowManager.Instance;

        stats.OnStatsChange += EvaluateMoveSpeed;
        stats.OnControllLoss += OnControllLoss;
        moveSpeed = stats.CurrentMovementSpeed;        
    }

    private void OnDestroy()
    {        
        stats.OnStatsChange -= EvaluateMoveSpeed;
        stats.OnControllLoss -= OnControllLoss;
    }


    private void Update()
    {
        if (gameFlowManager.IsGamePaused)
        {
            ManageConsole();

            ManagePause();

            ManageInventory();

            ManageSecondEquipmentSet();

            ManageInventoryPointer();

            ManageTalents();

            ManageAbilitiesOnKeyUp();

            return;
        }

        ManageConsole();

        ManagePause();

        ManageInventory();

        ManageSecondEquipmentSet();

        ManageTalents();

        if (stats.IsControllable == false) { return; }

        if (stats.CanMove)
        {
            ManageWASDInput();
        }

        if (stats.CanShoot)
        {
            ManageShooting();
            ManageAlternativeShoot();
            ManageReload();
        }

        if (stats.CanCast)
        {
            ManageAbilities();
            ManageAbilitiesOnKeyUp();
        }
        //----------------------------------------------

        moveDir = new Vector2(moveX, moveY).normalized;
        CurrentForwardDirection = moveDir;
    }

    private void FixedUpdate()
    {
        if (stats.IsControllable == false) { return; }

        rb2D.velocity = moveDir * moveSpeed;

        //---Animations---
        if (moveDir != Vector2.zero && isMoving == false)
        {
            cH_Animation.PlayWalk();
            isMoving = true;
        }
        else if (moveDir == Vector2.zero && isMoving)
        {
            cH_Animation.PlayIdle();
            isMoving = false;
        }
    }

    private void ManageWASDInput()
    {
        //MOVEMENT
        moveX = 0f;
        moveY = 0f;        

        if (Input.GetKey(inputSettings.UP))
        {
            moveY = +1f;
        }
        if (Input.GetKey(inputSettings.DOWN))
        {
            moveY = -1f;
        }
        if (Input.GetKey(inputSettings.LEFT))
        {
            moveX = -1f;
        }
        if (Input.GetKey(inputSettings.RIGHT))
        {
            moveX = +1f;
        }
        
        if (Mathf.Approximately(moveX, 0f) && Mathf.Approximately(moveY, 0f)) { return; }

        LastForwardDirection = new Vector2(moveX, moveY);

        FlipSpriteIfNeeded();
    }

    private void ManageInventoryPointer()
    {
        if (inventoryIsOpen == false) { return; }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            MouseDownRaycastTargets?.Invoke(raycastResults);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            MouseUpRaycastTargets?.Invoke(raycastResults);
        }
    }

    private void ManageShooting()
    {
        //SHOOT
        if (Input.GetKeyDown(inputSettings.SHOOT))
        {
            equipment.ShootOnce();
        }

        if (Input.GetKey(inputSettings.SHOOT))
        {
            equipment.Shoot();
        }

        if (Input.GetKeyUp(inputSettings.SHOOT))
        {
            equipment.OnShootButtonUp();
        }
    }

    private void ManageAlternativeShoot()
    {
        if (Input.GetKey(inputSettings.ALTERNATIVESHOOT))
        {
            //aim
        }
    }

    private void ManageReload()
    {
        //RELOAD
        if (Input.GetKeyDown(inputSettings.RELOAD) && !IsWeaponReloading() && equipment.EqupipmentList[EquipmentSlot.Weapon] != null)
        {
            equipment.Reload();
        }
    }

    private void ManageAbilities()
    {
        if (Input.GetKeyDown(inputSettings.ABILITY_1))
        {
            abilitiesManager.ActivateAbilityManualy(0);
        }
        
        if (Input.GetKeyDown(inputSettings.ABILITY_2))
        {
            abilitiesManager.ActivateAbilityManualy(1);
        }
        
        if (Input.GetKeyDown(inputSettings.ABILITY_3))
        {
            abilitiesManager.ActivateAbilityManualy(2);
        }
        
        if (Input.GetKeyDown(inputSettings.ABILITY_4))
        {
            abilitiesManager.ActivateAbilityManualy(3);
        }
        
        if (Input.GetKeyDown(inputSettings.ABILITY_5))
        {
            abilitiesManager.ActivateAbilityManualy(4);
        }
        
        if (Input.GetKeyDown(inputSettings.ABILITY_6))
        {
            abilitiesManager.ActivateAbilityManualy(5);
        }
    }

    private void ManageAbilitiesOnKeyUp()
    {
        if (Input.GetKeyUp(inputSettings.ABILITY_1))
        {
            abilitiesManager.OnAbilityButtonUp(0);
        }

        if (Input.GetKeyUp(inputSettings.ABILITY_2))
        {
            abilitiesManager.OnAbilityButtonUp(1);
        }

        if (Input.GetKeyUp(inputSettings.ABILITY_3))
        {
            abilitiesManager.OnAbilityButtonUp(2);
        }

        if (Input.GetKeyUp(inputSettings.ABILITY_4))
        {
            abilitiesManager.OnAbilityButtonUp(3);
        }

        if (Input.GetKeyUp(inputSettings.ABILITY_5))
        {
            abilitiesManager.OnAbilityButtonUp(4);
        }

        if (Input.GetKeyUp(inputSettings.ABILITY_6))
        {
            abilitiesManager.OnAbilityButtonUp(5);
        }
    }

    private void ManageTalents()
    {
        if (Input.GetKeyDown(inputSettings.TALENTS))
        {
            gameFlowManager.OnOpenTalents();
        }
    }

    private void ManageInventory()
    {
        if (Input.GetKeyDown(inputSettings.INVENTORY) && gameFlowManager.IsPlayerAllowedToOpenInventory)
        {
            OpenInventory?.Invoke(); //вызывается в Inventory_UI_Element

            inventoryIsOpen = !inventoryIsOpen;

            gameFlowManager.OnOpenInventory();
        }
    }

    private void ManageSecondEquipmentSet()
    {
        if (Input.GetKeyDown(inputSettings.SWAPEQUIPMENTSET))
        {
            if (Time.unscaledTime < nextEquipmentChange) { return; }
            nextEquipmentChange = Time.unscaledTime + equipmentSetChangeCD;

            equipment.ChangeEquipmentSet();
        }
    }

    private void ManagePause()
    {
        if (Input.GetKeyDown(inputSettings.PAUSE) && gameFlowManager.IsPlayerAllowedToPause)
        {
            OnPauseButtonPress?.Invoke();

            gameFlowManager.OnPauseButtonPress();
        }
    }

    private void ManageConsole()
    {
        if (Input.GetKeyDown(inputSettings.CONSOLE))
        {
            CheatCodeManager.Instance.ToggleConsole();
        }

        if (Input.GetKeyDown(inputSettings.ENTER))
        {
            CheatCodeManager.Instance.HandleInput();
        }
    }

    private void EvaluateMoveSpeed()
    {
        moveSpeed = stats.CurrentMovementSpeed;
    }

    private bool IsWeaponReloading()
    {
        if (equipment.EqupipmentList[EquipmentSlot.Weapon] is Weapon weapon)
        {
            return weapon.IsReloading;
        }
        else
            return false;
    }

    private void FlipSpriteIfNeeded()
    {
        if (Vector2.Dot(moveDir, Vector2.right) < 0f && turnedRight)
        {
            spriteRenderer.flipX = true;
            turnedRight = false;
        }
        else if (Vector2.Dot(moveDir, Vector2.right) > 0f && turnedRight == false)
        {
            spriteRenderer.flipX = false;
            turnedRight = true;
        }
    }

    private void OnControllLoss(bool isControllable)
    {
        cH_Animation.Freeze(!isControllable);
    }
}
