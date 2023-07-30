using UnityEngine;

public interface IWeaponMod
{
	public IWeaponItem WeaponItem { get; set; }

	public void SetItem(IWeaponItem item) => WeaponItem = item;
}
