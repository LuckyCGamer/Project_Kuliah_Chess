using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{

    public override List<string> GetPotentialMoves()
    {
        List<string> potentialMoves = new List<String>();
        string currentPosition = this.currentPosition;

        // Check all 8 possible L-shaped moves
        int[] movesX = { -2, -1, 1, 2, 2, 1, -1, -2 };
        int[] movesY = { 1, 2, 2, 1, -1, -2, -2, -1 };

        for (int i = 0; i < movesX.Length; i++)
        {
            Movement = 0;
            int dx = movesX[i];
            int dy = movesY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx, dy);

            if (nextPosition != null)
            {
                GameObject pieceAtNextPosition = chessBoardController.GetChessPieceAtPosition(nextPosition);
                if (pieceAtNextPosition == null || pieceAtNextPosition.GetComponent<Piece>().pieceColor != this.pieceColor)
                {
                    BoardEffect effect = CurrentPieceBoardStatusEffect == null ? GetBoardStatusEffect(nextPosition) : CurrentPieceBoardStatusEffect;
                    
                    if(pieceAtNextPosition != null && effect != null)
                    {
                        BoardEffect attackPieceEffect = pieceAtNextPosition.GetComponent<Piece>().CurrentPieceBoardStatusEffect;
                        if (attackPieceEffect.isInvurnerable)
                        {
                            continue;
                        }
                    }

                    // check if movement under status effect
                    
                    if (effect != null)
                    {
                        Movement++;
                        //certain piece effect
                        if (effect.cannotMove.Contains(pieceType))
                        {
                            break;
                        }
                        potentialMoves.Add(nextPosition);
                        // Debug.Log($"the potential move is under effect : {Effect.name}");
                        if (Movement == effect.canMove)
                        {
                            break;
                        }
                    }
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
            Movement = 0;
            int dx = movesX[i];
            int dy = movesY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx, dy);

            if (nextPosition != null)
            {
                    // check if movement under status effect
                    BoardEffect effect = CurrentPieceBoardStatusEffect == null ? GetBoardStatusEffect(nextPosition) : CurrentPieceBoardStatusEffect;
                    if (effect != null)
                    {
                        Movement++;
                        //certain piece effect
                        foreach(string piece in effect.cannotMove)
                        {
                            if(piece == pieceType)
                            {
                                break;
                            }
                        }
                        attackedFields.Add(nextPosition);
                        // Debug.Log($"the potential move is under effect : {Effect.name}");
                        if (Movement == effect.canMove)
                        {
                            break;
                        }
                    }
                attackedFields.Add(nextPosition);
            }
        }

        return attackedFields;
    }

}
