using System.Collections;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{

    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }  
    [field: SerializeField] public bool IsPlacement { get; private set; }
    [field: SerializeField] public int ID { get; private set; } 

    [field: SerializeReference, SR] public List<Effect> Effects { get; private set;}

}
