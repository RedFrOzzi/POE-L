using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
	public class Shuriken : Ability
	{
        private readonly float[] DamageOnHitPerLevel;
        private readonly float[] BleedDPSPercentPerLevel;
        private readonly byte[] ShurikensAmountPerLevel;
        private readonly byte[] ShurikenReturnTimes;

        private const byte bleedDuration = 4;

        private const float projSpeed = 15f;
        private const float projLifeTime = float.MaxValue;

        private const float cdOnFailAmountCheck = 1f;

        private int activeShurikens;

        public Shuriken()
        {
            ID = AbilityID.Shuriken;
            Name = "Shuriken";
            AbilityType = AbilityType.Automatic;
            Tags[0] = ModTag.Damage;
            Tags[1] = ModTag.Physical;
            Tags[2] = ModTag.DamageOverTime;
            Tags[3] = ModTag.Projectile;
            Tags[4] = ModTag.Duration;
            DamageOnHitPerLevel = new float[] { 10, 10, 20, 20, 20, 30, 30, 30, 40, 40, 40, 50, 50, 60, 60, 60, 60, 60, 60, 60, 60 };
            ShurikensAmountPerLevel = new byte[] { 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6 };
            ShurikenReturnTimes = new byte[] { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6 };
            BleedDPSPercentPerLevel = new float[] { 0.2f, 0.3f, 0.3f, 0.4f, 0.4f, 0.4f, 0.5f, 0.5f, 0.5f, 0.5f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.7f, 0.7f, 0.7f, 0.8f, 0.8f, 0.9f, 0.8f };

            CritChance = 8f;
            Cooldown = 4;
            Manacost = 2;

            Tip = $"Throw shuriken in random direction. Shuriken deals physical damage on impact and causing target to bleed for {bleedDuration} duration.";
        }
        public override string Description()
        {
            return $"Throw shuriken in random direction. Shuriken deals {Slot.GetAbilityDamage(new Damage(DamageOnHitPerLevel[LevelClamp()], 0, 0), 0, Tags).damage.Physical}" +
                $" damage on impact and causing target to bleed for {bleedDuration} duration." +
                $" Bleed damage is {BleedDPSPercentPerLevel[LevelClamp()] * 100}% out of impact damage";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            if (isAutocasted)
                Autocast(stats);
            else
                ManualCast(stats, aim);
        }

        public override (bool isActivatable, string reason) CanBeActivated(CH_Stats stats)
        {
            if (activeShurikens < ShurikensAmountPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue)
            {
                return (true, "");
            }
            else
            {
                Slot.Stats.AbilitiesManager.PutAbilityOnCooldown(Slot.SlotIndex, cdOnFailAmountCheck);

                return (false, "Max amount of shurikens");
            }
        }

        private void Autocast(CH_Stats stats)
        {
            ProjectileCreationArgs projectileArgs = new(MagicBehaviour.AbilitiesGameObjects["ShurikenGameObject"], new Damage(DamageOnHitPerLevel[LevelClamp()], 0, 0),
                CritChance, projSpeed, projLifeTime, ShurikensAmountPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue);

            ShurikenThrowAuto(stats, Slot, projectileArgs, ShurikenOnHit);
        }

        private void ManualCast(CH_Stats stats, Vector2 aim)
        {
            ProjectileCreationArgs projectileArgs = new(MagicBehaviour.AbilitiesGameObjects["ShurikenGameObject"], new Damage(DamageOnHitPerLevel[LevelClamp()], 0, 0),
                CritChance, projSpeed, projLifeTime, ShurikensAmountPerLevel[LevelClamp()] + Slot.LSC.MagicSC.FlatSpellProjectileAmountValue + Slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue);

            ShurikenThrowManual(stats, Slot, projectileArgs, aim, ShurikenOnHit);
        }

        private void ShurikenThrowAuto(CH_Stats stats, AbilitySlot slot, ProjectileCreationArgs projectileArgs, Action<BaseProjectile, Collider2D> projBehavior)
        {
            ShurikenGameObject shurikenGameObject;

            (var _damage, var _isCritical) = Slot.GetAbilityDamage(projectileArgs.Damage, projectileArgs.BaseCritChance, Tags);

            for (int i = 0; i < projectileArgs.ProjectilesAmount + slot.LSC.MagicSC.FlatSpellProjectileAmountValue + slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue; i++)
            {
                //проверка чтоб не выпускать дополнительные прожектайлы
                if (activeShurikens >= projectileArgs.ProjectilesAmount + slot.LSC.MagicSC.FlatSpellProjectileAmountValue + slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue) { return; }


                GameObject go = GameObject.Instantiate(projectileArgs.Projectile, stats.transform.position, Quaternion.identity, MagicBehaviour.ProjectileParentObject.transform);

                activeShurikens++;

                shurikenGameObject = go.GetComponent<ShurikenGameObject>();
                var spriteRenderer = go.GetComponent<SpriteRenderer>();
                go.GetComponent<SimpleAnimation>().GetState("Default").speed = 2;


                //VISUALS
                ObjectTrailRenderer.Instance.PlayTrail(spriteRenderer, go.transform, Vector3.one);
                //-------

                go.layer = MagicBehaviour.PlayerProjectileLayer;

                //angle---
                go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0, 360))) * Vector2.right);
                //--------

                shurikenGameObject.ShurikenSkill = this;
                shurikenGameObject.AmountOfReturningsLeft = ShurikenReturnTimes[LevelClamp()];

                shurikenGameObject.ShooterStats = stats;

                shurikenGameObject.ProjectileBehavior = projBehavior;
                shurikenGameObject.ChainsLeft = projectileArgs.ChainsAmount;
                shurikenGameObject.PierceLeft = projectileArgs.PierceAmount;

                shurikenGameObject.ProjDestroyTime = Time.time + projectileArgs.Range;
                shurikenGameObject.ProjSpeed = projectileArgs.Speed;
                shurikenGameObject.ProjIndex = WeaponBehaviour.IndexOfProjectiles;
                shurikenGameObject.IsMultipleProjWork = false;
                shurikenGameObject.LayerMask = MagicBehaviour.EnemyLayerMask;
                shurikenGameObject.ShooterInitialPosition = stats.transform.position;
                shurikenGameObject.ImpactAnimation = "BloodBig_01";
                shurikenGameObject.ImpactAnimationRotation = -90;
                shurikenGameObject.ImpactAnimationScale = Vector3.one;

                shurikenGameObject.Damage = _damage;
                shurikenGameObject.IsCritical = _isCritical;
            }

            WeaponBehaviour.IndexOfProjectiles++;
        }


        private void ShurikenThrowManual(CH_Stats stats, AbilitySlot slot, ProjectileCreationArgs projectileArgs, Vector2 aim, Action<BaseProjectile, Collider2D> projBehavior)
        {
            Vector2 firstAngle = new();
            ShurikenGameObject shurikenGameObject;

            (var _damage, var _isCritical) = Slot.GetAbilityDamage(projectileArgs.Damage, projectileArgs.BaseCritChance, Tags);

            for (int i = 0; i < projectileArgs.ProjectilesAmount + slot.LSC.MagicSC.FlatSpellProjectileAmountValue + slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue; i++)
            {
                //проверка чтоб не выпускать дополнительные прожектайлы
                if (activeShurikens >= projectileArgs.ProjectilesAmount + slot.LSC.MagicSC.FlatSpellProjectileAmountValue + slot.Stats.GSC.MagicSC.FlatSpellProjectileAmountValue) { return; }


                GameObject go = GameObject.Instantiate(projectileArgs.Projectile, stats.transform.position, Quaternion.identity, MagicBehaviour.ProjectileParentObject.transform);

                activeShurikens++;

                shurikenGameObject = go.GetComponent<ShurikenGameObject>();
                var spriteRenderer = go.GetComponent<SpriteRenderer>();
                go.GetComponent<SimpleAnimation>().GetState("Default").speed = 2;


                //VISUALS
                ObjectTrailRenderer.Instance.PlayTrail(spriteRenderer, go.transform, Vector3.one);

                go.layer = MagicBehaviour.PlayerProjectileLayer;

                if (i == 0)
                {
                    firstAngle = (aim - (Vector2)stats.transform.position).normalized;

                    go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90)) * firstAngle);
                }
                else if (i % 2 == 0)
                {
                    go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * 2)) * firstAngle);
                }
                else
                {
                    go.transform.rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.Euler(new Vector3(0, 0, 90 + i * -2)) * firstAngle);
                }

                shurikenGameObject.ShurikenSkill = this;
                shurikenGameObject.AmountOfReturningsLeft = ShurikenReturnTimes[LevelClamp()];

                shurikenGameObject.ShooterStats = stats;

                shurikenGameObject.ProjectileBehavior = projBehavior;
                shurikenGameObject.ChainsLeft = projectileArgs.ChainsAmount;
                shurikenGameObject.PierceLeft = projectileArgs.PierceAmount;

                shurikenGameObject.ProjDestroyTime = Time.time + projectileArgs.Range;
                shurikenGameObject.ProjSpeed = projectileArgs.Speed;
                shurikenGameObject.ProjIndex = WeaponBehaviour.IndexOfProjectiles;
                shurikenGameObject.IsMultipleProjWork = false;
                shurikenGameObject.LayerMask = MagicBehaviour.EnemyLayerMask;
                shurikenGameObject.ShooterInitialPosition = stats.transform.position;
                shurikenGameObject.ImpactAnimation = "BloodBig_01";
                shurikenGameObject.ImpactAnimationRotation = -90;
                shurikenGameObject.ImpactAnimationScale = Vector3.one;

                shurikenGameObject.Damage = _damage;
                shurikenGameObject.IsCritical = _isCritical;
            }

            WeaponBehaviour.IndexOfProjectiles++;
        }

        private void ShurikenOnHit(BaseProjectile b, Collider2D c)
        {
            if (c.TryGetComponent(out CH_OnHit onHit))
            {
                DebuffBleed buff = new();
                buff.SetDPS(Slot.GetAbilityPerSecDOTDamage(b.Damage * BleedDPSPercentPerLevel[LevelClamp()], Tags).Physical)
                    .SetDuration(Slot.GetAbilityDuration(bleedDuration))
                    .ApplyBuff(onHit.OwnerBuffManager, b.ShooterStats);
            }
        }

        public void RemoveActiveShuriken()
        {
            activeShurikens--;
        }
    }
}