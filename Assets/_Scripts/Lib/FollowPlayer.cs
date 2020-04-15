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
            adjustedBounds[index] = SharedCameraFunctions.adjustedBounds(bounds.min, bounds.max);
            index++;
        }
    }

    public void rolling()
    {
        Vector3 nextPosition = player.position;
        nextPosition.z = transform.position.z;
        for (int i = 0; i < playerBounds.Length; i++)
        {
            if (SharedCameraFunctions.positionInBounds(playerBounds[i], player.position))
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

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        onStage = scene.name.Contains("stage");
    }
}
