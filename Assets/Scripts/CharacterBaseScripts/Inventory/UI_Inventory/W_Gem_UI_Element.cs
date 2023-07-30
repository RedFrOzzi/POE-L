using UnityEngine;
using UnityEngine.UI;

public class W_Gem_UI_Element : MonoBehaviour
{
    public WeaponGem WeaponGem { get; private set; }
    public byte WeaponNumber { get; private set; }
    public byte SlotNumber { get; private set; }

    private Image gemImage;

    private void Awake()
    {
        gemImage = GetComponent<Image>();
    }

    public void SetUpWeaponGemUI(byte weaponNum, byte slotNum)
    {
        WeaponNumber = weaponNum;
        SlotNumber = slotNum;
    }

    public void SetWeaponGem(WeaponGem weaponGem)
    {
        WeaponGem = weaponGem;
        gemImage.sprite = weaponGem.Sprite;
        gemImage.color = weaponGem.GetComponent<SpriteRenderer>().color;
    }

    public void ClearWeaponGem()
    {
        WeaponGem = null;
        gemImage.sprite = null;
        gemImage.color = Color.white;
    }
}
