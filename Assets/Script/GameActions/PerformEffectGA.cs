using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformEffectGA : GameAction
{

    public Effect Effect { get; private set;}
    public Vector3Int targetGrid { get; private set; }
    public Piece SelectedPiece { get; private set; }

    public PerformEffectGA(Effect effect)
    {
        Effect = effect;
        targetGrid = Vector3Int.zero;
        SelectedPiece = null;
    }

    public PerformEffectGA(Effect effect, Vector3Int gridTarget)
    {
        Effect = effect;
        targetGrid = gridTarget;
        SelectedPiece = null;
    }

    public PerformEffectGA(Effect effect, Piece selectedPiece)
    {
        Effect = effect;
        targetGrid = Vector3Int.zero;
        SelectedPiece = selectedPiece;
    }

}
