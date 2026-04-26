using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{

    protected override List<string> GetPotentialMoves()
    {
        List<string> potentialMoves = new List<String>();
        string currentPosition = this.currentPosition;

        // Check horizontal, vertical, and diagonal directions
        int[] directionsX = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] directionsY = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            Movement = 0;
            int step = 1;
            int dx = directionsX[i];
            int dy = directionsY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx * step, dy * step);

            while (nextPosition != null)
            {

                // If there is chesspiece in next movement
                GameObject pieceAtNextPosition = chessBoardController.GetChessPieceAtPosition(nextPosition);
                if (pieceAtNextPosition == null)
                {

                    // check if movement under status effect
                    BoardEffect effect = CurrentPieceBoardStatusEffect == null ? GetBoardStatusEffect(nextPosition) : CurrentPieceBoardStatusEffect;
                    if (effect != null)
                    {
                        Movement++;
                        potentialMoves.Add(nextPosition);
                        // Debug.Log($"the potential move is under effect : {Effect.name}");
                        if (Movement == effect.canMove)
                        {
                            break;
                        }
                    }

                    potentialMoves.Add(nextPosition);
                }
                else
                {
                    if (pieceAtNextPosition.GetComponent<Piece>().pieceColor != this.pieceColor)
                    {
                        potentialMoves.Add(nextPosition);
                    }
                    break; // Stop searching in this direction after encountering a piece
                }
                step++;
                nextPosition = chessBoardController.GetChessPosition(currentPosition, dx * step, dy * step);

            }
        }

        return potentialMoves;
    }

    public override List<string> GetAttackedFields()
    {
        List<string> attackedFields = new List<String>();
        string currentPosition = this.currentPosition;

        // Check horizontal, vertical, and diagonal directions
        int[] directionsX = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] directionsY = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            int step = 1;
            Movement = 0;
            int dx = directionsX[i];
            int dy = directionsY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx * step, dy * step);

            while (nextPosition != null)
            {
                // check if movement under status effect
                BoardEffect effect = CurrentPieceBoardStatusEffect == null ? GetBoardStatusEffect(nextPosition) : CurrentPieceBoardStatusEffect;
                if (effect != null)
                {
                    Movement++;
                    attackedFields.Add(nextPosition);
                    // Debug.Log($"the potential move is under effect : {Effect.name}");
                    if (Movement == effect.canMove)
                    {
                        break;
                    }
                }

                attackedFields.Add(nextPosition);
                GameObject pieceAtNextPosition = chessBoardController.GetChessPieceAtPosition(nextPosition);
                if (pieceAtNextPosition != null)
                {
                    break; // Stop searching in this direction after encountering a piece
                }
                step++;
                nextPosition = chessBoardController.GetChessPosition(currentPosition, dx * step, dy * step);
            }
        }

        return attackedFields;
    }

}
