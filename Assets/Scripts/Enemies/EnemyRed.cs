using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRed : EnemyController
{
    protected void Update()
    {
        if (!targetReached && target)
        {
            SelectClosestTarget();
            GoToTarget(target.position);
        }

    }
}
