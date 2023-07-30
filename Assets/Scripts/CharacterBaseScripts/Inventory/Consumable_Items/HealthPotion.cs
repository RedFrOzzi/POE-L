using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : ConsumableItem
{
    public float HealFor = 50f;
    public override void UseItem(Inventory inventory)
    {
        inventory.health.TakeHeal(HealFor);
    }
}
