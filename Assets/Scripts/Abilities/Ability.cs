using UnityEngine;
using System;

namespace Database
{
    //Base class
    public class Ability
    {
        public AbilitySlot Slot { get; protected set; }
        public AbilityID ID { get; protected set; }
        public AbilityType AbilityType { get; protected set; }
        public string Name { get; protected set; }
        public Sprite Sprite { get; protected set; }
        public ModTag[] Tags { get; } = new ModTag[10];
        public int Level { get; protected set; }
        public float CritChance { get; protected set; }
        public float Cooldown { get; protected set; }
        public int Manacost { get; protected set; }
        public string Tip { get; protected set; }

        protected float originalCooldown;
        protected int timesCooldownHaveChanged;
        
        public Ability()
        {
            ID = AbilityID.None;
            Name = "name";
            Level = 0;
            CritChance = 0;
            Cooldown = 1;
            Manacost = 1;
        }

        protected int LevelClamp()
        {
            return Mathf.Clamp(Level + Slot.LSC.MagicSC.FlatAbilityLevelValue + Slot.Stats.GSC.MagicSC.FlatAbilityLevelValue, 0, 20);
        }
        public void SetAbilitySlot(AbilitySlot slot) => Slot = slot;
        public void SetBaseCooldown(float newCooldown)
        {
            if (timesCooldownHaveChanged == 0)
            {
                originalCooldown = Cooldown;
            }
            timesCooldownHaveChanged++;

            Cooldown = newCooldown;
        }
        public void SetBaseCooldownToOriginal() => Cooldown = originalCooldown;
        public virtual string Description() { return "none"; }
        public virtual void OnAbilityEquip(CH_Stats stats) { }
        public virtual void OnAbilityUnEquip(CH_Stats stats) { }
        public virtual void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted) { }
        public virtual void OnPeriodicCall(CH_Stats stats, float periodicCallCD) { return; }
        public virtual (bool isActivatable, string reason) CanBeActivated(CH_Stats stats) { return (true, string.Empty); }
    }
}
