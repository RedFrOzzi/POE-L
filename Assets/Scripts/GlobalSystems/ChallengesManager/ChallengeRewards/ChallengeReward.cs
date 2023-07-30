using System;
using UnityEngine;

namespace Database
{
    public class CReward
    {
        [field: SerializeField] public string Name { get; set; } = "Base";
        public byte Tier { get; set; } = 0; //4 тирa
        [field: SerializeField] public float[] TierValues { get; set; } = Array.Empty<float>();
        [field: SerializeField] public ModTag[] ModTags { get; set; } = new ModTag[10];
        public Func<CReward, string> Description { get; set; }
        public Action<CH_Stats, CReward> GiveReward { get; set; }
    }

    [Serializable]
    public class ChallengeReward : CReward
    {
        public ChallengeReward SetTier(byte tier)
        {
            Tier = tier;
            return this;
        }

        public ChallengeReward GetCopy()
        {
            ChallengeReward cr = new();

            cr.Name = Name;
            cr.Tier = Tier;
            
            if (TierValues.Length > 0)
            {
                cr.TierValues = new float[TierValues.Length];
                for (int i = 0; i < TierValues.Length; i++)
                {
                    cr.TierValues[i] = TierValues[i];
                }
            }

            if (ModTags.Length > 0)
            {
                cr.ModTags = new ModTag[ModTags.Length];
                for (int i = 0; i < ModTags.Length; i++)
                {
                    cr.ModTags[i] = ModTags[i];
                }
            }

            cr.Description = Description;
            cr.GiveReward = GiveReward;
            return cr;
        }
    }
}