using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseTimeCardEffect : Effect
{

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        ReverseTimeGA reverseTimeCardGA = new ReverseTimeGA(
            
        );
        return reverseTimeCardGA;
    }
}
