using UnityEngine;

public class AddEffectOnBoardGA : GameAction
{
    public Vector3Int GridTarget { get; private set; }
    public BoardEffect BoardEffect { get; private set;}
    public int EffectDuration { get; private set; }

    public AddEffectOnBoardGA(Vector3Int targetGrid, BoardEffect effect, int effectDuration)
    {
        GridTarget = targetGrid;
        BoardEffect = effect;
        EffectDuration = effectDuration;
    }
}