using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;

public class ModForAbilityGem : ModBase
{
    public AbilityGem AbilityGem { get; protected set; }
    public void SetAbilityGem(AbilityGem abilityGem) => AbilityGem = abilityGem;

    public override ModBase GetCopy()
    {
        ModForAbilityGem m = new();
        m.Name = Name;
        m.ID = ID;
        m.Tier = Tier;
        m.IsLocal = IsLocal;
        m.Description = Description;
        m.ApplyMod = ApplyMod;
        m.RemoveMod = RemoveMod;
        m.AbilityGem = AbilityGem;

        m.TierValues = new float[TierValues.Length];
        for (int i = 0; i < TierValues.Length; i++)
        {
            m.TierValues[i] = TierValues[i];
        }

        m.TierValues2 = new float[TierValues2.Length];
        for (int i = 0; i < TierValues2.Length; i++)
        {
            m.TierValues2[i] = TierValues2[i];
        }

        m.TierWeights = new float[TierWeights.Length];
        for (int i = 0; i < TierWeights.Length; i++)
        {
            m.TierWeights[i] = TierWeights[i];
        }

        m.ModTags = new ModTag[ModTags.Length];
        for (int i = 0; i < ModTags.Length; i++)
        {
            m.ModTags[i] = ModTags[i];
        }

        return m;
    }
}
