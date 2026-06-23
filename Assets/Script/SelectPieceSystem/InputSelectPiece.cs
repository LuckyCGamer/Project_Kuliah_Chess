using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSelectPiece : MonoBehaviour
{
    public event Action OnClicked, OnExit;
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    [SerializeField] private LayerMask placementLayerMask;
    
    private Vector3 lastPosition;
    private Piece chessPieceSelected;

    // Card
    public Card selectedCard;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private GameObject cardView;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedCard != null){
            OnClicked?.Invoke();
            if (chessPieceSelected)
            {
                resetCard();
            }
            
        }
        if(Input.GetKeyDown(KeyCode.Escape) && selectedCard != null){
            OnExit?.Invoke();
            resetCard();
            cardView.SetActive(true);
            cardView.transform.position = originalPosition;
            cardView.transform.rotation = originalRotation;
        }
    }

    public Piece GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        bool isHit = Physics.Raycast(ray, out RaycastHit hitInfo, 100f, placementLayerMask);
        if (isHit && SelectChessPiece(hitInfo) && hitInfo.collider.gameObject.GetComponent<Piece>().IsWhite() == ChessBoardController.Instance.isWhiteTurn)
        {
            chessPieceSelected = hitInfo.collider.gameObject.GetComponent<Piece>();
        }
        // Debug.Log($"{chessPieceSelected.pieceType} {chessPieceSelected.pieceColor}");

        return chessPieceSelected;
    }

    private bool SelectChessPiece(RaycastHit hitInfo)
    {
        return hitInfo.collider.CompareTag("ChessPiece");
    }


    public void CardPlayed(Card card, Vector3 pos, Quaternion rotation, GameObject CardView)
    {
        selectedCard = card;
        originalPosition = pos;
        originalRotation = rotation;
        cardView = CardView;
    }

    public void resetCard()
    {
        selectedCard = null;
    }
}