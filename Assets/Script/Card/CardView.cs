using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardView : MonoBehaviour
{

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    public int playerHand = 0;
    public Card Card { get; private set;}
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    private PlacementSystem placementSystem;
    private InputManager InputManager;
    private SelectionState selectionState;
    private InputSelectPiece inputSelectPiece;
    private DropArea dropArea;
    private SwitchCamera SwitchCamera;


    public void Awake()
    {
        placementSystem = FindFirstObjectByType<PlacementSystem>();
        InputManager = FindFirstObjectByType<InputManager>();
        selectionState = FindAnyObjectByType<SelectionState>();
        inputSelectPiece = FindAnyObjectByType<InputSelectPiece>();
        SwitchCamera = FindAnyObjectByType<SwitchCamera>();
    }

    public void Start()
    {
        dropArea = FindFirstObjectByType<DropArea>();
    }

    public void Setup (Card card, int PlayerHand)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        imageSR.sprite = card.Image;
        playerHand = PlayerHand;
    }

    void OnMouseEnter()
    {
        float zoffset = 0.05f;
        if(!Interactions.Instance.PlayerCanHover()) return;
        if(!Interactions.Instance.IsPlayerCard(playerHand)) return;

        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, transform.position.y + 0.3f, transform.position.z + zoffset);
        
        // Hover card rotation
        Quaternion cardRotation = transform.rotation;
        Quaternion rotation = Quaternion.Euler(45, 0, 0);

        CardViewHoverSystem.Instance.Show(Card, pos, cardRotation, playerHand);
    }

    void OnMouseExit()
    {
        if(!Interactions.Instance.PlayerCanHover()) return;
        if(!Interactions.Instance.IsPlayerCard(playerHand)) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    void OnMouseDown()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        if(!Interactions.Instance.IsPlayerCard(playerHand)) return;
        Interactions.Instance.PlayerIsDragging = true;
        wrapper.SetActive(true);
        CardViewHoverSystem.Instance.Hide();

        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;

        transform.position = MouseUtil.GetMousePositionInWorldSpace(transform.position);
    }

    void OnMouseDrag()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        if(!Interactions.Instance.IsPlayerCard(playerHand)) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(transform.position);
    }

    void OnMouseUp()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        if(!Interactions.Instance.IsPlayerCard(playerHand)) return;
        
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
    

        if(Physics.Raycast(ray, out RaycastHit hit, 100f, dropLayer))
        {
            // Play a card
            if (Card.IsPlacement)
            {
                placementSystem.StartPlacement(Card.IDGrid);
                gameObject.SetActive(false);
                InputManager.CardPlayed(Card, dragStartPosition, dragStartRotation, gameObject);
            }
            else if (Card.IsSelectPiece)
            {
                selectionState.StartSelection();
                gameObject.SetActive(false);
                inputSelectPiece.CardPlayed(Card, dragStartPosition, dragStartRotation, gameObject);

            }
            else
            {
                PlayCardGA playCardGA = new(Card, Vector3Int.zero);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else
        {
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
        }
        Interactions.Instance.PlayerIsDragging = false;
    }

}
