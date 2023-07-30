using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DebugSignatureMod", menuName = "ScriptableObjects/SignatureMods/AbilityGem/DebugMod")]
public class AbilityGemDebug : AbilityGemSignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem abilityGem)
    {
        Debug.Log($"{abilityGem.Name}'s signature mod OnApply");

        abilityGem.OnEquipAction = OnEquip;
        abilityGem.OnUnEquipAction = OnUnequip;

        void OnEquip()
        {
            Debug.Log("OnEquip Action with gem signature mod");
        }
        void OnUnequip()
        {
            Debug.Log("OnUnequip Action with gem signature mod");
        }
    }
}
