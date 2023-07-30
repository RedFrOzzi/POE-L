using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourExploder : EnemyBehaviour
{
    public override void Moving(Vector2 movingTo)
    {
        if (stats.IsControllable == false || stats.CanMove == false) { return; }

        if (agent.SetDestination(movingTo) == false) { return; }
    }
}
