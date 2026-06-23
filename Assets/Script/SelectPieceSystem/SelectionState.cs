using UnityEngine;

public class SelectionState : MonoBehaviour {

    [SerializeField] private InputSelectPiece inputSelectPiece;
    [SerializeField] private GameObject GameManager;
    [SerializeField] private GameObject CancelPlacement;
    
    public bool isSelectingPiece = false;

    private void Start()
    {
        StopSelectionPiece();
    }

    public void StartSelection()
    {
        StopSelectionPiece();
        inputSelectPiece.OnClicked += SelectPiece;
        inputSelectPiece.OnExit += StopSelectionPiece;
        CancelPlacement.SetActive(true);
        GameManager.SetActive(false);
        isSelectingPiece = true;
    }

    private void SelectPiece()
    {
        if (inputSelectPiece.IsPointerOverUI())
        {
            return;
        }
        Piece selectedPiece = inputSelectPiece.GetSelectedMapPosition();

        if(selectedPiece != null)
        {
            PlayCardGA playCardGA = new(inputSelectPiece.selectedCard, selectedPiece);
            ActionSystem.Instance.Perform(playCardGA);
            StopSelectionPiece();
        }
    }

    private void StopSelectionPiece()
    {
        inputSelectPiece.OnClicked -= SelectPiece;
        inputSelectPiece.OnExit -= StopSelectionPiece;
        CancelPlacement.SetActive(false);
        GameManager.SetActive(true);
        isSelectingPiece = false;
    }
}