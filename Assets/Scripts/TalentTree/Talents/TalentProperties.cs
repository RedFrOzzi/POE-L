using System;
using UnityEngine;

namespace Database
{
    public abstract class TalentProperties
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpriteName { get; set; }
        public Rarity TalentRarity { get; set; }

        public Action<CH_Stats> OnUnlock { get; set; }
        public Action<CH_Stats> OnLock { get; set; }
    }
}