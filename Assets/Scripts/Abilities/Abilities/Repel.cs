using System;
using UnityEngine;

namespace Database
{
	public class Repel : Ability
	{
        private readonly float[] DamagePercentReturnPerLevel;
        private readonly int[] AddedBaseArmorPerLevel;

        private string generatingOnHitID;
        private const byte numberOfAnimations = 5;
        private const float animSpeedMulti = 2f;

        private readonly AnimationObject[] animationObjects = new AnimationObject[numberOfAnimations];

        public Repel()
        {
            ID = AbilityID.Repel;
            Name = "Repel";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Defance;
            Tags[1] = ModTag.Magic;
            Tags[2] = ModTag.Damage;
            DamagePercentReturnPerLevel = new float[] { 20, 20, 25, 25, 25, 30, 30, 30, 30, 35, 35, 35, 40, 40, 40, 45, 45, 50, 50, 70, 70 };
            AddedBaseArmorPerLevel = new int[] { 50, 50, 50, 50, 100, 100, 100, 100, 150, 150, 150, 150, 200, 200, 200, 200, 250, 250, 250, 250, 400 };

            CritChance = 8f;
            Cooldown = float.MaxValue;
            Manacost = 2;

            Tip = "Return premitigated damage back to attackers On Hit. Adds flat armor to player.";
        }
        public override string Description()
        {
            return $"Return {DamagePercentReturnPerLevel[LevelClamp()]}% of the premitigated damage back to attackers On Hit." +
                $" Adds {AddedBaseArmorPerLevel[LevelClamp()]} to base armor rate.";
        }

        public override void OnAbilityEquip(CH_Stats stats)
        {
            ApplyBuff(stats);

            Visuals();

            stats.Health.OnDamageTake += OnTakingDamage;

            stats.Equipment.OnEquipmentChange += UpdateAbilityStats;

            stats.AbilitiesManager.AbilitiesEquipment.OnAbilityGemChange += UpdateAbilityStats;
        }

        public override void OnAbilityUnEquip(CH_Stats stats)
        {
            RemoveBuff(stats);

            stats.Health.OnDamageTake -= OnTakingDamage;

            stats.Equipment.OnEquipmentChange -= UpdateAbilityStats;

            stats.AbilitiesManager.AbilitiesEquipment.OnAbilityGemChange -= UpdateAbilityStats;
        }

        private void UpdateAbilityStats(EquipmentItem a, EquipmentItem b)
        {
            RemoveBuff(Slot.Stats);

            ApplyBuff(Slot.Stats);
        }

        private void UpdateAbilityStats(byte abilityIndex, byte b, AbilityGem gem)
        {
            if (abilityIndex == Slot.SlotIndex)
            {
                RemoveBuff(Slot.Stats);

                ApplyBuff(Slot.Stats);
            }
        }

        private void OnTakingDamage(Damage _)
        {
            if (animationObjects[numberOfAnimations - 1].isActiveAndEnabled) { return; }

            Visuals();
        }

        private void ApplyBuff(CH_Stats stats)
        {
            stats.GSC.DefanceSC.AddFlatArmor(AddedBaseArmorPerLevel[LevelClamp()]);

            var onHitEffect = new RepelEffect
            {
                DamagePercentRepel = DamagePercentReturnPerLevel[LevelClamp()],
                GeneratedID = Guid.NewGuid().ToString()
            };

            generatingOnHitID = onHitEffect.GeneratedID;

            stats.OnHit.AddOnHit_GettingHit(onHitEffect);
        }

        private void RemoveBuff(CH_Stats stats)
        {
            stats.GSC.DefanceSC.AddFlatArmor(-AddedBaseArmorPerLevel[LevelClamp()]);

            stats.OnHit.RemoveOnHit_GettingHit(generatingOnHitID);
        }

        private void Visuals()
        {
            for (byte i = 0; i < numberOfAnimations; i++)
            {
                UtilityDelayFunctions.RunWithDelay((index) =>
                {
                    animationObjects[index] = AnimationPlayer.Instance.PlayAndFollowForDuration("FireCircleAround_01", Slot.Stats.transform,
                        Quaternion.identity, Vector3.one, Color.white, AnimationPlayer.Instance.AnimationsDurations["FireCircleAround_01"] / animSpeedMulti, animSpeedMulti);
                }
                , i * (AnimationPlayer.Instance.AnimationsDurations["FireCircleAround_01"] / (numberOfAnimations * animSpeedMulti)), i);
            }
        }
    }
}
