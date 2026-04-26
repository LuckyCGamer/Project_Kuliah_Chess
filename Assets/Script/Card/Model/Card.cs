using System;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string Title => data.name;
    public string Description => data.Description;
    public Sprite Image => data.Image;
    public bool IsPlacement => data.IsPlacement;
    public int IDGrid => data.ID;
    public List<Effect> Effects => data.Effects;
    private readonly CardData data;
    public Card (CardData cardData)
    {
        data = cardData;
    }
}