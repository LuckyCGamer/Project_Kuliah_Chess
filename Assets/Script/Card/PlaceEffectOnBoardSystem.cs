using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceEffectOnBoardSystem : MonoBehaviour
{
    [SerializeField] ChessBoardController chessBoardController;

    public void OnEnable()
    {
        ActionSystem.AttachPerformer<AddEffectOnBoardGA>(AddEffectOnBoardPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddEffectOnBoardGA>();
    }    

    private IEnumerator AddEffectOnBoardPerformer(AddEffectOnBoardGA addEffectOnBoardGA)
    {
        chessBoardController.boardStatusEffect[addEffectOnBoardGA] = addEffectOnBoardGA.EffectDuration;
        yield return null;
    }
}