using Database;
using UnityEngine;

[CreateAssetMenu(fileName = "BookIgniteSM", menuName = "ScriptableObjects/SignatureMods/Book/BookIgniteSM")]
public class BookIgniteSM : SignatureMod
{
    [SerializeField] private float cooldownRecoveryPercent;
    [SerializeField] private float magicDamagePercent;
    [SerializeField, Space(10)] private int maxStacks;
    [SerializeField] private float percentDamageToIgnite;
    [SerializeField] private float duration;

    private IncendiaryEffect incendiaryEffect;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description = Description + $"\nDamage Increase {magicDamagePercent}%.\nCooldown reduction increase {cooldownRecoveryPercent}%";

        incendiaryEffect = new IncendiaryEffect
        {
            MaxStacks = maxStacks,
            PercentDamageToIgnite = percentDamageToIgnite,
            Duration = duration,
            GeneratedID = equipmentItem.ID
        };

        equipmentItem.Description += $"\n{incendiaryEffect.Description()}";

        if (equipmentItem is Book book)
        {
            book.OnEquipAction = () =>
            {
                book.Equipment.Stats.GSC.MagicSC.IncreaseCooldownReduction(cooldownRecoveryPercent);
                book.Equipment.Stats.GSC.GlobalSC.IncreaseMagicDamage(magicDamagePercent);

                book.Equipment.Stats.OnHit.AddOnHit_GivingMagicHit(incendiaryEffect);
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
