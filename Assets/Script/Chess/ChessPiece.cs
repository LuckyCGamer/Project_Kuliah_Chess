using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chess Piece", menuName = "Data/Chess Piece")]
public class ChessPiece : ScriptableObject
{

    [SerializeField] public string pieceType;
    [SerializeField] public string pieceColor;
}
