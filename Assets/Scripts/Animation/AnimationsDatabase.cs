using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationsDatabase", menuName = "ScriptableObjects/AnimationsDatabase")]
public class AnimationsDatabase : ScriptableObject
{
	[SerializeField] private List<AnimationClip> animationClips = new();

	public Dictionary<string, AnimationClip> Animations { get; private set; } = new();
	public Dictionary<string, float> AnimationDurations { get; private set; } = new();

	public void Initialize()
    {
		animationClips = new(Resources.LoadAll<AnimationClip>("Animations"));

		if (animationClips.Count <= 0) { return; }

		foreach (var clip in animationClips)
        {
			if (Animations.ContainsValue(clip)) { continue; }

			string[] subStrings = clip.ToString().Split(' ');

			Animations.Add(subStrings[0], clip);

			AnimationDurations.Add(subStrings[0], clip.length);
        }
    }
}
