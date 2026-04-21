using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class UIManagerChess : MonoBehaviour
{

    public GameObject promotionPanel;
    private ChessBoardController chessBoardController;
    private Pawn promotingPawn;
    private CreateChessBoard createChessBoard;

    public void Start()
    {
        chessBoardController = FindFirstObjectByType<ChessBoardController>();
        createChessBoard = FindFirstObjectByType<CreateChessBoard>();
        promotionPanel.SetActive(false);
    }

    public void Show(Pawn pawn)
    {
        promotingPawn = pawn;
        promotionPanel.SetActive(true);
    }

    public void OnPromotePawn(string pieceName)
    {

        int prefabIndex = -1;

        switch (pieceName)
        {
            case "queen":
                prefabIndex = promotingPawn.IsWhite() ? 11 : 6;
                break;
            case "rook":
                prefabIndex = promotingPawn.IsWhite() ? 8 : 3;
                break;
            case "bishop":
                prefabIndex = promotingPawn.IsWhite() ? 10 : 5;
                break;
            case "knight":
                prefabIndex = promotingPawn.IsWhite() ? 9 : 4;
                break;
        }

        if (prefabIndex != -1)
        {
            string pawnPosition = promotingPawn.currentPosition;

            // Instantiate dan simpan
            createChessBoard.InstantiatePiece(
                createChessBoard.PiecePrefabs[prefabIndex], 
                pawnPosition
            );

            // Hapus pawn lama
            Destroy(promotingPawn.gameObject);
            promotionPanel.SetActive(false);

            // Baru update check map
            chessBoardController.UpdateCheckMap();

            chessBoardController.isPromotionActive = false;
        }
    }

}
