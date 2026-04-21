using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    [SerializeField] Camera playerCamera;
    private GameObject ChessPieceSelected;
    [SerializeField] private GameObject HighLightObject;
    [SerializeField] public ChessBoardController chessBoardController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chessBoardController = FindFirstObjectByType<ChessBoardController>();
    }

    // Update is called once per frame
    void Update()
    {
        RayCastClickManager();
    }

    private void RayCastClickManager()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = playerCamera.ScreenPointToRay(mousePos);

        bool isHit = Physics.Raycast(ray, out RaycastHit hitInfo);

        if (!isHit) return;

        if (Input.GetMouseButtonDown(0))
        {
            
            if (ClickSameChessPiece(hitInfo))
            {
                UnSelectChessPiece();
            }
            else if (SelectChessPiece(hitInfo))
            {

                if (ChessPieceSelected != null)
                {
                    if (ChessPieceSelected.GetComponent<Piece>().GetLegalMoves().Contains(hitInfo.collider.gameObject.GetComponent<Piece>().currentPosition) && 
                        chessBoardController.isWhiteTurn != hitInfo.collider.gameObject.GetComponent<Piece>().IsWhite())
                    {
                        MoveCapturedPiece(ChessPieceSelected.GetComponent<Piece>(), hitInfo.collider.gameObject.GetComponent<Piece>());
                        return;
                    }
                    UnSelectChessPiece();
                }
                ChessPieceSelected = hitInfo.collider.gameObject;
                Piece chessPiece = ChessPieceSelected.GetComponent<Piece>();
                // Debug.Log(chessBoardController.GetChessPieceAtPosition(chessPiece.currentPosition));

                if ((chessPiece.IsWhite() && chessBoardController.isWhiteTurn) || 
                    (!chessPiece.IsWhite() && !chessBoardController.isWhiteTurn))
                {
                    chessPiece.SelectPiece();
                    SelectPiece(chessPiece);   
                }

            }
            else if (ClickLegalMove(hitInfo) && ChessPieceSelected != null)
            {
                string newPosition = hitInfo.collider.gameObject.name;
                MovePiece(ChessPieceSelected.GetComponent<Piece>(), newPosition);
            }
            else if(ChessPieceSelected != null)
            {                
                UnSelectChessPiece();
            }
        }
    }

    void SelectPiece(Piece chessPiece)
    {
        SpawnHighlightLegalMoves(chessPiece.GetLegalMoves());
    }

    void SaveLastMoveData(Piece chessPiece, string startPosition, string endPosition)
    {
        chessBoardController.lastMovedPiece = chessPiece.gameObject;
        chessBoardController.lastMovedPieceStartPosition = startPosition;
        chessBoardController.lastMovedPieceEndPosition = endPosition;
    }

    void MovePiece(Piece chessPiece, string HighLightPosition)
    {
        string newPosition = HighLightPosition.Replace("Highlight_", "");
        SaveLastMoveData(chessPiece, chessPiece.currentPosition, newPosition);
        chessPiece.MoveToPosition(newPosition);
        ChangeTurn();
    }

    void MoveCapturedPiece(Piece chessPiece, Piece targetPiece)
    {
        // Debug.Log(chessBoardController.GetChessPieceAtPosition(targetPiece.currentPosition));
        SaveLastMoveData(chessPiece, chessPiece.currentPosition, targetPiece.currentPosition);
        chessPiece.MoveToPosition(targetPiece.currentPosition);
        ChangeTurn();
    }

    void ChangeTurn()
    {
        UnSelectChessPiece();
        ClearHighlightLegalMoves();
        chessBoardController.UpdateCheckMap();
        chessBoardController.EndTurn();
    }

    void SpawnHighlightLegalMoves(List<string> legalMoves)
    {
        // Implement logic to spawn highlight objects on the board for each legal move
        foreach (string legalMove in legalMoves)
        {
            // Example: Instantiate a highlight prefab at the position corresponding to the move
            // GameObject highlight = Instantiate(highlightPrefab, chessBoardController.GetPositionFromMove(move), Quaternion.identity);
            // Debug.Log($"Highlight legal move: {legalMove}");
            GameObject positionBoard = GameObject.Find(legalMove);
            if (positionBoard != null)
            {
                GameObject HighLight = Instantiate(HighLightObject, positionBoard.transform.position, Quaternion.identity);
                HighLight.name = $"Highlight_{legalMove}";
            }
        }
    }

    void ClearHighlightLegalMoves()
    {
        // Implement logic to clear all highlight objects from the board
        GameObject[] highlights = GameObject.FindGameObjectsWithTag("HighLight");
        foreach (GameObject highlight in highlights)
        {
            Destroy(highlight);
        } 
    }

    private bool SelectChessPiece(RaycastHit hitInfo)
    {
        return hitInfo.collider.CompareTag("ChessPiece");
    }

    private bool ClickSameChessPiece(RaycastHit hitInfo)
    {
        return hitInfo.collider.CompareTag("ChessPiece") && ChessPieceSelected == hitInfo.collider.gameObject;
    }

    private bool ClickLegalMove(RaycastHit hitInfo)
    {
        return hitInfo.collider.CompareTag("HighLight");
    }

    private void UnSelectChessPiece()
    {
        ChessPieceSelected.GetComponent<Piece>().DeselectPiece();
        ClearHighlightLegalMoves();
        ChessPieceSelected = null;
    }

    // Debug purpose only, to visualize in the scene view
    private void OnDrawGizmos()
    {
        Vector3 mousePos = Input.mousePosition;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.ScreenPointToRay(mousePos).direction * 15f);
    }

}
