using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class TargetMode 
{

    public abstract List<Vector3Int> GetTargetGrids();

    public abstract List<Piece> GetTargetPieces();

}
