using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject
        shuriken,
        aim;
    public Transform shurikenSpawn;
    private GameObject aimClone;
    private TouchField touch;
    private PlayerStateAnimationSound state;
    private Vector2 neutralShurikenSpawnPosition;
    private Vector2 shootDirection;
    private Vector2 shootDirectionLocalSpace;
    private AttackState attackState = AttackState.DONE;
    private bool doneAiming;
    private float minimumSwipePower = .4f;
    private float maximumSwipePower = 1.2f;
    private float swipeLowerBound = 20f;
    private float swipeUpperBound = 100f;
    private float maximumShootPower = 3.5f;
    private float keyboardAimSpeed = 10f;

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
            HandleAttack();
        else if (state.hit)
        {
            // Reset Attack
            attackState = AttackState.DONE;
            if (aimClone != null)
                Destroy(aimClone.gameObject);
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

    private void KeepAiming()
    {
        if (InputManager.touchActive)
        {
            Vector2 swipeVector = touch.vSwipe;
            state.Flip(swipeVector.x);
            if (!state.facingRight)
            {
                swipeVector = new Vector2(-swipeVector.x, swipeVector.y);
            }
            if (state.upsideDown)
            {
                swipeVector = new Vector2(swipeVector.x, -swipeVector.y);
            }

            // Bestimme Swipe-Länge zwischen 100 - 200 px
            float magnitude = Mathf.Clamp(swipeVector.magnitude, swipeLowerBound, swipeUpperBound);
            // Projeziere Swipe-Länge auf Ellipse
            magnitude = ellipticThrowPower(magnitude);

            // Projeziere in Local Space
            shootDirectionLocalSpace = magnitude * swipeVector.normalized;
            shurikenSpawn.localPosition = shootDirectionLocalSpace.normalized * neutralShurikenSpawnPosition.magnitude;
            Debug.DrawLine(transform.position, aimClone.transform.position, Color.red);
            shootDirection = transform.TransformVector(shootDirectionLocalSpace);
            aimClone.transform.localPosition = CalculateArrow();
            CastArrow();
        }
        else
        {
            Vector2 moveShootDirection = keyboardAimSpeed * Time.deltaTime * new Vector2(
                InputManager.xAxis,
                InputManager.yAxis
            ).normalized;
            shootDirectionLocalSpace += moveShootDirection;
            shootDirectionLocalSpace = Vector2.ClampMagnitude(shootDirectionLocalSpace, maximumShootPower);
            state.Flip(shootDirectionLocalSpace.x);
            shootDirection = transform.TransformDirection(shootDirectionLocalSpace);

            if (shootDirectionLocalSpace.magnitude > 0f)
            {
                Vector2 shurikenSpawnPosition = (Vector2) transform.position
                    + shootDirectionLocalSpace.normalized * Mathf.Abs(transform.localScale.x) * neutralShurikenSpawnPosition.magnitude;

                Vector2 shurikenSpawnLocalPosition = shurikenSpawnPosition - (Vector2) transform.position;
                Vector2 aimClonePosition = shootDirection.magnitude > shurikenSpawnLocalPosition.magnitude
                    ? (Vector2) transform.position + shootDirection
                    : shurikenSpawnPosition;

                shurikenSpawn.position = shurikenSpawnPosition;
                aimClone.transform.position = aimClonePosition;
            }
            CastArrow();
        }
    }

    private float ellipticThrowPower(float x)
    {
        float b = maximumSwipePower - minimumSwipePower;
        return Mathf.Clamp(
            minimumSwipePower + b * (1 - Mathf.Sqrt(b * b * (1 - Mathf.Pow(x / swipeUpperBound, 2)))),
            minimumSwipePower,
            maximumSwipePower
        );
    }

    private void CastArrow()
    {
        Vector2 source = shurikenSpawn.position;
        Vector2 dest = aimClone.transform.position;

        aimClone.GetComponent<LineRenderer>().SetPosition(0, source);
        aimClone.GetComponent<LineRenderer>().SetPosition(1, dest);

        Vector3 temp = dest - 0.3f * NormalizeVector2D(dest - source).normalized - 0.1f * NormalVector2D(source - dest).normalized;
        aimClone.GetComponent<LineRenderer>().SetPosition(2, temp);

        temp = dest - 0.3f * (dest - source).normalized + 0.1f * NormalVector2D(source - dest).normalized;
        aimClone.GetComponent<LineRenderer>().SetPosition(3, temp);
        aimClone.GetComponent<LineRenderer>().SetPosition(4, dest);
    }

    private Vector2 CalculateArrow()
    {
        if (shootDirectionLocalSpace.magnitude > shurikenSpawn.localPosition.magnitude)
            return shootDirectionLocalSpace;
        else
            return shurikenSpawn.localPosition;
    }


    private Vector2 ClipBetweenMinAndMax(Vector2 vector)
    {
        float adjustedLength = (maximumSwipePower - minimumSwipePower) * vector.magnitude + minimumSwipePower;
        vector = adjustedLength * Vector3.Normalize(vector);
        return vector;
    }

    private Vector2 NormalizeVector2D(Vector2 vector)
    {
        return vector / vector.magnitude;
    }

    private Vector2 NormalVector2D(Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x);
    }

    public void abortAttack()
    {
        attackState = AttackState.DONE;
    }

    // DONT DELETE! Animation Event: Last Frame of Attack_02
    public void SpawnShuriken()
    {
        shuriken.GetComponent<ShurikenScript>().setDirection(state.facingRight);
        GameObject shurikenClone = Instantiate(shuriken, shurikenSpawn.position, Quaternion.identity) as GameObject;
        Vector2 velocity = ClipBetweenMinAndMax(shootDirection);
        shurikenClone.GetComponent<ShurikenScript>().SetVelocity(velocity);
        shurikenSpawn.localPosition = neutralShurikenSpawnPosition;
        state.animator.SetBool("ReleaseAttack", false);
        state.audioManager.PlaySound("shurikenShoot");
        Destroy(aimClone);
        state.shurikenThrown = true;
        attackState = AttackState.THROWN;
    }

    // DONT DELETE! Animation Event: Last Frame of Attack_01
    public void ShurikenPulled()
    {
        attackState = AttackState.START;
        shootDirectionLocalSpace = Vector2.zero;
    }
}