using Database;
using UnityEngine;

[CreateAssetMenu(fileName = "BookKnockbackSM", menuName = "ScriptableObjects/SignatureMods/Book/BookKnockbackSM")]
public class BookKnockbackSM : SignatureMod
{
    [SerializeField] private float cooldownRecoveryPercent;
    [SerializeField] private float magicDamagePercent;
    [SerializeField, Space(10)] private float distance;

    private KnockbackEffect knockbackEffect;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description = Description + $"\nDamage Increase {magicDamagePercent}%.\nCooldown reduction increase {cooldownRecoveryPercent}%";

        knockbackEffect = new KnockbackEffect
        {
            GeneratedID = equipmentItem.ID,
            Distance = distance
        };

        equipmentItem.Description += $"\n{knockbackEffect.Description()}";

        if (equipmentItem is Book book)
        {
            book.OnEquipAction = () =>
            {
                book.Equipment.Stats.GSC.MagicSC.IncreaseCooldownReduction(cooldownRecoveryPercent);
                book.Equipment.Stats.GSC.GlobalSC.IncreaseMagicDamage(magicDamagePercent);

                book.Equipment.Stats.OnHit.AddOnHit_GivingMagicHit(knockbackEffect);
            };

            book.OnUnEquipAction = () =>
            {
                book.Equipment.Stats.GSC.MagicSC.IncreaseCooldownReduction(-cooldownRecoveryPercent);
                book.Equipment.Stats.GSC.GlobalSC.IncreaseMagicDamage(-magicDamagePercent);

                book.Equipment.Stats.OnHit.RemoveOnHit_GivingMagicHit(book.ID);
            };
        }
    }
}
