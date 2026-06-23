using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardGA : GameAction
{

    public Card Card{ get; private set;}
    public Vector3Int targetGrid { get; private set; }
    public Piece SelectedPiece { get; private set; }
    public PlayCardGA(Card card)
    {
        Card = card;
        targetGrid = Vector3Int.zero;
        SelectedPiece = null;
    }

    public PlayCardGA(Card card, Vector3Int gridTarget)
    {
        Card = card;
        targetGrid = gridTarget;
        SelectedPiece = null;
    }

    public PlayCardGA(Card card, Piece selectedPiece)
    {
        Card = card;
        targetGrid = Vector3Int.zero;
        SelectedPiece = selectedPiece;
    }

}
