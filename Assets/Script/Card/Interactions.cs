using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : Singleton<Interactions>
{

    public bool PlayerIsDragging { get; set; } = false;
    [SerializeField] SwitchCamera switchCamera;
    [SerializeField] PlacementSystem placementSystem;
    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming) return true;
        else return false;
    }

    public bool PlayerCanHover()
    {
        if(PlayerIsDragging || placementSystem.isPlacing) return false;
        return true;
    }

    public bool IsPlayerCard(int playerHand)
    {
        if(switchCamera.Manager == playerHand) return true;
        return false;
    }

}
