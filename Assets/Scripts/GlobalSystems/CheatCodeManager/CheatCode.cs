using System;

public class CheatCode : CheatCodeBase
{
	protected Action cheatCommand;

	public void ApplyCommand()
    {
		cheatCommand?.Invoke();
    }
}
