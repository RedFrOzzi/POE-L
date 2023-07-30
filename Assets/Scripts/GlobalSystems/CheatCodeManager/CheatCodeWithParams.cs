using System;
using UnityEngine;


public class CheatCodeWithParams : CheatCodeBase
{
	protected Action<float[]> cheatCommand;

	public void ApplyCommand(params float[] parameters)
	{
		cheatCommand?.Invoke(parameters);
	}
}