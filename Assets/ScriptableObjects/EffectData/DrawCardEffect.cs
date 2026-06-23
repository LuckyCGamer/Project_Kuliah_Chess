using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardEffect : Effect
{
    [SerializeField] private int amount;
    [SerializeField] private string player;

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        DrawCardGA drawCardGA = new(
            amount,
            player
        );
        return drawCardGA;
    }
}
