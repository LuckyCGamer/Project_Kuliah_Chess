using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlacementState : IBuildingState
{

    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectDatabaseSO database;
    GridData floorData;
    GridData spaceData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectDatabaseSO database, GridData floorData, GridData spaceData, ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.spaceData = spaceData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }
        else
            throw new System.Exception("Object with ID " + ID + " not found in database.");

        }

        public void EndState()
        {
            previewSystem.StopShowingPreview();
        }

        public void OnAction(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            if (placementValidity == false) return;

            Vector3Int updatedGridPosition = CheckBoardEdge(gridPosition);
            int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(updatedGridPosition));

            GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
                floorData :
                spaceData;
            selectedData.AddObjectAt(updatedGridPosition,
                database.objectsData[selectedObjectIndex].Size,
                database.objectsData[selectedObjectIndex].ID,
                index,
                database.objectsData[selectedObjectIndex].boardEffect);

            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);

        }

        private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
        {
            GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
                floorData :
                spaceData;

            return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
        }

        public void UpdateState(Vector3Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            Vector3Int updatedGridPosition = CheckBoardEdge(gridPosition);
            previewSystem.UpdatePosition(grid.CellToWorld(updatedGridPosition), placementValidity);
        }

        private Vector3Int CheckBoardEdge(Vector3Int gridPosition)
        {
            Vector3Int offsetPosition = new Vector3Int(0,0,0);
            if (gridPosition.x == -4)
            {
                offsetPosition.x += 1;
            }
            if(gridPosition.y == 7){
                offsetPosition.y += 1;
            }
            return gridPosition + offsetPosition;
        }
    }
