using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{

    protected override List<string> GetPotentialMoves()
    {
        List<string> potentialMoves = new List<String>();
        string currentPosition = this.currentPosition;

        // Check all 8 possible L-shaped moves
        int[] movesX = { -2, -1, 1, 2, 2, 1, -1, -2 };
        int[] movesY = { 1, 2, 2, 1, -1, -2, -2, -1 };

        for (int i = 0; i < movesX.Length; i++)
        {
            int dx = movesX[i];
            int dy = movesY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx, dy);

            if (nextPosition != null)
            {
                GameObject pieceAtNextPosition = chessBoardController.GetChessPieceAtPosition(nextPosition);
                if (pieceAtNextPosition == null || pieceAtNextPosition.GetComponent<Piece>().pieceColor != this.pieceColor)
                {
                    potentialMoves.Add(nextPosition);
                }
            }
        }

        return potentialMoves;
    }

    public override List<string> GetAttackedFields()
    {
        List<string> attackedFields = new List<String>();
        string currentPosition = this.currentPosition;

        // Check all 8 possible L-shaped moves
        int[] movesX = { -2, -1, 1, 2, 2, 1, -1, -2 };
        int[] movesY = { 1, 2, 2, 1, -1, -2, -2, -1 };

        for (int i = 0; i < movesX.Length; i++)
        {
            int dx = movesX[i];
            int dy = movesY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx, dy);

            if (nextPosition != null)
            {
                attackedFields.Add(nextPosition);
            }
        }

        return attackedFields;
    }

}
