using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseUtil
{
    
    private static Camera camera = Camera.main;

    public static Vector3 GetMousePositionInWorldSpace(Vector3 position)
    {
        Plane dragPlane = new(
            camera.transform.forward,
            position
        );
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
