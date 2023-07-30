using System;
using UnityEngine;

namespace Database
{
    public class CCondition
    {
        [field: SerializeField] public string Name { get; set; } = "Base";
        public byte Tier { get; set; } = 0; //4 тирa
        [field: SerializeField] public float[] TimeValues { get; set; } = Array.Empty<float>();
        [field: SerializeField] public int[] KillsValues { get; set; } = Array.Empty<int>();
        [field: SerializeField] public ModTag[] ModTags { get; set; } = new ModTag[5];
        
        public Func<CCondition, string> Description { get; set; }
        public Action<ChallengeConditionsManager, CCondition> SetCondition { get; set; }
    }

    [Serializable]
    public class ChallengeCondition : CCondition
    {
        public ChallengeCondition SetTier(byte tier)
        {
            Tier = tier;
            return this;
        }

        public ChallengeCondition GetCopy()
        {
            ChallengeCondition cc = new();
            cc.Name = Name;
            cc.Tier = Tier;

            if (TimeValues.Length > 0)
            {
                cc.TimeValues = new float[TimeValues.Length];
                for (int i = 0; i < TimeValues.Length; i++)
                {
                    cc.TimeValues[i] = TimeValues[i];
                }
            }

            if (KillsValues.Length > 0)
            {
                cc.KillsValues = new int[KillsValues.Length];
                for (int i = 0; i < KillsValues.Length; i++)
                {
                    cc.KillsValues[i] = KillsValues[i];
                }
            }

            if (ModTags.Length > 0)
            {
                cc.ModTags = new ModTag[ModTags.Length];
                for (int i = 0; i < ModTags.Length; i++)
                {
                    cc.ModTags[i] = ModTags[i];
                }
            }

            cc.Description = Description;
            cc.SetCondition = SetCondition;
            return cc;
        }
    }
}