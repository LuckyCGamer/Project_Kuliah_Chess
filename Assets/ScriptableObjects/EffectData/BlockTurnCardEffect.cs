using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTurnCardEffect : Effect
{

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        BlockTurnCardGA blockTurnCardGA = new(

        );
        return blockTurnCardGA;
    }
}
