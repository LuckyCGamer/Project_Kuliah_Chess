using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData;
    GridData spaceData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, 
                         PreviewSystem previewSystem, 
                         GridData floorData, 
                         GridData spaceData, 
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.spaceData = spaceData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = spaceData;
        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
    
        if(gameObjectIndex == -1)
            return;
        selectedData.RemoveObjectAt(gridPosition);
        objectPlacer.RemoveObjectAt(gameObjectIndex);
        // Vector3 cellPosition = grid.CellToWorld(gridPosition);
        // previewSystem.UpdatePosition(cellPosition, false);
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(spaceData.CanPlaceObjectAt(gridPosition, Vector2Int.one) &&
            floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }


    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
