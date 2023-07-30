using UnityEngine;

public class CH_DamageFilter : MonoBehaviour
{
	//-------FULL-ONHIT---------------------------
	public void OutgoingAttackHIT(DamageArgs damageArgs)
	{
		damageArgs.EnemyStats.OnHit.OnGettingAttackHit(damageArgs);

		damageArgs.ShooterStats.OnHit.OnGivingAttackHit(damageArgs);

		damageArgs.EnemyStats.Health.TakeDamage(damageArgs.Damage, damageArgs.IsCritical);
	}
	
	public void OutgoingSpellHIT(DamageArgs damageArgs)
	{
		damageArgs.EnemyStats.OnHit.OnGettingSpellHit(damageArgs);

		damageArgs.ShooterStats.OnHit.OnGivingSpellHit(damageArgs);

		damageArgs.EnemyStats.Health.TakeDamage(damageArgs.Damage, damageArgs.IsCritical);
	}
	//---------------------------------------------

	//------JUST-DAMAGE----------------------------
	public void OutgoingDAMAGE(DamageArgs damageArgs)
	{
		damageArgs.EnemyStats.Health.TakeDamage(damageArgs.Damage, damageArgs.IsCritical);
	}
	//---------------------------------------------

	//------JUST-ONHIT-EFFECTS---------------------
	public void OutgoingAttackEFFECT(DamageArgs damageArgs)
    {
		damageArgs.EnemyStats.OnHit.OnGettingAttackHit(damageArgs);

		damageArgs.ShooterStats.OnHit.OnGivingAttackHit(damageArgs);
	}

	public void OutgoingSpellEFFECT(DamageArgs damageArgs)
    {
		damageArgs.EnemyStats.OnHit.OnGettingSpellHit(damageArgs);

		damageArgs.ShooterStats.OnHit.OnGivingSpellHit(damageArgs);
	}
	//---------------------------------------------
}
