using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCardGA : GameAction
{

    public int moveAmount { get; private set;}

    public GravityCardGA (int moveAmount)
    {
        this.moveAmount = moveAmount;
    }

}
