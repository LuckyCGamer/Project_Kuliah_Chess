using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    
    public override List<string> GetPotentialMoves()
    {
        List<string> potentialMoves = new List<String>();
        string currentPosition = this.currentPosition;

        // Check horizontal and vertical directions
        int[] directionsX = { -1, 1, 0, 0 };
        int[] directionsY = { 0, 0, -1, 1 };

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
                        
                        if(attackPieceEffect != null)
                        {
                            if (attackPieceEffect.isInvurnerable)
                            {
                                break;
                            }                            
                        }
                        potentialMoves.Add(nextPosition);
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

        // Check horizontal and vertical directions
        int[] directionsX = { -1, 1, 0, 0 };
        int[] directionsY = { 0, 0, -1, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            Movement = 0;
            int dx = directionsX[i];
            int dy = directionsY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx, dy);

            while (nextPosition != null)
            {
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
