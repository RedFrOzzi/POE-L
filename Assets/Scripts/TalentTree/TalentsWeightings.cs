using UnityEngine;

[CreateAssetMenu(fileName = "TalentsWeightings", menuName ="ScriptableObjects/Weights/TalentsWeights")]
public class TalentsWeightings : ScriptableObject
{
	[SerializeField] private float commonTalentWeight = 1;
	[SerializeField] private float uncommonTalentWeight = 1;
	[SerializeField] private float rareTalentWeight = 1;
	[SerializeField] private float epicTalentWeight = 1;
	[SerializeField] private float mythicTalentWeight = 1;
	[SerializeField] private float legendaryTalentWeight = 1;

	public float CommonTalentShowPercent { get; private set; }
	public float UncommonTalentShowPercent { get; private set; }
	public float RareTalentShowPercent { get; private set; }
	public float EpicTalentShowPercent { get; private set; }
	public float MythicTalentShowPercent { get; private set; }
	public float LegendaryTalentShowPercent { get; private set; }
	public void InitializeWeights()
    {
		var totalWeight = commonTalentWeight + uncommonTalentWeight + rareTalentWeight + epicTalentWeight + mythicTalentWeight + legendaryTalentWeight;

		CommonTalentShowPercent = commonTalentWeight / totalWeight;
		UncommonTalentShowPercent = uncommonTalentWeight / totalWeight;
		RareTalentShowPercent = rareTalentWeight / totalWeight;
		EpicTalentShowPercent = epicTalentWeight / totalWeight;
		MythicTalentShowPercent = mythicTalentWeight / totalWeight;
		LegendaryTalentShowPercent = legendaryTalentWeight / totalWeight;
	}
}
