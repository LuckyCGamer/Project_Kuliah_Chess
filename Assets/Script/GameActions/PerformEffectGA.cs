using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformEffectGA : GameAction
{

    public Effect Effect { get; private set;}
    public Vector3Int targetGrid { get; private set; }

    public PerformEffectGA(Effect effect, Vector3Int gridTarget)
    {
        Effect = effect;
        targetGrid = gridTarget;
    }

}
