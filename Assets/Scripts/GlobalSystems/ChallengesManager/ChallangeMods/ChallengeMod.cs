using System;
using UnityEngine;

namespace Database
{
    public abstract class CMod
    {
        [field: SerializeField] public string Name { get; set; } = "Base";
        public byte Tier { get; set; } = 0;
        [field: SerializeField] public float[] TierValues { get; set; } = Array.Empty<float>();
        [field: SerializeField] public ModTag[] ModTags { get; set; } = new ModTag[10];
        public Func<CMod, string> Description { get; set; }
        public Func<byte, float> GetChallangeValue { get; set; }
        public Action<CH_Stats, GlobalEnemyModifiers> ApplyMod { get; set; }
        public Action<CH_Stats, GlobalEnemyModifiers> RemoveMod { get; set; }
        
    }

    [Serializable]
    public class ChallengeMod : CMod
    {
        public ChallengeMod SetTier(byte tier)
        {
            Tier = tier;
            return this;
        }

        public float GetHightestChallengeValue() => TierValues == null ? 0 : GetChallangeValue.Invoke(0);

        public ChallengeMod GetCopy()
        {
            ChallengeMod cm = new();
            cm.Name = Name;
            cm.Tier = Tier;

            if (TierValues.Length > 0)
            {
                cm.TierValues = new float[TierValues.Length];
                for (int i = 0; i < TierValues.Length; i++)
                {
                    cm.TierValues[i] = TierValues[i];
                }
            }

            if (ModTags.Length > 0)
            {
                cm.ModTags = new ModTag[ModTags.Length];
                for (int i = 0; i < ModTags.Length; i++)
                {
                    cm.ModTags[i] = ModTags[i];
                }
            }

            cm.Description = Description;
            cm.GetChallangeValue = GetChallangeValue;
            cm.ApplyMod = ApplyMod;
            cm.RemoveMod = RemoveMod;
            return cm;
        }
    }
}
