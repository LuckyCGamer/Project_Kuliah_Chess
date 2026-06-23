using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCardEffect : Effect
{

    [SerializeField] int moveAmount;

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        GravityCardGA gravityCardGA = new GravityCardGA(
            moveAmount
        );
        return gravityCardGA;
    }
}
