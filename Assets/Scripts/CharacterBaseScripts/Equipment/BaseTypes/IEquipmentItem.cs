using System;
using UnityEngine;

public interface IEquipmentItem
{
	public string Name { get; set; }
	public string ID { get; set; }
	public string Description { get; set; }
	public EquipmentSlot EquipmentSlot { get; set; }
	public EquipmentType EquipmentType { get; set; }
	public Equipment Equipment { get; set; }
	public SignatureMod SignatureMod {get; set;}
	public Sprite Sprite { get; set; }
	public ModsHolder ModsHolder { get; set; }
	public Action OnEquipAction { get; set; }
	public Action OnUnEquipAction { get; set; }

	public EquipmentType GetEquipmentType();

	public void Initialize();
}
