using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessBoardController : MonoBehaviour
{

    public Dictionary<string, GameObject> chessBoardController = new Dictionary<string, GameObject>();
    public List<Piece> piecesOnBoard;
    public bool isWhiteTurn;
    public GameObject lastMovedPiece;
    public string lastMovedPieceStartPosition;
    public string lastMovedPieceEndPosition;
    public Dictionary<string, bool> whiteCheckMap = new Dictionary<string, bool>();
    public Dictionary<string, bool> blackCheckMap = new Dictionary<string, bool>();
    public bool isPromotionActive = false;
    private UIManagerChess UIManagerChess;

    public void Start()
    {
        UIManagerChess = FindFirstObjectByType<UIManagerChess>();
    }

    public void Initialize()
    {
        isWhiteTurn = true;
    }

    public void EndTurn()
    {
        // Debug.Log("End Turn");
        isWhiteTurn = !isWhiteTurn;
        CheckGameOver();
    }

    public void AddBoardData(string position, GameObject chessPiece)
    {
        if (!chessBoardController.ContainsKey(position))
        {
            chessBoardController.Add(position, chessPiece);
            // Debug.Log("BoardData added at position: " + position);
        }
        else
        {
            Debug.LogWarning("Position " + position + " is already occupied by another chess piece.");
        }
    }

    public void UpdateBoardData(string position, GameObject chessPiece)
    {
        if (chessBoardController.ContainsKey(position))
        {
            chessBoardController[position] = chessPiece;
            // Debug.Log($"{chessBoardController[position]}");
        }
        else
        {
            Debug.LogWarning("Position " + position + " does not exist in the board controller.");
        }
    }

    public void BoardDataNull(string position)
    {
        if (chessBoardController.ContainsKey(position))
        {
            chessBoardController[position] = null;
            // Debug.Log("BoardData removed at position: " + position);
        }
        else
        {
            Debug.LogWarning("Position " + position + " does not exist in the board controller.");
        }
    }

    public GameObject GetChessPieceAtPosition(string position)
    {
        if (chessBoardController.ContainsKey(position))
        {
            return chessBoardController[position];
        }
        else
        {
            Debug.LogWarning("Position " + position + " does not exist in the board controller.");
            return null;
        }
    }

    public string GetChessPosition(string position, int horizontal, int vertical)
    {
        List<char> chessPieces = new() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
        
        char currentHorizontal = position[0];
        int currentVertical = int.Parse(position[1].ToString());
        int newHorizontalIndex = chessPieces.IndexOf(currentHorizontal) + vertical;
        int newVertical = currentVertical + horizontal;
        if (newHorizontalIndex < 0 || newHorizontalIndex >= chessPieces.Count || newVertical < 1 || newVertical > 8)
        {
            return null; // Return null if the new position is out of bounds
        }

        string newPosition = chessPieces[newHorizontalIndex].ToString() + newVertical.ToString();
        
        return newPosition;
    }

    private void UpdatePiecesOnBoard()
    {
        piecesOnBoard.Clear();
        foreach (var position in chessBoardController){
            if (position.Value != null)
            {
                piecesOnBoard.Add(position.Value.GetComponent<Piece>());
            }
        }
    }

    private void ResetCheckMap()
    {
        foreach (var position in chessBoardController)
        {
            whiteCheckMap[position.Key] = false;
            blackCheckMap[position.Key] = false;
        }
    }

    public void UpdateCheckMap()
    {
        ResetCheckMap();
        UpdatePiecesOnBoard();

        // Debug.Log($"{piecesOnBoard.Count} pieces on board to update check map.");
        foreach (Piece piece in piecesOnBoard)
        {
            if (piece != null)
            {
                List<string> attackedFields = piece.GetAttackedFields();
                foreach (string attackedField in attackedFields)
                {
                    if (piece.IsWhite())
                    {
                        // Debug.Log($"Updating check map for {piece.pieceColor} {piece.pieceType} at position {piece.currentPosition}");
                        whiteCheckMap[attackedField] = true;
                    }
                    else
                    {
                        blackCheckMap[attackedField] = true;
                    }
                }
            }
        }
    }

    public bool CheckKingStatus()
    {
        King king = null;

        foreach (var position in chessBoardController)
        {
            if (position.Value == null) continue;
            Piece piece = position.Value.GetComponent<Piece>();
            if (piece.pieceType == "king" && piece.IsWhite() == isWhiteTurn)
            {
                king = (King)piece;
                break;
            }

            if (king != null)
            {
                break;
            }
        }

        if (king != null)
        {
            // Debug.Log($"Checking king status at position: {king.currentPosition}");
            if (king.CheckForChecks())
            {
                // Debug.Log($"{king.pieceColor} King is in check after the move!");
                return true;
            }
            else
            {
                // Debug.Log($"{king.pieceColor} King is not in check after the move.");
                return false;
            }
        } 
        else
        {
            return false;
        }

    }

    public void CheckGameOver()
    {
        if (CheckKingStatus())
        {
            UpdatePiecesOnBoard();
            List<Piece> piecesCopy = new List<Piece>(piecesOnBoard);
            bool hasValidMoves = false;

            foreach (Piece piece in piecesCopy)
            {
                if (piece != null && piece.IsWhite() == isWhiteTurn)
                {
                    if (piece.GetLegalMoves().Count > 0)
                    {
                        hasValidMoves = true;
                        break;
                    }
                }
            }

            if (!hasValidMoves)
            {
                string winner = isWhiteTurn ? "Black Wins" : "White Wins";
                Debug.Log(winner);
            }
        }
        else
        {
            UpdatePiecesOnBoard();
            List<Piece> piecesCopy = new List<Piece>(piecesOnBoard);

            int kingCount = 0;
            int knightCount = 0;
            int bishopCount = 0;

            foreach (Piece piece in piecesCopy)
            {
                if (piece is King)
                {
                    kingCount++;
                }
                else if (piece is Knight)
                {
                    knightCount++;
                }
                else if (piece is Bishop)
                {
                    bishopCount++;
                }
            }

            if (kingCount == 2 && (knightCount == 0 && bishopCount == 0 || knightCount == 1 && bishopCount == 0 || knightCount == 0 && bishopCount == 1))
            {
                string result = "Draw";
                Debug.Log(result);
                return;
            }

            bool hasValidMoves = false;
            foreach (Piece piece in piecesCopy)
            {
                if (piece != null && piece.IsWhite() == isWhiteTurn)
                {
                    if (piece.GetLegalMoves().Count > 0)
                    {
                        hasValidMoves = true;
                        break;
                    }
                }
            }

            if (!hasValidMoves)
            {
                string result = "Draw";
                Debug.Log(result);
            }

        }
    }

    public void HandlePromotion(Pawn pawn)
    {
        isPromotionActive = true;
        UIManagerChess.Show(pawn);
    }

}
