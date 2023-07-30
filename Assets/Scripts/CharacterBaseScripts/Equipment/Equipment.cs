using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Database;
public class Equipment : MonoBehaviour
{
    public Dictionary<EquipmentSlot, EquipmentItem> EqupipmentList { get; private set; } = new()
    {
        {EquipmentSlot.Helmet, null },
        {EquipmentSlot.BodyArmor, null },
        {EquipmentSlot.Boots, null },
        {EquipmentSlot.Gloves , null },
        {EquipmentSlot.Weapon, null },
        {EquipmentSlot.LeftHand, null }
    };

    public Dictionary<EquipmentSlot, EquipmentItem> SecondEquipmentSet { get; private set; } = new()
    {
        { EquipmentSlot.Weapon, null },
        { EquipmentSlot.LeftHand, null }
    };

    //-------------GEMS-------------------------------------------------

    public WeaponGem[] MainWeaponGems { get; private set; } = new WeaponGem[5];
    public WeaponGem[] SecodaryWeaponGems { get; private set; } = new WeaponGem[5];
    public WeaponGem[][] WeaponGems { get; private set; }

    private readonly WeaponGem[] temp1WeaponGems = new WeaponGem[5];
    private readonly WeaponGem[] temp2WeaponGems = new WeaponGem[5];

    public StatsChanges MainWeaponLSC { get; private set; } = new();
    public StatsChanges SecondaryWeaponLSC { get; private set; } = new();
    public StatsChanges[] WeaponsLSC { get; private set; }

    private readonly StatsChanges temp1WeaponLSC = new();
    private readonly StatsChanges temp2WeaponLSC = new();

    //-------------------------------------------------------------------
    public Weapon CurrentEquipedWeapon { get; private set; } = null;
    //-------------------------------------------------------------------

    private Inventory inventory;
    public CH_Stats Stats { get; private set; }

    public event Action<EquipmentItem, EquipmentItem> OnEquipmentChange;

    public event Action OnWeaponShot;

    public event Action<byte, byte, WeaponGem> OnWeaponGemChange; //слот оружия, слот гема, гем

    //------PROJECTILE-PARAMETERS------------
    public GameObject ProjectileParentObject { get; private set; }
    public GameObject AimGO { get; private set; }
    public Transform Player { get; private set; }
    public float ProjLiveTime 
    { get
        {
            return Stats.CurrentAttackRange / Stats.CurrentProjectileSpeed;
        }
    }
    //----------------------------------------

    public WeaponGem weaponGem;
    public WeaponGem weaponGem2;
    public AbilityGem abilityGem;
    public AbilityGem abilityGem2;
    public EquipmentItem equipmentItem;
    public EquipmentItem equipmentItem2;
    public EquipmentItem equipmentItem3;
    public EquipmentItem equipmentItem4;



    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        Stats = GetComponent<CH_Stats>();

        ProjectileParentObject = GameObject.FindGameObjectWithTag("ProjectilesParent");
        AimGO = GameObject.FindGameObjectWithTag("Aim");
        Player = gameObject.transform;

