using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public abstract class Effect 
{
    public abstract GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece);

}
