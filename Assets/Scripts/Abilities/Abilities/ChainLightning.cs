using System.Collections.Generic;
using UnityEngine;


namespace Database
{
	public class ChainLightning : Ability
	{
        private readonly float[] DamagePerLevel;
        private readonly int[] ChainsAmountPerLevel;
        private const float endOfChainMultiplier = 1.2f;

        private const float maxRange = 10f;
        private const float seekRadius = 3;

        private readonly List<ChainLightningGameObject> activeGameObjects = new();

        public ChainLightning()
        {
            ID = AbilityID.ChainLightning;
            Name = "Chain Lightning";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Damage;
            Tags[1] = ModTag.Magic;
            Tags[2] = ModTag.Projectile;
            DamagePerLevel = new float[] { 10, 10, 20, 20, 20, 30, 30, 30, 40, 40, 40, 50, 50, 60, 60, 60, 60, 60, 60, 60, 60 };
            ChainsAmountPerLevel = new int[] { 1, 2, 2, 3, 3, 4, 4, 5, 5, 5, 6, 6, 6, 6, 7, 7, 7, 8, 8, 8, 8, 9 };

            CritChance = 8f;
            Cooldown = 1f;
            Manacost = 2;

            Tip = $"Lightning arc strikes enemy and chains on to other random enemies nearby";
        }
        public override string Description()
        {
            return $"Lightning arc strikes enemy and chains on to other random enemies nearby {ChainsAmountPerLevel[LevelClamp()]} more times." +
                $" Deals {DamagePerLevel[LevelClamp()]} damage";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            (Damage _damage, bool _isCritical) = Slot.GetAbilityDamage(new (0, DamagePerLevel[LevelClamp()], 0), CritChance, Tags);

            DamageArgs damageArgs = new(_damage, _isCritical, stats, null, DamageArgs.DamageSource.SingleTarget);

            GetNonActiveGameObject()
                .StartChainLightning(stats.transform.position, aim, damageArgs,
                    ChainsAmountPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + stats.GSC.MagicSC.FlatSpellProjectileAmountValue,
                    seekRadius, maxRange, endOfChainMultiplier);
        }

        private ChainLightningGameObject GetNonActiveGameObject()
        {
            foreach (var go in activeGameObjects)
            {
                if (go.IsActive == false)
                {
                    return go;
                }
            }

            var newChain = GameObject.Instantiate(MagicBehaviour.AbilitiesGameObjects["ChainLightningGameObject"]).GetComponent<ChainLightningGameObject>();
            activeGameObjects.Add(newChain);

            return newChain;
        }
    }
}