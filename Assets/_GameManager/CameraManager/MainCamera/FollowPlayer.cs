using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    private Vector3 currentVelocity = Vector3.zero;
    private Bounds[] playerBounds;
    private Bounds[] adjustedBounds;

    public float
        smoothTime = 20,
        velocity = 0.0F;
    private bool onStage;

    void FixedUpdate()
    {
        if (onStage) { 
            if((adjustedBounds == null || player == null) 
                && GameObject.FindGameObjectWithTag("Player") != null
                && FindObjectOfType<CameraBound>() != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
                transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
                initializeBounds();
            } else if (adjustedBounds != null && player != null)
            {
                DebugBounds();
                rolling();
            }
        }  
        else 
        {
            adjustedBounds = null;
        }
    }

    private void initializeBounds()
    {
        playerBounds = FindObjectOfType<CameraBound>().bounds.ToArray();
        adjustedBounds = new Bounds[playerBounds.Length];
        int index = 0;
        foreach (Bounds bounds in playerBounds)
        {
            adjustedBounds[index] = adjustBounds(bounds.min, bounds.max);
            index++;
        }
    }

    public void rolling()
    {
        Vector3 nextPosition = player.position;
        nextPosition.z = transform.position.z;
        for (int i = 0; i < playerBounds.Length; i++)
        {
            if (positionInBounds(playerBounds[i], player.position))
            {
                Vector2
                    min = adjustedBounds[i].min,
                    max = adjustedBounds[i].max;

                nextPosition = new Vector3(
                    nextPosition.x < min.x
                    ? min.x : nextPosition.x > max.x
                        ? max.x : nextPosition.x,
                    nextPosition.y < min.y
                        ? min.y : nextPosition.y > max.y
                            ? max.y : nextPosition.y,
                    transform.position.z
                );
                transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref currentVelocity, smoothTime);
                break;
            }
        }
    }
    private void DebugBounds()
    {
        foreach (Bounds bounds in adjustedBounds)
        {
            Debug.DrawLine(bounds.min, bounds.min + new Vector3(2F * bounds.extents.x, 0, 0), Color.green);
            Debug.DrawLine(bounds.min + new Vector3(0, 2F * bounds.extents.y, 0), bounds.max, Color.green);
            Debug.DrawLine(bounds.max, bounds.max - new Vector3(0, 2F * bounds.extents.y, 0), Color.green);
            Debug.DrawLine(bounds.min, bounds.min + new Vector3(0, 2F * bounds.extents.y, 0), Color.green);
        }
    }

    public void OnLevelFinishedLoading()
    {
        onStage = StageManager.onStage;
    }


    private Bounds adjustBounds(Vector2 minBound, Vector2 maxBound)
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

    private bool positionInBounds(Bounds bounds, Vector2 position)
    {
        return position.x < bounds.max.x
            && position.x > bounds.min.x
            && position.y < bounds.max.y
            && position.y > bounds.min.y;
    }

    private Vector2 CameraExtents()
    {
        return new Vector2(Camera.main.orthographicSize * Screen.width / Screen.height, Camera.main.orthographicSize);
    }
}
