using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCardEffect : Effect
{
    [SerializeField] private BoardEffect boardEffect;
    [SerializeField] private int EffectDuration;

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        AddEffectOnBoardGA addEffectOnBoardGA = new(
            targetGrid,
            boardEffect,
            EffectDuration
        );
        return addEffectOnBoardGA;
    }
}
