using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{

    public override List<string> GetPotentialMoves()
    {

        List<string> potentialMoves = new List<String>();
        string currentPosition = this.currentPosition;

        // Check diagonal directions
        int[] directionsX = { -1, -1, 1, 1 };
        int[] directionsY = { -1, 1, -1, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            Movement = 0;
            int dx = directionsX[i];
            int dy = directionsY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx, dy);

            while (nextPosition != null)
            {
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
                        BoardEffect attackPieceEffect = pieceAtNextPosition.GetComponent<Piece>().CurrentPieceBoardStatusEffect;
                        if (!attackPieceEffect.isInvurnerable)
                        {
                            potentialMoves.Add(nextPosition);
                        }
                    }
                    break; // Stop searching in this direction after encountering a piece
                }
                nextPosition = chessBoardController.GetChessPosition(nextPosition, dx, dy);
            }
        }

        return potentialMoves;
    }

    public override List<string> GetAttackedFields()
    {
        List<string> attackedFields = new List<String>();
        string currentPosition = this.currentPosition;

        // Check diagonal directions
        int[] directionsX = { -1, -1, 1, 1 };
        int[] directionsY = { -1, 1, -1, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            Movement = 0;
            int dx = directionsX[i];
            int dy = directionsY[i];

            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx, dy);

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
                nextPosition = chessBoardController.GetChessPosition(nextPosition, dx, dy);
            }
        }
        return attackedFields;
    }

}
