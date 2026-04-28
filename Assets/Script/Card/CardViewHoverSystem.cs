using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView cardViewHover;
    public void Show(Card card, Vector3 position, Quaternion rotation, int playerHand)
    {
        cardViewHover.gameObject.SetActive(true);
        cardViewHover.Setup(card, playerHand);
        cardViewHover.transform.position = position;
        cardViewHover.transform.rotation = rotation;
    }

    public void Hide()
    {
        cardViewHover.gameObject.SetActive(false);
    }
}
