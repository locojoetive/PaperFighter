using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartScript : MonoBehaviour {
    public bool hit;
    private int life = 3;

    public int shaker = 0;
    public float shakeIt = 0.0f;
    private float shakeTime = .5f;
    private float growTime;
    public float shakePower;
    public float growExtent;
    public float nextGrowDecision;

    private Vector3 newSize;

    public Animator[] hearts;


    void Start () {
        Debug.Log("Start has been called");

        // handle Sizes
        newSize = GetComponent<Transform>().localScale;
        float horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        horzExtent -= 1.5f * newSize.x;
        float vertExtent = Camera.main.orthographicSize;
        vertExtent -= 1.5f * newSize.y;
        transform.localPosition = new Vector3(-horzExtent, vertExtent, 1F);
    }

    void FixedUpdate()
    {
        if (hit)
        {
            HandleHit();
        }
        HandleGrow();
    }

    void HandleGrow()
    {
        if (Time.time >= growTime)
        {
            growTime += nextGrowDecision;
            growExtent = -growExtent;
        }
        newSize.x = transform.localScale.x + growExtent;
        newSize.y = transform.localScale.y + growExtent;
        newSize.z = transform.localScale.z + growExtent;
        transform.localScale = newSize;
    }

    void HandleHit()
    {
        HandleShake();
    }

    void HandleShake()
    {
        if (Time.time > shakeTime)
        {
            shakeTime = Time.time + 0.1f;
            shakePower = -shakePower;
        }

        Quaternion rotation = transform.rotation;
        float z = rotation.eulerAngles.z;
        z -= shakePower * Time.deltaTime;
        rotation = Quaternion.Euler(0, 0, z);
        transform.rotation = rotation;
        shaker++;
        if(shaker == 25)
        {
            HandleLife();
        }

        if (shaker == 50)
        {
            shaker = 0;

            hit = false;

            float temp = 0;
            rotation = Quaternion.Euler(0, 0, temp);
            transform.rotation = rotation;
        }
    }

    private void HandleLife()
    {
        life--;
        if (life == 2)
        {
            hearts[0].SetBool("Hit", true);
        }
        else if (life == 1)
        {
            hearts[1].SetBool("Hit", true);
        }
        else if (life == 0)
        {
            hearts[2].SetBool("Hit", true);
        }
    }

    public void ResetHearts()
    {
        hearts[0].SetBool("Hit", false);
        hearts[1].SetBool("Hit", false);
        hearts[2].SetBool("Hit", false);
    } 

    public void setHit()
    {
        hit = true;
    }
}
