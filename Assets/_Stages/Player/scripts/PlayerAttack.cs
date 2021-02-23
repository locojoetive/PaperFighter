using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private TouchField touch;
    private PlayerStateAnimationSound state;
    public Transform shurikenSpawn;
    public GameObject
        shuriken,
        aim;
    private float maxAimRadius = 2;
    public AttackState attackState = AttackState.DONE;
    private GameObject aimClone;
    private Vector2 neutralShurikenSpawnPosition;
    private Vector2 shootDirection;

    private bool doneAiming;
    public float minimumShootPower = 0f,
        maximumShootPower = 1f;


    private float aimSpeed = 5f;
    private Vector2 goalPosition;
    private  Vector2 shootDirectionLocalSpace;

    void Start()
    {
        touch = FindObjectOfType<TouchField>();
        state = GetComponent<PlayerStateAnimationSound>();
        attackState = AttackState.DONE;
        neutralShurikenSpawnPosition = shurikenSpawn.localPosition;
    }

    private void Update()
    {
        if (state.recovered && !state.hit)
        {
            HandleAttack();
        } else if (state.hit)
        {
            // Reset Attack
            attackState = AttackState.DONE;
            if (aimClone != null)
            {
                Destroy(aimClone.gameObject);
            }
        }
    }

    private void HandleAttack()
    {

        switch (attackState)
        {
            // start new attack
            case AttackState.DONE:
                if (InputManager.action && state.recovered)
                {
                    // Pull out shuriken
                    state.shurikenThrown = false;
                    state.animator.SetBool("Aiming", true);
                    state.audioManager.PlaySound("shurikenSpawn");
                }
                break;
            // start aiming, when shuriken is pulled
            case AttackState.START:
                aimClone = Instantiate(aim, shurikenSpawn.transform.position, Quaternion.identity, transform);
                attackState = AttackState.AIM;
                doneAiming = false;
                break;
            // keep aiming
            case AttackState.AIM:
                if (InputManager.actionContinuous)
                {
                    KeepAiming();
                }
                // init throw
                else if (!doneAiming)
                {
                    doneAiming = true;
                    KeepAiming();
                    state.animator.SetBool("ReleaseAttack", true);
                    state.animator.SetBool("Aiming", false);
                }
                break;
            case AttackState.THROWN:
                state.animator.SetBool("ReleaseAttack", false);
                attackState = AttackState.DONE;
                break;
            default:
                break;
        }
    }

    // Animation Event: Last Frame of Attack_01
    public void ShurikenPulled()
    {
        attackState = AttackState.START;
        shootDirectionLocalSpace = Vector2.zero;
    }


    private void KeepAiming()
    {
        if (InputManager.touchActive)
        {
            Vector2 swipeVector = touch.vSwipe;
            // Debug.Log("PIXELS: " + swipeVector);
            state.Flip(swipeVector.x);
            if (!state.facingRight)
            {
                swipeVector = new Vector2(-swipeVector.x, swipeVector.y);
            }
            if (state.upsideDown)
            {
                swipeVector = new Vector2(swipeVector.x, -swipeVector.y);
            }
            swipeVector.x = swipeVector.x / Camera.main.scaledPixelWidth;
            swipeVector.y = swipeVector.y / (2f * Camera.main.scaledPixelHeight);
            shootDirectionLocalSpace = swipeVector;
            // Debug.Log("LOCAL: " + shootDirectionLocalSpace);
        }
        else
        {
            goalPosition = maxAimRadius * new Vector2(InputManager.xAxis, InputManager.yAxis).normalized;
            state.Flip(goalPosition.x);
            if (!state.facingRight)
            {
                goalPosition = new Vector2(-goalPosition.x, goalPosition.y);
            }
            if (state.upsideDown)
            {
                goalPosition = new Vector2(goalPosition.x, -goalPosition.y);
            }
            shootDirectionLocalSpace = Vector3.Lerp(shootDirectionLocalSpace, goalPosition, Time.deltaTime * aimSpeed);

        }
        shurikenSpawn.localPosition = shootDirectionLocalSpace.normalized * neutralShurikenSpawnPosition.magnitude;
        aimClone.transform.localPosition = ClampLocalSpaceShootDirection();
        shootDirection = transform.TransformVector(ClampLocalSpaceShootDirection());
        // Debug.Log("WORLD: " + shootDirection);

        CastArrow();
    }

    private void CastArrow()
    {
        Vector2 source = shurikenSpawn.position;
        Vector2 dest = aimClone.transform.position;

        aimClone.GetComponent<LineRenderer>().SetPosition(0, source);
        aimClone.GetComponent<LineRenderer>().SetPosition(1, dest);
        
        Vector3 temp = dest - 0.3f * Normalize2D(dest - source).normalized - 0.1f * NormalVector2D(source - dest).normalized;
        aimClone.GetComponent<LineRenderer>().SetPosition(2, temp);
        
        temp = dest - 0.3f * (dest - source).normalized + 0.1f * NormalVector2D(source - dest).normalized; 
        aimClone.GetComponent<LineRenderer>().SetPosition(3, temp);
        aimClone.GetComponent<LineRenderer>().SetPosition(4, dest);
    }

    // Animation Event: Last Frame of Attack_02
    public void SpawnShuriken()
    {
        shuriken.GetComponent<ShurikenScript>().setDirection(state.facingRight);
        GameObject shurikenClone = Instantiate(shuriken, shurikenSpawn.position, Quaternion.identity) as GameObject;
        Vector2 velocity = ClipBetweenMinAndMax(shootDirection);
        Debug.Log("Shoot with: " + shootDirection.magnitude);
        shurikenClone.GetComponent<ShurikenScript>().SetVelocity(velocity);
        shurikenSpawn.localPosition = neutralShurikenSpawnPosition;
        state.animator.SetBool("ReleaseAttack", false);
        state.audioManager.PlaySound("shurikenShoot");
        Destroy(aimClone);
        state.shurikenThrown = true;
        attackState = AttackState.THROWN;
    }
    private Vector2 ClampLocalSpaceShootDirection()
    {
        if (shootDirectionLocalSpace.magnitude > shurikenSpawn.localPosition.magnitude)
            return shootDirectionLocalSpace;
        else
            return shurikenSpawn.localPosition;
    }


    private Vector2 ClipBetweenMinAndMax(Vector2 vector)
    {
        float adjustedLength = (maximumShootPower - minimumShootPower) * vector.magnitude + minimumShootPower;
        vector = adjustedLength * Vector3.Normalize(vector);
        return vector;
    }

    private Vector2 Normalize2D(Vector2 vector)
    {
        return vector / vector.magnitude;
    }

    private Vector2 NormalVector2D(Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x);
    }
}