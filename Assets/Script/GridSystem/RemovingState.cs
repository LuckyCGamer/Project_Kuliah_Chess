using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RemovingState : MonoBehaviour
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

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
        if(gameObjectIndex == -1)
            return;
        selectedData.RemoveObjectAt(gridPosition);
        objectPlacer.RemoveObjectAt(gameObjectIndex);
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }
}
