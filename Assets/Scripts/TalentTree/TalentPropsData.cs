using Database;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TalentPropsData", menuName = "ScriptableObjects/TalentPropsData")]
public class TalentPropsData : ScriptableObject
{
	[ContextMenuItem("checkCount", "CheckCount")]
	public string checkCount = "Right click to check for list items count";

	public List<TProp> Props { get; private set; } = new();
	public Dictionary<string, TProp> PropsDictionary { get; private set; } = new();

	public void AddRange(IEnumerable<TProp> props)
	{
		Props.AddRange(props);

		foreach(var prop in props)
		{
			PropsDictionary.Add(prop.Name, prop);
        }
    }

	public void Clear()
	{
		Props.Clear();
		PropsDictionary.Clear();
    }

	private void CheckCount()
	{
		Debug.Log(Props.Count);
	}
}