        WeaponGems = new WeaponGem[4][] { MainWeaponGems , SecodaryWeaponGems, temp1WeaponGems, temp2WeaponGems };
        WeaponsLSC = new StatsChanges[4] { MainWeaponLSC, SecondaryWeaponLSC, temp1WeaponLSC, temp2WeaponLSC };
    }

    //ВРЕМЕННОЕ ДОБАВЛЕНИЕ ОРУЖИЯ
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        if (equipmentItem != null)
        {
            var go = Instantiate(equipmentItem, new Vector3(100, 100, 0), Quaternion.identity);
            go.GetComponent<EquipmentItem>().Initialize();
            inventory.AddItem(go.GetComponent<EquipmentItem>());
        }

        if (equipmentItem2 != null)
        {
            var go2 = Instantiate(equipmentItem2, new Vector3(100, 100, 0), Quaternion.identity);
            go2.GetComponent<EquipmentItem>().Initialize();
            inventory.AddItem(go2.GetComponent<EquipmentItem>());
        }

        if (equipmentItem3 != null)
        {
            var go3 = Instantiate(equipmentItem3, new Vector3(100, 100, 0), Quaternion.identity);
            go3.GetComponent<EquipmentItem>().Initialize();
            inventory.AddItem(go3.GetComponent<EquipmentItem>());
        }

        if (equipmentItem4 != null)
        {
            var go4 = Instantiate(equipmentItem4, new Vector3(100, 100, 0), Quaternion.identity);
            go4.GetComponent<EquipmentItem>().Initialize();
            inventory.AddItem(go4.GetComponent<EquipmentItem>());
        }

        if (abilityGem2 != null)
        {
            var go5 = Instantiate(abilityGem2, new Vector3(100, 100, 0), Quaternion.identity);
            go5.GetComponent<AbilityGem>().Initialize();
            inventory.AddItem(go5.GetComponent<AbilityGem>());
        }

        if (abilityGem != null)
        {
            var go6 = Instantiate(abilityGem, new Vector3(100, 100, 0), Quaternion.identity);
            go6.GetComponent<AbilityGem>().Initialize();
            inventory.AddItem(go6.GetComponent<AbilityGem>());
        }

        if (weaponGem != null)
        {
            var go7 = Instantiate(weaponGem, new Vector3(100, 100, 0), Quaternion.identity);
            go7.GetComponent<WeaponGem>().Initialize();
            inventory.AddItem(go7.GetComponent<WeaponGem>());
        }

        if (weaponGem2 != null)
        {
            var go8 = Instantiate(weaponGem2, new Vector3(100, 100, 0), Quaternion.identity);
            go8.GetComponent<WeaponGem>().Initialize();
            inventory.AddItem(go8.GetComponent<WeaponGem>());
        }
        //--------------------------------------
    }


    // вызывается из контроллера персонажа для атаки    
    public void Shoot()
    {
        if (CurrentEquipedWeapon == null) { return; }

        CurrentEquipedWeapon.Shoot(this);
    }

    public void ShootOnce()
    {
        if (CurrentEquipedWeapon == null) { return; }

        CurrentEquipedWeapon.ShootOnce(this);
    }

    public void OnShootButtonUp()
    {
        if (CurrentEquipedWeapon != null)
            CurrentEquipedWeapon.OnShootButtonUp(this);
    }

    
    // вызывается из контроллера персонажа для перезарядки    
    public void Reload()
    {
        if (CurrentEquipedWeapon != null && Stats.CurrentAmmo < Stats.CurrentAmmoCapacity)
        {
            CurrentEquipedWeapon.Reload(this);
        }
    }

    // вызывается в оружии
    public void WeaponShot()
    {
        OnWeaponShot?.Invoke();
    }

    private void OnEquipChange()
    {
        if (EqupipmentList[EquipmentSlot.Weapon] is Weapon weapon)
        {
            CurrentEquipedWeapon = weapon;
            CurrentEquipedWeapon.StopReloadRoutine();
            CurrentEquipedWeapon.OnEquipmentChange();
        }
        else
        {
            CurrentEquipedWeapon = null;
        }
    }

    
    //Помещает указанный item в слод экипировки    
    //Возвращает true если item помещен в экипировку
    public bool EquipItem(EquipmentItem item)
    {     

        if(item == null || !inventory.InventoryList.Contains(item))
        {
            return false;
        }

        if (EqupipmentList[item.EquipmentSlot] == null)
        {
            EqupipmentList[item.EquipmentSlot] = item;
            item.Equipment = this;
            inventory.RemoveItem(item);
            item.OnEquipAction();
            ApplyStatsFromItem(item, null);

            OnEquipChange();

            return true;
        }
        else
            return false;
    }

    public bool UnequipItemFromSlot(EquipmentSlot slot)
    {
        EquipmentItem oldItem = EqupipmentList[slot];

        if(oldItem == null)
        {
            return false;
        }

        if (CurrentEquipedWeapon != null)
            CurrentEquipedWeapon.StopReloadRoutine();

        inventory.AddItem(oldItem);
        oldItem.OnUnEquipAction();
        EqupipmentList[slot] = null;
        ApplyStatsFromItem(null, oldItem);

        OnEquipChange();

        return true;

    }


    /// <summary>
    /// Экипирует item и помещает старый предмет в инвентарь
    /// </summary>
    /// <param name="item">предмет который необходимо экипировать</param>
    /// <returns>Возвращает true если item помещен в экипировку</returns>
    public bool SwapItemFromInventory(EquipmentItem item)
    {
        EquipmentSlot slot = item.EquipmentSlot;
        EquipmentItem oldItem = EqupipmentList[slot];

        if (item == null || EqupipmentList[slot] == null || !inventory.InventoryList.Contains(item))
        {            
            return false;
        }

        if (CurrentEquipedWeapon != null)
            CurrentEquipedWeapon.StopReloadRoutine();

        //перемещаем предмет из экипировки в инвентарь
        inventory.AddItem(oldItem);
        oldItem.OnUnEquipAction();
        //добовляем предмет в экипировку
        EqupipmentList[slot] = item;
        item.Equipment = this;
        item.OnEquipAction();
        //убераем экипированный предмет из инвенеторя
        inventory.RemoveItem(item);
        ApplyStatsFromItem(item, oldItem);

        OnEquipChange();

        return true;
    }

    public void ChangeEquipmentSet()
    {
        if (CurrentEquipedWeapon != null)
            CurrentEquipedWeapon.StopReloadRoutine();

        EquipmentItem temp = EqupipmentList[EquipmentSlot.Weapon];
        //---WEAPON-SWAP---
        if (EqupipmentList[EquipmentSlot.Weapon] != null)
            EqupipmentList[EquipmentSlot.Weapon].OnUnEquipAction();
        
        EqupipmentList[EquipmentSlot.Weapon] = SecondEquipmentSet[EquipmentSlot.Weapon];
        if (SecondEquipmentSet[EquipmentSlot.Weapon] != null)
        {
            SecondEquipmentSet[EquipmentSlot.Weapon].Equipment = this;
            SecondEquipmentSet[EquipmentSlot.Weapon].OnEquipAction();
        }
        SecondEquipmentSet[EquipmentSlot.Weapon] = temp;

        ApplyStatsFromItem(EqupipmentList[EquipmentSlot.Weapon], SecondEquipmentSet[EquipmentSlot.Weapon]);
        //---LEFTHAND-SWAP---
        temp = EqupipmentList[EquipmentSlot.LeftHand];

        if (EqupipmentList[EquipmentSlot.LeftHand] != null)
            EqupipmentList[EquipmentSlot.LeftHand].OnUnEquipAction();

        EqupipmentList[EquipmentSlot.LeftHand] = SecondEquipmentSet[EquipmentSlot.LeftHand];
        if (SecondEquipmentSet[EquipmentSlot.LeftHand] != null)
        {
            SecondEquipmentSet[EquipmentSlot.LeftHand].Equipment = this;
            SecondEquipmentSet[EquipmentSlot.LeftHand].OnEquipAction();
        }
        SecondEquipmentSet[EquipmentSlot.LeftHand] = temp;

        ApplyStatsFromItem(EqupipmentList[EquipmentSlot.LeftHand], SecondEquipmentSet[EquipmentSlot.LeftHand]);
        //--------------------

        OnEquipChange();

        OnWeaponSetChangeSwapGems();
    }

    public void EquipGem(byte weaponPos, byte gemSlotPos, WeaponGem gem)
    {
        if (gem == null || !inventory.InventoryList.Contains(gem)) { return; }

        if (WeaponGems[weaponPos][gemSlotPos] == null)
        {
            WeaponGems[weaponPos][gemSlotPos] = gem;
            gem.Equipment = this;
            gem.WeaponIndex = weaponPos;
            inventory.RemoveItem(gem);
            gem.OnEquipAction();

            if (weaponPos == 0)
                ApplyStatsFromGem(gem, null);
        }
        else
        {
            SwapGem(weaponPos, gemSlotPos, gem);
        }

        OnWeaponGemChange?.Invoke(weaponPos, gemSlotPos, gem);
    }

    public void UnEquipGem(byte weaponPos, byte gemSlotPos)
    {

        WeaponGem oldGem = WeaponGems[weaponPos][gemSlotPos];

        if (oldGem == null) { return; }

        inventory.AddItem(oldGem);
        oldGem.OnUnEquipAction();
        WeaponGems[weaponPos][gemSlotPos] = null;

        if (weaponPos == 0)
            ApplyStatsFromGem(null, oldGem);

        OnWeaponGemChange?.Invoke(weaponPos, gemSlotPos, null);
    }

    public void SwapGem(byte weaponPos, byte gemSlotPos, WeaponGem gem)
    {
        if (gem == null || !inventory.InventoryList.Contains(gem) || WeaponGems[weaponPos][gemSlotPos] == null) { return; }

        WeaponGem oldGem = WeaponGems[weaponPos][gemSlotPos];

        //перемещаем предмет из экипировки в инвентарь
        inventory.AddItem(oldGem);
        oldGem.OnUnEquipAction();
        //добовляем предмет в экипировку
        WeaponGems[weaponPos][gemSlotPos] = gem;
        gem.Equipment = this;
        gem.WeaponIndex = weaponPos;
        gem.OnEquipAction();
        //убераем экипированный предмет из инвенеторя
        inventory.RemoveItem(gem);

        ApplyStatsFromGem(gem, oldGem);

        OnWeaponGemChange?.Invoke(weaponPos, gemSlotPos, gem);
    }

    private void OnWeaponSetChangeSwapGems()
    {
        for (byte i = 0; i < WeaponGems[0].Length; i++)
        {
            if (WeaponGems[0][i] != null)
            {
                WeaponGems[2][i] = WeaponGems[0][i];

                WeaponGems[0][i].OnUnEquipAction();
                ApplyStatsFromGem(null, WeaponGems[0][i]);

                WeaponGems[0][i] = null;

                OnWeaponGemChange?.Invoke(0, i, null);
            }

            if (WeaponGems[1][i] != null)
            {
                WeaponGems[3][i] = WeaponGems[1][i];

                WeaponGems[1][i].OnUnEquipAction();
                WeaponGems[1][i] = null;

                OnWeaponGemChange?.Invoke(1, i, null);
            }
        }

        for (byte i = 0; i < WeaponGems[0].Length; i++)
        {
            if (WeaponGems[2][i] != null)
            {
                WeaponGems[1][i] = WeaponGems[2][i];
                WeaponGems[1][i].Equipment = this;
                WeaponGems[1][i].WeaponIndex = 1;
                WeaponGems[1][i].OnEquipAction();

                OnWeaponGemChange?.Invoke(1, i, WeaponGems[1][i]);
            }

            if (WeaponGems[3][i] != null)
            {
                WeaponGems[0][i] = WeaponGems[3][i];
                WeaponGems[0][i].Equipment = this;
                WeaponGems[0][i].WeaponIndex = 0;
                WeaponGems[0][i].OnEquipAction();
                ApplyStatsFromGem(WeaponGems[0][i], null);

                OnWeaponGemChange?.Invoke(0, i, WeaponGems[0][i]);
            }

            WeaponGems[2][i] = null;
            WeaponGems[3][i] = null;
        }
    }

    private void ApplyStatsFromGem(WeaponGem newGem, WeaponGem oldGem)
    {
        if (oldGem != null)
        {
            Debug.Log("old gem remove " + oldGem.Name);
            Debug.Log("old gem stat " + oldGem.LSC.AttackSC.IncreaseAttackDamageValue);
            Stats.GSC.RemoveChanges(oldGem.LSC);  //Локальные моды

            oldGem.ModsHolder.RemoveGlobalModsModifiers(); //Глобальные моды

            Stats.EvaluateStats();
        }

        if (newGem != null)
        {
            Stats.GSC.CombineChanges(newGem.LSC); //Локальные моды

            newGem.ModsHolder.ApplyGlobalModsModifiers(Stats);  //Глобальные моды

            Stats.EvaluateStats();
        }
    }

    private void ApplyStatsFromItem(EquipmentItem newItem, EquipmentItem oldItem)
    {

        if (newItem is Weapon || oldItem is Weapon)
        {
            Weapon newWeapon = (Weapon)newItem;
            Weapon oldWeapon = (Weapon)oldItem;

            if (oldWeapon != null)
            {
                oldWeapon.CurrentAmmo = Stats.CurrentAmmo;

                Stats.RemoveStatsFromOldWeapon(oldWeapon);

                oldWeapon.ModsHolder.RemoveGlobalModsModifiers(); //убирает эффект модов предмета с персонажа

                Stats.EvaluateStats();
            }

            if (newWeapon != null)
            {
                Stats.ApplyStatsFromNewWeapon(newWeapon);

                newWeapon.IsReloading = false;
                newWeapon.ModsHolder.ApplyGlobalModsModifiers(Stats);  //добавляет эффект модов предмета  персонажа

                Stats.EvaluateStats();
            }
        }
        else if (newItem is Armor || oldItem is Armor)
        {
            Armor newArmor = (Armor)newItem;
            Armor oldArmor = (Armor)oldItem;

            if (oldArmor != null)
            {
                Stats.RemoveStatsFromOldArmor(oldArmor);

                oldArmor.ModsHolder.RemoveGlobalModsModifiers();

                Stats.EvaluateStats();
            }

            if (newArmor != null)
            {
                Stats.ApplyStatsFromNewArmor(newArmor);

                newArmor.ModsHolder.ApplyGlobalModsModifiers(Stats);

                Stats.EvaluateStats();
            }
        }

        OnEquipmentChange?.Invoke(newItem, oldItem);
    }
}
