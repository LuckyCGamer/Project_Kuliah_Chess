using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestSystemCard : MonoBehaviour
{
    [SerializeField] private List<CardData> deckData;

    void Start()
    {
        CardSystem.Instance.Setup(deckData);
    }

}
