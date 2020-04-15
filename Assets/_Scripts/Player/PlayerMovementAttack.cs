using System;
using UnityEngine;

public class PlayerMovementAttack : MonoBehaviour
{
    private PlayerStateAnimationSound state;
    private Rigidbody2D rb;
    private GameObject aimClone;
    private Vector2 shurikenDirection;
    private Vector3 aimingVelocity;

    public Transform shurikenSpawn;
    public GameObject 
        shuriken,
        aim;


    private float
        aimingSmoothDamp = 0.2f,
        aimFieldSize = 3F,
        wallJumpFrame = 0.2f;
    private int
        jumpHeight = 10;
    public int walkingSpeed = 8;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<PlayerStateAnimationSound>();
    }

    private void Update()
    {
        if (!state.isAiming() && aimClone)
        {
            Destroy(aimClone);
            if (!state.shurikenThrownOrCanceled)
            {
                Debug.Log("something smells");
            }
        }

        if (state.isRecovered() && !state.isHit())
        {
            HandleAttack();
        }
        if (state.isMovable())
        {
            HandleMovement();
        }
        HandleGravity();
    }


    private void HandleGravity()
    {
        bool upsideDown = state.isUpsideDown();
        jumpHeight = upsideDown ? - Mathf.Abs(jumpHeight) : Mathf.Abs(jumpHeight);
    }

    private void HandleMovement()
    {
        state.Flip(state.horizontalMovementDegree());
        if (state.isWallGrabbing())
        {
            rb.velocity = Vector3.zero;
        }
        rb.velocity = new Vector2(state.horizontalMovementDegree() * walkingSpeed, rb.velocity.y);
        Jump();
    }

    private void Jump()
    {
        if (state.isJumpButtonPressed() && state.getJumpNo() < 2)
        {
            if (!state.isWallGrabbing())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            }
            else
            {
                if (state.isFacingRight())
                {
                    rb.velocity = new Vector2(-5, jumpHeight);
                    state.Flip(-1);
                }
                else
                {
                    rb.velocity = new Vector2(5, jumpHeight);
                    state.Flip(1);
                }
                state.FreezeMoves(wallJumpFrame);
            }
            state.PlayJumpSound();
            state.incrementJumpNo();
        }
    }
    
    private void HandleAttack()
    {
        if (state.aiming)
        {
            if (InputManager.actionRelease)
            {
                ReleaseAttack();
            }
            else if (InputManager.actionContinuous)
            {
                KeepAiming();
            }
        } 
        else if (!aimClone && InputManager.action)
        {
            state.setAiming();
            aimClone = Instantiate(aim, shurikenSpawn.transform.position, Quaternion.identity, transform);
        }
    }

    private void KeepAiming()
    {
        // state.setMove(false);
        if (InputManager.touchActive)
        {
            aimClone.transform.position = shurikenSpawn.position + 5F * NormalizeSwipeVector();
            state.Flip(NormalizeSwipeVector().x);
        }
        else
        {
            float horizontal = InputManager.xAxis < 0F ? -1F : InputManager.xAxis > 0F ? 1F : 0F,
                vertical = InputManager.yAxis < 0F ? -1F : InputManager.yAxis > 0F ? 1F : 0F;
            Vector2 aimMoveDirection = new Vector2(horizontal, vertical);
            Vector2 nextAimDirection = (Vector2)(aimClone.transform.position - transform.position) + aimMoveDirection;
            if (nextAimDirection.magnitude > aimFieldSize)
            {
                nextAimDirection = nextAimDirection.normalized * aimFieldSize;
            }

            aimClone.transform.position = Vector3.SmoothDamp(
                aimClone.transform.position,
                (Vector2)transform.position + nextAimDirection,
                ref aimingVelocity,
                aimingSmoothDamp
            );
        }

        Vector2 localShurikenSpawnPosition = (aimClone.transform.position - transform.position).normalized * shurikenSpawn.localPosition.magnitude;
        if (!state.isFacingRight())
        {
            localShurikenSpawnPosition.x = -localShurikenSpawnPosition.x;
        }

        if (state.isUpsideDown())
        {
            localShurikenSpawnPosition.y = -localShurikenSpawnPosition.y;
        }

        shurikenSpawn.localPosition = localShurikenSpawnPosition;
        CastArrow();
        bool turnAround = (aimClone.transform.position - transform.position).x < 0 && state.isFacingRight()
            || (aimClone.transform.position - transform.position).x > 0 && !state.isFacingRight();
        if (turnAround)
        {
            state.Flip(state.isFacingRight() ? -1F : 1F);
        }
    }

    private void CastArrow()
    {
        aimClone.GetComponent<LineRenderer>().SetPosition(0, shurikenSpawn.position);
        aimClone.GetComponent<LineRenderer>().SetPosition(1, aimClone.transform.position);
        Vector3 temp = aimClone.transform.position - 0.3f * (Vector3)SimpleMath.Normalize2D(aimClone.transform.position - shurikenSpawn.position)
            - 0.1f * (Vector3)SimpleMath.Normalize2D(SimpleMath.NormalVector2D(shurikenSpawn.position - aimClone.transform.position));
        aimClone.GetComponent<LineRenderer>().SetPosition(2, temp);
        temp = aimClone.transform.position - 0.3f * (Vector3)SimpleMath.Normalize2D(aimClone.transform.position - shurikenSpawn.position)
            + 0.1f * (Vector3)SimpleMath.Normalize2D(SimpleMath.NormalVector2D(shurikenSpawn.position - aimClone.transform.position)); aimClone.GetComponent<LineRenderer>().SetPosition(3, temp);
        aimClone.GetComponent<LineRenderer>().SetPosition(4, aimClone.transform.position);
    }

    private void ReleaseAttack()
    {
        if (aimClone)
        {
            Vector2 localShurikenSpawnPosition = (aimClone.transform.position - transform.position).normalized * shurikenSpawn.localPosition.magnitude;
            if (!state.isFacingRight())
            {
                localShurikenSpawnPosition.x = -localShurikenSpawnPosition.x;
            }

            if (state.isUpsideDown())
            {
                localShurikenSpawnPosition.y = -localShurikenSpawnPosition.y;
            }

            shurikenSpawn.localPosition = localShurikenSpawnPosition;
            shurikenDirection = aimClone.transform.position - shurikenSpawn.transform.position;
            Destroy(aimClone);
            state.resetAiming();
            
        }
    }
    
    public void spawnShuriken()
    {
        shuriken.GetComponent<ShurikenScript>().setDirection(state.isFacingRight());
        GameObject shurikenClone = Instantiate(shuriken, shurikenSpawn.position, Quaternion.identity) as GameObject;
        shurikenClone.GetComponent<ShurikenScript>().SetVelocity(shurikenDirection);
        shurikenSpawn.localPosition = new Vector3(shurikenSpawn.localPosition.magnitude, 0F, 0F);
        state.ThrowShuriken();
    }
    private Vector3 NormalizeSwipeVector()
    {
        Vector3 swipe = TouchBehaviour.swipeVector;
        swipe.x = swipe.x / Camera.main.scaledPixelWidth;
        swipe.y = swipe.y / Camera.main.scaledPixelHeight;
        swipe.z = 0F;
        return swipe;
    }
}