using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceEffectOnBoard : Effect
{
    [SerializeField] private BoardEffect boardEffect;
    [SerializeField] private int EffectDuration;

    public override GameAction GetGameAction(Vector3Int targetGrid)
    {
        AddEffectOnBoardGA addEffectOnBoardGA = new(
            targetGrid,
            boardEffect,
            EffectDuration
        );
        return addEffectOnBoardGA;
    }
}
