using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshFilter))]
public class Pawn : Piece
{

    public override List<string> GetPotentialMoves()
    {
        List<string> potentialMoves = new List<String>();
        string currentPosition = this.currentPosition;
        int direction = IsWhite() ? -1 : 1;
        Movement = 0;

        if(additionalPotentialMove.Count != 0)
        {
            potentialMoves.AddRange(additionalPotentialMove);
        }
        // Debug.Log(additionalPotentialMove.Count);

        if (chessBoardController.lastMovedPiece != null && chessBoardController.lastMovedPiece.GetComponent<Piece>().pieceType == "pawn")
        {
            string lastMovedPieceStartPosition = chessBoardController.lastMovedPieceStartPosition;
            string lastMovedPieceEndPosition = chessBoardController.lastMovedPieceEndPosition;
            if (Math.Abs(lastMovedPieceStartPosition[0] - lastMovedPieceEndPosition[0]) == 2)
            {
                if (lastMovedPieceEndPosition[0] == currentPosition[0] &&
                    Math.Abs(lastMovedPieceEndPosition[1] - currentPosition[1]) == 1)
                {
                    string enPassantPosition = chessBoardController.GetChessPosition(lastMovedPieceEndPosition, 0, 1 * direction);
                    potentialMoves.Add(enPassantPosition);
                }
            }
        }

        string forwardMove = chessBoardController.GetChessPosition(currentPosition, 0, 1 * direction);
        if (forwardMove != null && chessBoardController.GetChessPieceAtPosition(forwardMove) == null)
        {

            // check if movement under status effect
            BoardEffect effect = CurrentPieceBoardStatusEffect == null ? GetBoardStatusEffect(forwardMove) : CurrentPieceBoardStatusEffect;
            if (effect != null)
            {
                Movement++;
                // Debug.Log($"the potential move is under effect : {Effect.name}");
            }

            potentialMoves.Add(forwardMove);

            if (HasMoved == 0)
            {
                string doubleForwardMove = chessBoardController.GetChessPosition(currentPosition, 0, 2 * direction);
                if (doubleForwardMove != null && chessBoardController.GetChessPieceAtPosition(doubleForwardMove) == null)
                {
                    if(effect != null)
                    {
                        if (Movement != effect.canMove)
                        {
                            potentialMoves.Add(doubleForwardMove);
                        }                
                    }
                    else
                    {
                        potentialMoves.Add(doubleForwardMove);
                    }
                    
                }
            }
        }

        string leftAttackMove = chessBoardController.GetChessPosition(currentPosition, -1, 1 * direction);
        if (leftAttackMove != null &&
        chessBoardController.GetChessPieceAtPosition(leftAttackMove) != null &&
        chessBoardController.GetChessPieceAtPosition(leftAttackMove).GetComponent<Piece>().IsWhite() != IsWhite())
        {
            BoardEffect attackPieceEffect = chessBoardController.GetChessPieceAtPosition(leftAttackMove).GetComponent<Piece>().CurrentPieceBoardStatusEffect;
            if( attackPieceEffect != null)
            {
                if (!attackPieceEffect.isInvurnerable)
                {
                    potentialMoves.Add(leftAttackMove);
                }
            }
            else
            {
                potentialMoves.Add(leftAttackMove);
            }
            
        }

        string rightAttackMove = chessBoardController.GetChessPosition(currentPosition, 1, 1 * direction);
        if (rightAttackMove != null &&
        chessBoardController.GetChessPieceAtPosition(rightAttackMove) != null &&
        chessBoardController.GetChessPieceAtPosition(rightAttackMove).GetComponent<Piece>().IsWhite() != IsWhite())
        {
            
            BoardEffect attackPieceEffect = chessBoardController.GetChessPieceAtPosition(rightAttackMove).GetComponent<Piece>().CurrentPieceBoardStatusEffect;
            if( attackPieceEffect != null)
            {
                if (!attackPieceEffect.isInvurnerable)
                {
                    potentialMoves.Add(rightAttackMove);
                }
            }
            else
            {
                potentialMoves.Add(rightAttackMove);
            }
            
        }

        return potentialMoves;
    }

    public override List<string> GetAttackedFields()
    {
        List<string> attackedFields = new List<String>();
        string currentPosition = this.currentPosition;
        int direction = IsWhite() ? -1 : 1;

        string leftAttackMove = chessBoardController.GetChessPosition(currentPosition, -1, 1 * direction);
        if (leftAttackMove != null)
        {
            attackedFields.Add(leftAttackMove);
        }

        string rightAttackMove = chessBoardController.GetChessPosition(currentPosition, 1, 1 * direction);
        if (rightAttackMove != null)
        {
            attackedFields.Add(rightAttackMove);
        }

        return attackedFields;
    }

    public override void MoveToPosition(string newPosition)
    {
        // handle en passant capture
        newPosition = newPosition.Replace("Highlight_", "");
        if (Mathf.Abs(newPosition[0] - currentPosition[0]) == 1 &&
            Mathf.Abs(newPosition[1] - currentPosition[1]) == 1 &&
            chessBoardController.GetChessPieceAtPosition(newPosition) == null
            )
        {
            GameObject targetPiece = chessBoardController.GetChessPieceAtPosition(chessBoardController.GetChessPosition(newPosition, 0, 1 * (IsWhite() ? 1 : -1)));
            Piece targetPawn = targetPiece.GetComponent<Piece>();
            if (targetPiece != null && targetPawn.pieceType == "pawn")
            {
                chessBoardController.BoardDataNull(targetPawn.currentPosition);
                Destroy(targetPiece);
            }
        }

        // Implement pawn-specific movement logic here (e.g., first move, en passant)
        base.MoveToPosition(newPosition);

        // Check for promotion
        if (IsWhite() && currentPosition[0] == 'A' || !IsWhite() && currentPosition[0] == 'H')
        {
            chessBoardController.HandlePromotion(this);
        }
    }

}
