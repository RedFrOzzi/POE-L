using System.Collections.Generic;
using UnityEngine;

namespace Database {
	[CreateAssetMenu(fileName = "TypeSpecificMods", menuName = "ScriptableObjects/ModsDatabase/TypeSpecificMods")]
	public class TypeSpecificMods : ScriptableObject
	{
		private readonly Dictionary<EquipmentType, ItemSpecificMods> itemSpecificMods = new();

		[SerializeField] private List<ItemSpecificMods> itemModsScriptableObjects = new();

		public void InitializeDatabase()
		{
			var ism = Resources.LoadAll<ItemSpecificMods>("ModsDatabase/ItemSpecificMods");

			itemSpecificMods.Clear();
			itemModsScriptableObjects.Clear();

			foreach (var obj in ism)
			{
				if (itemSpecificMods.ContainsValue(obj)) { continue; }

				itemSpecificMods.Add(obj.TypeOfEquipment, obj);

				obj.InitializeDatabase();

				itemModsScriptableObjects.Add(obj);
			}
		}

		public ModBase GetWeightedMod(EquipmentType equipmentType, string[] exceptModNames, ModType modType)
        {
			if (itemSpecificMods.ContainsKey(equipmentType) == false)
            {
				Debug.Log($"List of mods does not contain {equipmentType} key");
				return ModsDatabase.EmptyMod;
            }

			return itemSpecificMods[equipmentType].GetWeightedMod(exceptModNames, modType);
		}

		public (ModBase, ModType) GetWeightedModWithTag(EquipmentType equipmentType, string[] exceptModNames, ModTag tag)
		{
			if (itemSpecificMods.ContainsKey(equipmentType) == false)
			{
				Debug.Log($"List of mods does not contain {equipmentType} key");
				return (ModsDatabase.EmptyMod, ModType.None);
			}

			return itemSpecificMods[equipmentType].GetWeightedModWithTag(exceptModNames, tag);
		}

		public (ModBase, ModType) GetRandomModWithTag(EquipmentType equipmentType, string[] exceptModNames, ModTag tag)
        {
			if (itemSpecificMods.ContainsKey(equipmentType) == false)
			{
				Debug.Log($"List of mods does not contain {equipmentType} key");
				return (ModsDatabase.EmptyMod, ModType.None);
			}

			return itemSpecificMods[equipmentType].GetRandomModWithTag(exceptModNames, tag);
		}

		public (string, ModType) GetRandomModNameInCurrentWithTag(EquipmentType equipmentType, string[] currentModNames, ModTag tag)
		{
			if (itemSpecificMods.ContainsKey(equipmentType) == false)
			{
				Debug.Log($"List of mods does not contain {equipmentType} key");
				return ("EmptyMod", ModType.None);
			}

			return itemSpecificMods[equipmentType].GetRandomModNameInCurrentWithTag(currentModNames, tag);
		}
	}
}