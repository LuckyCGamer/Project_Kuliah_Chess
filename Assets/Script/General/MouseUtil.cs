using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseUtil
{

    public static Vector3 GetMousePositionInWorldSpace(Vector3 position)
    {
        Plane dragPlane = new(
            Camera.main.transform.forward,
            position
        );
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
