using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedCameraFunctions : MonoBehaviour
{
    static Vector2 CameraExtents()
    {
        return new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height, Camera.main.orthographicSize);
    }

    static public Bounds adjustedBounds(Vector2 minBound, Vector2 maxBound)
    {
        Vector2
            cameraExtents = CameraExtents();
        
        float 
            minX = minBound.x + cameraExtents.x,
            minY = minBound.y + cameraExtents.y,
            maxX = maxBound.x - cameraExtents.x, 
            maxY = maxBound.y - cameraExtents.y;

        return new Bounds(
            new Vector3(minX + 0.5f * (maxX - minX), minY + 0.5f * (maxY - minY), 0F),
            new Vector3(maxX - minX, maxY - minY, 0F)
        );
    }

    static public bool positionInBounds(Bounds bounds, Vector2 position)
    {
        return position.x < bounds.max.x 
            && position.x > bounds.min.x
            && position.y < bounds.max.y
            && position.y > bounds.min.y;
    }
}
