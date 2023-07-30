using System;

namespace Database
{
    public class Buff
    {
        public string GeneratedID { get; protected set; } = Guid.NewGuid().ToString();
        public bool ShouldShow { get; protected set; } = true;
        public bool IsPositive { get; protected set; } = true;
        public bool CanHaveMultipleInstances { get; protected set; } = false;
        public string Name { get; protected set; } = "Name";
        public ModTag[] Tags { get; protected set; } = new ModTag[10];
        public float Duration { get; protected set; } = float.MaxValue;
        public float ExpireTime { get; protected set; } = 0f;
        public CH_Stats OwnerStats { get; protected set; }
        public CH_Stats SourceStats { get; protected set; }
        public virtual string Description() => string.Empty;
        public Buff SetName(string name)
        {
            Name = name;
            return this;
        }
        public Buff SetDuration(float duration)
        {
            Duration = duration;
            return this;
        }
        public void SetExpireTime(float time) => ExpireTime = time;
        public void SetOwnerStats(CH_Stats stats) => OwnerStats = stats;
        public void SetSourseStats(CH_Stats stats) => SourceStats = stats;
        public void ApplyBuff(CH_BuffManager buffManager, CH_Stats sourceStats) => buffManager.ApplyBuff(this, sourceStats);

        public virtual void OnBuffApplication() { return; }
        public virtual void OnBuffTick(float tickDalay) { return; }
        public virtual void OnBuffExpire() { return; }
    }
}
