using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnOfWarCardGA : GameAction
{

    public int[] directionX;
    public int[] directionY;
    public int duration;

    public PawnOfWarCardGA (int[] dirX, int[] dirY, int duration)
    {
        this.directionX = dirX;
        this.directionY = dirY;
        this.duration = duration;
    }
}
