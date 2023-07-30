using UnityEngine;

namespace Database
{
    public class DashAbility : Ability
    {
        private readonly float distance;

        public DashAbility()
        {
            ID = AbilityID.Dash;
            Name = "Dash";
            AbilityType = AbilityType.Activatable;
            Tags[0] = ModTag.Movement;
            Tags[1] = ModTag.Utility;
            distance = 3;
            Cooldown = 2;
            Manacost = 2;

            Tip = $"Dash towards mouse position";
        }

        public override string Description()
        {
            return $"Dash towards mouse position for {distance} units. Dash distance depends on current movement speed";
        }

        public override void OnAbilityActivation(CH_Stats stats, Vector2 aim, bool isAutocasted)
        {
            ObjectTrailRenderer.Instance.PlayTrail(stats.SpriteRenderer, stats.transform, Vector3.one, stats.AdditionalEffects.DashTime, false);

            stats.AdditionalEffects.Dash(aim - (Vector2)stats.transform.position, distance * stats.CurrentMovementSpeed);
        }
    }
}
