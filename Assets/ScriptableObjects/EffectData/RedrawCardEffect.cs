using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedrawCardEffect : Effect
{

    public override GameAction GetGameAction(Vector3Int targetGrid, Piece selectedPiece)
    {
        RedrawCardGA redrawCardGA = new RedrawCardGA(

        );
        return redrawCardGA;
    }

}
