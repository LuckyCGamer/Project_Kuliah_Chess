using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class Piece : MonoBehaviour
{
    [SerializeField] private ChessPiece chessPieceData;
    [SerializeField] MeshRenderer pieceMeshRenderer;
    [SerializeField] public ChessBoardController chessBoardController;
    public String pieceType;
    public String pieceColor;
    public String currentPosition;
    public int HasMoved;
    protected abstract List<String> GetPotentialMoves();
    public abstract List<String> GetAttackedFields();

    public virtual List<String> GetLegalMoves()
    {
        List<String> legalMoves = new List<String>();
        foreach (String move in GetPotentialMoves())
        {
            if (WillMoveEndCheck(move))
            {
                legalMoves.Add(move);
            }
        }
        // Debug.Log($"Legal moves for {pieceType}: {legalMoves.Count}");
        return legalMoves;
    }

    protected bool WillMoveEndCheck(string targetPosition)
    {
        Piece originalPieceAtTarget = chessBoardController.GetChessPieceAtPosition(targetPosition)?.GetComponent<Piece>();
        string originalPosition = currentPosition;

        // Simulate the move
        chessBoardController.BoardDataNull(originalPosition);
        chessBoardController.UpdateBoardData(targetPosition, this.gameObject);
        currentPosition = targetPosition;
        chessBoardController.UpdateCheckMap();

        bool isKingInCheck = chessBoardController.CheckKingStatus();
        if (this is King)
        {
            isKingInCheck = IsWhite() ? chessBoardController.blackCheckMap[targetPosition] : 
                                        chessBoardController.whiteCheckMap[targetPosition];
        }

        // Revert the move
        if (originalPieceAtTarget != null)
        {
            chessBoardController.UpdateBoardData(targetPosition, originalPieceAtTarget.gameObject);
        }
        else
        {
            chessBoardController.BoardDataNull(targetPosition);
        }
        chessBoardController.UpdateBoardData(originalPosition, this.gameObject);
        currentPosition = originalPosition;
        chessBoardController.UpdateCheckMap();

        return !isKingInCheck;
    }

    public virtual void MoveToPosition(string newPosition)
    {
        // Implement movement logic here (e.g., update position, animate movement)
        // Debug.Log($"Moving {pieceColor} {pieceType} to {newPosition}");
        transform.position = GameObject.Find(newPosition).transform.position;
        chessBoardController.BoardDataNull(currentPosition);
        CapturePiece(newPosition);
        currentPosition = newPosition;
        HasMoved = 1;
        chessBoardController.UpdateBoardData(newPosition, this.gameObject);
    }

    public void CapturePiece(string capturePosition)
    {
        // Implement logic for capturing the piece (e.g., remove from board, play animation)
        GameObject targetPiece = chessBoardController.GetChessPieceAtPosition(capturePosition);
        if (targetPiece != null)
        {
            Destroy(targetPiece);
        }
        chessBoardController.BoardDataNull(capturePosition);
    }

    public void Awake()
    {
        chessBoardController = FindFirstObjectByType<ChessBoardController>();
        if (chessBoardController == null)
        {
            Debug.LogError("ChessBoardController not found in scene. Assign it manually or add a ChessBoardController component.");
        }
    }

    public void Start()
    {
        LoadPieceData();
    }

    public void LoadPieceData()
    {
        pieceType = chessPieceData.pieceType;
        pieceColor = chessPieceData.pieceColor;
        HasMoved = 0;
    }

    public void SelectPiece()
    {
        // Implement visual feedback for selecting the piece (e.g., highlight)
        pieceMeshRenderer.material.color = Color.green; 
        // Debug.Log($"Selected {pieceColor} {pieceType}");
    }

    public void DeselectPiece()
    {
        // Implement visual feedback for deselecting the piece (e.g., remove highlight)
        pieceMeshRenderer.material.color = Color.white;
        // Debug.Log($"Deselected {pieceColor} {pieceType}");
    }

    public bool IsWhite()
    {
        if (pieceColor == "white")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
