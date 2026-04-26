using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectDatabaseSO database;
    GridData floorData;
    public GridData spaceData;

    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private PreviewSystem previewSystem;

    private Vector3Int LastDetectedPosition = Vector3Int.zero;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private 
    
    IBuildingState buildingState;


    private void Start()
    {
        StopPlacement();
        floorData = new();
        spaceData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState( ID, 
                                            grid, 
                                            previewSystem, 
                                            database, 
                                            floorData, 
                                            spaceData, 
                                            objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        PlayCardGA playCardGA = new(inputManager.selectedCard, gridPosition);
        ActionSystem.Instance.Perform(playCardGA);

        buildingState.OnAction(gridPosition);
        StopPlacement();

    }

    public void RemoveEffectOnBoard(Vector3Int gridPosition)
    {
        buildingState = new RemovingState(
            grid, previewSystem, floorData, spaceData, objectPlacer
        );
        buildingState.OnAction(gridPosition);
        StopPlacement();
    }

    // private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    // {
    //     GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? 
    //         floorData : 
    //         spaceData;
        
    //     return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    // }

    private void StopPlacement()
    {
        if(buildingState == null) 
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        LastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(LastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            LastDetectedPosition = gridPosition;     
        }
    }
}
