using System.Collections;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<ReduceDurationGA>(ReduceDurationPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<ReduceDurationGA>();
    }

    private IEnumerator ReduceDurationPerformer(ReduceDurationGA reduceDurationGA)
    {
        Debug.Log("Reduce Duration effect on board");
        yield return new WaitForSeconds(2f);
    }
}