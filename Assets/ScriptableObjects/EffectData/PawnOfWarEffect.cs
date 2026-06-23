using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnOfWarEffect : Effect
{

    int[] directionsX = { -1, -1, -1, 0, 0, 1, 1, 1 };
    int[] directionsY = { -1, 0, 1, -1, 1, -1, 0, 1 };
    public int duration;

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        PawnOfWarCardGA pawnOfWarCardGA = new PawnOfWarCardGA(
            directionsX,
            directionsY,
            duration
        );
        return pawnOfWarCardGA;
    }
}
