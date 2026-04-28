using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoardEffect")]
public class BoardEffect : ScriptableObject
{

    [SerializeField] string effectName;
    [SerializeField] public int canMove;
    [SerializeField] public List<string> cannotMove;

}
