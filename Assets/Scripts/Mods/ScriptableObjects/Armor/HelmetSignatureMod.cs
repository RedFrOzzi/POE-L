using UnityEngine;

[CreateAssetMenu(fileName = "HelmetSignatureMod", menuName = "ScriptableObjects/SignatureMods/Armor/Helmet")]
public class HelmetSignatureMod : SignatureMod
{
    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Armor armor)
        {            
        }
    }
}
