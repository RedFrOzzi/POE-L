using UnityEngine;

public interface IDamageEffect
{
	public CH_Stats GetEffectOwnerStats();
	public void AddToManagerList();
	public void RemoveFromManagerList();
	public (Collider2D[], Damage) TickDamage();
}
