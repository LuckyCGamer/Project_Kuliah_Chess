using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{

    [ SerializeField] private List<CardData> deckData;

    private void Start()
    {
        CardSystem.Instance.Setup(deckData);
        DrawCardGA drawCardGA_player1 = new(5, "both");
        ActionSystem.Instance.Perform(drawCardGA_player1);
    }

}
