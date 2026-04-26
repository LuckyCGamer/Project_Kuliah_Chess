using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{

    public override List<string> GetLegalMoves()
    {
        List<string> legalMoves = base.GetLegalMoves();

        if (HasMoved == 0 && !chessBoardController.CheckKingStatus())
        {
            if (canCastleKingSide())
            {
                legalMoves.Add(chessBoardController.GetChessPosition(currentPosition, 2, 0));
            }
            if (canCastleQueenSide())
            {
                legalMoves.Add(chessBoardController.GetChessPosition(currentPosition, -2, 0));
            }
        }

        return legalMoves;
    }

    protected override List<string> GetPotentialMoves()
    {
        List<string> potentialMoves = new List<string>();
        string currentPosition = this.currentPosition;

        // Check all 8 possible directions
        int[] directionsX = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] directionsY = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            int dx = directionsX[i];
            int dy = directionsY[i];
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

    public override void MoveToPosition(string newPosition)
    {

        string currentPosition = this.currentPosition;

        if (HasMoved == 0 && Mathf.Abs(newPosition[1] - currentPosition[1]) == 2)
        {
            
            char currentPositionHorizontal = currentPosition[0];
            if (newPosition[1] == '7')
            {
                Piece rook = chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}8").GetComponent<Piece>();
                // Debug.Log(rook);
                if (rook is Rook)
                {
                    rook.MoveToPosition($"{currentPositionHorizontal}6");
                }
            }
            else if (newPosition[1] == '3')
            {
                Piece rook = chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}1").GetComponent<Piece>();
                if (rook is Rook)
                {
                    rook.MoveToPosition($"{currentPositionHorizontal}4");
                }
            }
        }

        base.MoveToPosition(newPosition);
    }

    public override List<string> GetAttackedFields()
    {
        List<string> attackedFields = new List<string>();
        string currentPosition = this.currentPosition;

        // Check all 8 possible directions
        int[] directionsX = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] directionsY = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < directionsX.Length; i++)
        {
            int dx = directionsX[i];
            int dy = directionsY[i];
            string nextPosition = chessBoardController.GetChessPosition(currentPosition, dx , dy);

            if (nextPosition != null)
            {
                attackedFields.Add(nextPosition);
            }
        }

        return attackedFields;
    }

    public bool CheckForChecks()
    {
        string currentPosition = this.currentPosition;
        Dictionary<string, bool> opponentAttackedFields = IsWhite()
            ? chessBoardController.blackCheckMap
            : chessBoardController.whiteCheckMap; 

        bool isInCheck = opponentAttackedFields[currentPosition];
        return isInCheck;
    }

    private bool canCastleKingSide()
    {
        string currentPosition = this.currentPosition;
        char currentPositionHorizontal = currentPosition[0];
        Piece rook =
            chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}8")?.GetComponent<Piece>();

        if (rook is Rook && rook.HasMoved == 0)
        {
            Dictionary<string, bool> opponentAttackedFields = IsWhite()
                ? chessBoardController.blackCheckMap
                : chessBoardController.whiteCheckMap; 

            bool canCastle = opponentAttackedFields[$"{currentPositionHorizontal}6"] == false &&
                             opponentAttackedFields[$"{currentPositionHorizontal}7"] == false &&
                             chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}6") == null &&
                             chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}7") == null;
            return canCastle;
        }
        return false;
    }

    private bool canCastleQueenSide()
    {
        string currentPosition = this.currentPosition;
        char currentPositionHorizontal = currentPosition[0];
        Piece rook = 
            chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}1")?.GetComponent<Piece>();

        if (rook is Rook && rook.HasMoved == 0)
        {
            Dictionary<string, bool> opponentAttackedFields = IsWhite()
                ? chessBoardController.blackCheckMap
                : chessBoardController.whiteCheckMap; 

            bool canCastle = opponentAttackedFields[$"{currentPositionHorizontal}2"] == false &&
                             opponentAttackedFields[$"{currentPositionHorizontal}3"] == false &&
                             opponentAttackedFields[$"{currentPositionHorizontal}4"] == false &&
                             chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}2") == null &&
                             chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}3") == null &&
                             chessBoardController.GetChessPieceAtPosition($"{currentPositionHorizontal}4") == null;
            return canCastle;
        }
        return false;
    }

}
