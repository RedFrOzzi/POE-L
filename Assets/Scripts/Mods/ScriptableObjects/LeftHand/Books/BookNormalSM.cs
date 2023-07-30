using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "BookNormalSM", menuName = "ScriptableObjects/SignatureMods/Book/BookNormalSM")]
public class BookNormalSM : SignatureMod
{
    [SerializeField] private float cooldownReductionPercent;
    [SerializeField] private float magicDamagePercent;
    [SerializeField] private float manacostPercent;
    [SerializeField] private float manaRegenPercent;
    [SerializeField] private float critChancePercent;
    [SerializeField] private int critMulti;

    public override void ApplySignatureMod(IEquipmentItem equipmentItem)
    {
        equipmentItem.Description = Description + GetDescription();

        if (equipmentItem is Book book)
        {
            book.OnEquipAction = () =>
            {
                book.Equipment.Stats.GSC.MagicSC.IncreaseCooldownReduction(cooldownReductionPercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseSpellDamage(magicDamagePercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseManacost(-manacostPercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseManaRegeneration(manaRegenPercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseSpellCritChance(critChancePercent);
                book.Equipment.Stats.GSC.MagicSC.AddFlatSpellCritMultiplier(critMulti);
            };

            book.OnUnEquipAction = () =>
            {
                book.Equipment.Stats.GSC.MagicSC.IncreaseCooldownReduction(-cooldownReductionPercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseSpellDamage(-magicDamagePercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseManacost(manacostPercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseManaRegeneration(-manaRegenPercent);
                book.Equipment.Stats.GSC.MagicSC.IncreaseSpellCritChance(-critChancePercent);
                book.Equipment.Stats.GSC.MagicSC.AddFlatSpellCritMultiplier(-critMulti);
            };
        }
    }

    private string GetDescription()
    {
        StringBuilder strBldr = new();

        if (magicDamagePercent != 0)
            strBldr.Append($"\nSpell damage Increase {magicDamagePercent}%.");

        if (cooldownReductionPercent != 0)
            strBldr.Append($"\nCooldown reduction increase {cooldownReductionPercent}%.");

        if (manacostPercent != 0)
            strBldr.Append($"\nManacost decrease {manacostPercent}%");

        if (critChancePercent != 0)
            strBldr.Append($"\nSpell crit chance increase {critChancePercent}%.");

        if (critMulti != 0)
            strBldr.Append($"\nAdd {critMulti}% to spell crit multiplier.");

        if (manaRegenPercent != 0)
            strBldr.Append($"\nMana regen increase {manaRegenPercent}%.");

        return strBldr.ToString();
    }
}
