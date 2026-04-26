using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChessBoard : MonoBehaviour
{

    [SerializeField] GameObject BoardPlane;
    [SerializeField] GameObject CoordinatePlane;
    [SerializeField] GameObject ParentObject;
    [SerializeField] private GameObject ParentPieces;
    [SerializeField] private ChessBoardController chessBoardController;
    [SerializeField] public GameObject[] PiecePrefabs;
    private int boardSize = 8;
    private readonly List<char> chessPieces = new() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
    
    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
        PlaceStartingPosition();
        chessBoardController.Initialize();
    }

    void CreateBoard()
    {
        
        for (int j = 0; j < chessPieces.Count; j++)
        {
            for (int i = 0; i < boardSize; i++)
            {
                GameObject newPlane = Instantiate(
                    CoordinatePlane, 
                    ParentObject.transform
                );

                Renderer planeRenderer = newPlane.GetComponent<Renderer>();

                newPlane.transform.position = new Vector3(
                    BoardPlane.transform.position.x - (BoardPlane.transform.localScale.x / 2) + planeRenderer.bounds.size.x / 2 + (i * planeRenderer.bounds.size.x), 
                    BoardPlane.transform.position.y,
                    BoardPlane.transform.position.z + (BoardPlane.transform.localScale.z / 2) - planeRenderer.bounds.size.z / 2 - (j * planeRenderer.bounds.size.z)
                );
                newPlane.name = $"{chessPieces[j]}{i + 1}";
                newPlane.tag = "PositionBoard";
                chessBoardController.AddBoardData($"{chessPieces[j]}{i + 1}", null);
                chessBoardController.boardGridLocation.Add($"{chessPieces[j]}{i + 1}", chessBoardController.boardGrid.WorldToCell(newPlane.transform.position) - new Vector3Int(0,0,1));
                // Debug.Log(chessBoardController.boardGridLocation[$"{chessPieces[j]}{i + 1}"]);
            }
        }
    }

    public void PlaceStartingPosition()
    {

        normalChessPiecePosition();

    }

    public void normalChessPiecePosition()
    {
        // Black and White Pawns
        for (int i = 0; i < boardSize; i++)
        {
            InstantiatePiece(PiecePrefabs[0], $"B{i + 1}");
            InstantiatePiece(PiecePrefabs[1], $"G{i + 1}");
        }

        // Kings Black
        InstantiatePiece(PiecePrefabs[2], "A5");

        //Black Rooks
        InstantiatePiece(PiecePrefabs[3], "A1");
        InstantiatePiece(PiecePrefabs[3], "A8");

        // Black Knights
        InstantiatePiece(PiecePrefabs[4], "A2");
        InstantiatePiece(PiecePrefabs[4], "A7");

        // Black Bishops
        InstantiatePiece(PiecePrefabs[5], "A3");
        InstantiatePiece(PiecePrefabs[5], "A6");

        // Black Queen
        InstantiatePiece(PiecePrefabs[6], "A4");

        //King White
        InstantiatePiece(PiecePrefabs[7], "H5");

        // White Rooks
        InstantiatePiece(PiecePrefabs[8], "H1");
        InstantiatePiece(PiecePrefabs[8], "H8");

        // White Knights
        InstantiatePiece(PiecePrefabs[9], "H2");
        InstantiatePiece(PiecePrefabs[9], "H7");

        // White Bishops
        InstantiatePiece(PiecePrefabs[10], "H3");
        InstantiatePiece(PiecePrefabs[10], "H6");

        // White Queen
        InstantiatePiece(PiecePrefabs[11], "H4");     
    }

    public void InstantiatePiece(GameObject chessPiecePrefab, string position)
    {
        if (chessBoardController != null)
        {
            GameObject positionBoard = GameObject.Find(position);
            if (positionBoard != null)
            {
                GameObject newChessPiece = Instantiate(chessPiecePrefab, positionBoard.transform.position, Quaternion.identity, ParentPieces.transform);
                newChessPiece.name = chessPiecePrefab.name;
                newChessPiece.tag = "ChessPiece";
                
                Piece piece = newChessPiece.GetComponent<Piece>();
                if (piece != null)
                {
                    piece.LoadPieceData();
                    piece.currentPosition = position;
                }

                chessBoardController.UpdateBoardData(position, newChessPiece);
            }
            else
            {
                Debug.LogError("Position board not found: " + position);
            }
        }
        else
        {
            Debug.LogError("ChessBoardController reference is missing.");
        }
    }
}
