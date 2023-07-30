using UnityEngine;

[CreateAssetMenu(fileName = "BasicArmorSM", menuName = "ScriptableObjects/SignatureMods/Armor/BasicArmorSM")]
public class BasicArmorSM : SignatureMod
{
    [SerializeField] private float health;
    [SerializeField] private int armor;
    [SerializeField] private int resist;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        if (equipmentItem is Armor armr)
        {
            armr.ChangeBaseStats(health, armor, resist);
        }
    }
}
