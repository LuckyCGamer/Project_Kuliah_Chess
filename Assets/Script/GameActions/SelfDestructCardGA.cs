using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructCardGA : GameAction
{

    public Piece selectedPiece;

    public SelfDestructCardGA (Piece selectedPiece)
    {
        this.selectedPiece = selectedPiece;
    }
}
