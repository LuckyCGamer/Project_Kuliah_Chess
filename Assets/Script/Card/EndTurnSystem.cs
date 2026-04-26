using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnSystem : MonoBehaviour
{

    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        // Debug.Log("End Turn");
        yield return null;;
        // Debug.Log("Change Turn");
    }

}
