using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Database
{
    public class ModBase
    {
        public string Name { get; set; } = "Base";
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public byte Tier { get; set; }
        public float[] TierValues { get; set; } = Array.Empty<float>();
        public float[] TierValues2 { get; set; } = Array.Empty<float>();
        public float[] TierWeights { get; set; } = Array.Empty<float>();
        public ModTag[] ModTags { get; set; } = new ModTag[10];
        public bool IsLocal { get; set; }
        public Func<ModBase, string> Description { get; set; } = (m) => string.Empty;
        public Action<ModBase> ApplyMod { get; set; }
        public Action<ModBase> RemoveMod { get; set; }
        public void SetTier(byte tier) => Tier = tier;
        public virtual ModBase GetCopy() => new();
    }
}