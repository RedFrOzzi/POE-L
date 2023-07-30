using UnityEngine;
using UnityEngine.EventSystems;

public class AbilitySlotRemove_UI_Element : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AbilitySlot_UI_Element slotUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        slotUI.RemoveAbility();
    }
}
