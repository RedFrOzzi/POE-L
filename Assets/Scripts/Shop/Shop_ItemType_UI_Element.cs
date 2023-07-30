using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_ItemType_UI_Element : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI descriptionText;
    private Shop shop;
    private EquipmentSlot equipmentSlot;
    private float price;

    public void SetUp(Shop shop, EquipmentSlot slot, Sprite sprite, float initialPrice)
	{
        this.shop = shop;
        equipmentSlot = slot;
        image.sprite = sprite;
        price = initialPrice;

        descriptionText.text = $"Price: {price}";
    }

    public void NewPrice(float newPrice)
    {
        price = newPrice;
        descriptionText.text = $"Price: {price}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shop.TryBuyItem(equipmentSlot);
    }
}
