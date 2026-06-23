using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructEffect : Effect
{

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        SelfDestructCardGA selfDestructCardGA = new(
            selectedPiece
        );
        return selfDestructCardGA;
    }
}
