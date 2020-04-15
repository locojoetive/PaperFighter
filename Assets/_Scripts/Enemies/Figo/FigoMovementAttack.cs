using System;
using UnityEngine;

public class FigoMovementAttack : MonoBehaviour {

    private GameObject player;
    private FigoStatesAnimationSound state;
    private Rigidbody2D rb;

    private bool facingRight = true;
    private float switchDirectionAt = 0.0f;
    public float 
        coolDown,
        loopTime,
        nextAttack,
        speed;
    public int life = 1;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state = GetComponent<FigoStatesAnimationSound>();
    }

    private void Update()
    {
        if (!state.playerDetected()) HandleMovement();
        else HandleAttack();
    }

    private void HandleMovement()
    {
        
        if (Time.time < switchDirectionAt && rb) rb.velocity = new Vector2(speed, rb.velocity.y);
        else {
            switchDirectionAt = Time.time + loopTime;
            Flip();
            speed = facingRight ? Mathf.Abs(speed) : -Mathf.Abs(speed);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(
            facingRight ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );
    }

    private void HandleAttack()
    {
        switchDirectionAt += Time.deltaTime;
        LookAtPlayer();
        if (Time.time > nextAttack)
        {
            state.triggerAttack();
            nextAttack = Time.time + coolDown;
        }
    }

    private void LookAtPlayer()
    {
        Vector2 relativePlayerPosition = state.relativePlayerPosition();
        if (
            relativePlayerPosition.x > 0 && !facingRight
            || relativePlayerPosition.x < 0 && facingRight
        ) Flip();

    }
}
