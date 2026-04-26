using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    private Vector3 lastPosition;
    [SerializeField] private LayerMask placementLayerMask;
    [SerializeField] ActionSystem actionSystem;
    public Card selectedCard;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private GameObject cardView;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedCard != null){
            OnClicked?.Invoke();
            resetCard();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            OnExit?.Invoke();
            resetCard();
            cardView.SetActive(true);
            cardView.transform.position = originalPosition;
            cardView.transform.rotation = originalRotation;
        }
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
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
