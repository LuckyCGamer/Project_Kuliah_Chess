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
    public Card Card { get; private set;}
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    private PlacementSystem placementSystem;
    private InputManager InputManager;
    private DropArea dropArea;


    public void Awake()
    {
        placementSystem = FindFirstObjectByType<PlacementSystem>();
        InputManager = FindFirstObjectByType<InputManager>();
        
    }

    public void Start()
    {
        dropArea = FindFirstObjectByType<DropArea>();
    }

    public void Setup (Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        imageSR.sprite = card.Image;
    }

    void OnMouseEnter()
    {
        float zoffset = 0.05f;
        if(!Interactions.Instance.PlayerCanHover()) return;

        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, 0.5f, transform.position.z + zoffset);
        
        // Hover card rotation
        Quaternion rotation = Quaternion.Euler(45, 0, 0);

        CardViewHoverSystem.Instance.Show(Card, pos, rotation);
    }

    void OnMouseExit()
    {
        if(!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    void OnMouseDown()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        Interactions.Instance.PlayerIsDragging = true;
        wrapper.SetActive(true);
        CardViewHoverSystem.Instance.Hide();

        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;

        transform.rotation = Quaternion.Euler(45,0,0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(transform.position);
    }

    void OnMouseDrag()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(transform.position);
    }

    void OnMouseUp()
    {
        if(!Interactions.Instance.PlayerCanInteract()) return;
        
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
