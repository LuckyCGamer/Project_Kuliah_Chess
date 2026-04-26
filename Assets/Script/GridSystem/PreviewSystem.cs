using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{

    [SerializeField] private float previewYOffset = 0.06f;
    [SerializeField] private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField] private Material previewMaterialPrefab;
    private Material previewMaterialInstance;
    private Renderer cellIndicatorRenderer;
    [SerializeField] private Grid grid;
    private float checkBoardCellScale = 1;


    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.gameObject.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PrepareReview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        cellIndicator.transform.localScale = new Vector3(
            checkBoardCellScale, 
            0.01f, 
            checkBoardCellScale);
            
        if(size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(
                cellIndicator.transform.localScale.x * size.x, 
                0.01f, 
                cellIndicator.transform.localScale.z * size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    private void PrepareReview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for(int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
        cellIndicator.transform.position = new Vector3(
            cellIndicator.transform.position.x, 
            cellIndicator.transform.position.y - 1f, 
            cellIndicator.transform.position.z
        );
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(
            position.x, 
            position.y + previewYOffset - 1f, 
            position.z);  
    }

    internal void StartShowingRemovePreview()
    {
        PrepareCursor(Vector2Int.one);
    }
}
