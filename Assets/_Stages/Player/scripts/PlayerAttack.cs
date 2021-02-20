using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStateAnimationSound state;
    public Transform shurikenSpawn;
    public GameObject
        shuriken,
        aim;
    public float maxAimRadius;
    public AttackState attackState = AttackState.DONE;

    private GameObject aimClone;
    private Vector2 neutralShurikenSpawnPosition;
    private Vector2 shootDirection;

    private bool doneAiming;
    public float minimumShootPower,
        maximumShootPower;

    void Start()
    {
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
    }

    // Animation Event: Last Frame of Attack_02
    public void SpawnShuriken()
    {
        shuriken.GetComponent<ShurikenScript>().setDirection(state.facingRight);
        GameObject shurikenClone = Instantiate(shuriken, shurikenSpawn.position, Quaternion.identity) as GameObject;
        
        shurikenClone.GetComponent<ShurikenScript>().SetVelocity(shootDirection);
        shurikenSpawn.localPosition = neutralShurikenSpawnPosition;
        state.animator.SetBool("ReleaseAttack", false);
        state.audioManager.PlaySound("shurikenShoot");
        Destroy(aimClone);
        state.shurikenThrown = true;
        attackState = AttackState.THROWN;
    }

    private void KeepAiming()
    {

        Vector2 shootDirectionLocalSpace;
        if (InputManager.touchActive)
        {
            shootDirectionLocalSpace = TouchBehaviour.swipeVector;
            shootDirectionLocalSpace.x = shootDirectionLocalSpace.x / (.25f * Camera.main.scaledPixelWidth);
            shootDirectionLocalSpace.y = shootDirectionLocalSpace.y / (.5f * Camera.main.scaledPixelHeight);
        }
        else
        {
            shootDirectionLocalSpace = new Vector2(InputManager.xAxis, InputManager.yAxis);
        }
        
        shootDirectionLocalSpace = ClipBetweenMinAndMax(shootDirectionLocalSpace);

        state.Flip(shootDirectionLocalSpace.x);
        if (!state.facingRight)
        {
            shootDirectionLocalSpace = new Vector2(-shootDirectionLocalSpace.x, shootDirectionLocalSpace.y);
        }
        if (state.upsideDown)
        {
            shootDirectionLocalSpace = new Vector2(shootDirectionLocalSpace.x, -shootDirectionLocalSpace.y);
        }

        shurikenSpawn.localPosition = Vector3.Normalize(shootDirectionLocalSpace) * neutralShurikenSpawnPosition.magnitude;
        aimClone.transform.localPosition = maxAimRadius * shootDirectionLocalSpace;

        shootDirection = aimClone.transform.position - shurikenSpawn.position;

        CastArrow();
    }

    private void CastArrow()
    {
        aimClone.GetComponent<LineRenderer>().SetPosition(0, shurikenSpawn.position);
        aimClone.GetComponent<LineRenderer>().SetPosition(1, aimClone.transform.position);
        Vector3 temp = aimClone.transform.position - 0.3f * (Vector3)Normalize2D(aimClone.transform.position - shurikenSpawn.position)
            - 0.1f * (Vector3)Normalize2D(NormalVector2D(shurikenSpawn.position - aimClone.transform.position));
        aimClone.GetComponent<LineRenderer>().SetPosition(2, temp);
        temp = aimClone.transform.position - 0.3f * (Vector3)Normalize2D(aimClone.transform.position - shurikenSpawn.position)
            + 0.1f * (Vector3)Normalize2D(NormalVector2D(shurikenSpawn.position - aimClone.transform.position)); aimClone.GetComponent<LineRenderer>().SetPosition(3, temp);
        aimClone.GetComponent<LineRenderer>().SetPosition(4, aimClone.transform.position);
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