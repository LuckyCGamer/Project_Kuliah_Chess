using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChessBoardController : Singleton<ChessBoardController>
{

    public Dictionary<string, GameObject> chessBoardController = new Dictionary<string, GameObject>();
    public Dictionary<string, Vector3Int> boardGridLocation = new();
    public List<Piece> piecesOnBoard;
    public Dictionary<string, GameObject> HistoryPiecesOnBoard = new();
    public List<Dictionary<string, GameObject>> previousBoardState = new();
    public bool isWhiteTurn;
    public GameObject lastMovedPiece;
    public string lastMovedPieceStartPosition;
    public string lastMovedPieceEndPosition;
    public Dictionary<string, bool> whiteCheckMap = new();
    public Dictionary<string, bool> blackCheckMap = new();
    // BoardStats Effect Variable
    public Dictionary<AddEffectOnBoardGA, int> boardStatusEffect = new();
    public Dictionary<PawnOfWarCardGA, int> pawnOfWar = new();
    public GridData Temp = new();
    public bool isPromotionActive = false;
    private UIManagerChess UIManagerChess;
    [SerializeField] public Grid boardGrid;
    [SerializeField] PlacementSystem placementSystem;
    [SerializeField] GameObject Turn;
    [SerializeField] SwitchCamera switchCamera;
    [SerializeField] public bool skipTurn;
    private int CapturedSnapShot = 0;

    public void Start()
    {
        UIManagerChess = FindFirstObjectByType<UIManagerChess>();
    }

    public void Initialize()
    {
        isWhiteTurn = true;
        ChangeTurnText();
        ResetCheckMap();
        UpdatePiecesOnBoard();
        UpdateHistoryBoard();
    }

    public void EndTurn()
    {
        // Debug.Log("End Turn");
        ReduceDurationGA reduceDurationGA = new();
        ActionSystem.Instance.Perform(reduceDurationGA);
        isWhiteTurn = !isWhiteTurn;
        ChangeTurnText();
        CheckGameOver();
        SwitchOtherPlayer();

        if (CapturedSnapShot == 2)
        {
            UpdateHistoryBoard();
            CapturedSnapShot = 0;
        }

        if (skipTurn)
        {
            skipTurn = false;
            EndTurn();
        }

        CapturedSnapShot++;
    }

    public void SwitchOtherPlayer()
    {
        if (switchCamera.Manager == 1)
        {
            switchCamera.switchCamera(2);
        }
        else
        {
            switchCamera.switchCamera(1);
        }
    }

    public void ChangeTurnText()
    {
        // Debug.Log(Turn.GetComponent<TextMeshProUGUI>().text);
        if (isWhiteTurn)
        {
            Turn.GetComponent<TextMeshProUGUI>().text = "White Turn";
        }
        else
        {
            Turn.GetComponent<TextMeshProUGUI>().text = "Black Turn";
        }
    }

    public void UpdateAllChessPieceStatusEffect()
    {
        // foreach(var position in placementSystem.spaceData.placedObjects)
        // {
        //     Debug.Log($"{position.Key} : {position.Value}");
        // }
        foreach (Piece piece in piecesOnBoard)
        {
            piece.UpdateStatusEffect();
        }
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
        foreach (var position in chessBoardController)
        {
            if (position.Value != null)
            {
                piecesOnBoard.Add(position.Value.GetComponent<Piece>());
            }
        }
    }

    public void UpdateHistoryBoard()
    {
        HistoryPiecesOnBoard.Clear();
        HistoryPiecesOnBoard = new Dictionary<string, GameObject>(chessBoardController);

        if (previousBoardState.Count >= 2)
        {
            previousBoardState.RemoveAt(0);
        }

        previousBoardState.Add(new Dictionary<string, GameObject>(chessBoardController));
        
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
                string winner = isWhiteTurn ? "Black Win" : "White Win";
                // Debug.Log(winner);
                UIManagerChess.ShowGameOver(winner);
            }
        }
        else
        {
            UpdatePiecesOnBoard();
            List<Piece> piecesCopy = new List<Piece>(piecesOnBoard);

            int kingCount = 0;
            int knightCount = 0;
            int bishopCount = 0;
            int queenCount = 0;
            int rookCount = 0;


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
                else if (piece is Queen)
                {
                    queenCount++;
                }
                else if (piece is Rook)
                {
                    rookCount++;
                }
            }

            if (kingCount == 2 &&
                (knightCount == 0 && bishopCount == 0 && rookCount == 0 && queenCount == 0 ||
                knightCount == 1 && bishopCount == 0 ||
                knightCount == 0 && bishopCount == 1))
            {
                string result = "Draw insufficient piece";
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
                string result = "Draw no move";
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
