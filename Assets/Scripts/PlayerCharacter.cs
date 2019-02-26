using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;

    new Rigidbody2D rigidbody;

    SpriteRenderer spriteRenderer;

    public float maxHorizontalSpeed = 2;
    public float movementForce = 10;
    [Range(0, 1)] public float friction = 0.75f;
    public float jumpForce = 10;
    [Range(0, 1)] public float jumpSmoothStopForceModifier = 0.5f;
    public float jumpTime = 0.5f;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private float movementDirection;
    private Vector2 velocity;
    private bool isGrounded;

    // Jump staff
    private float currentJumpForce;
    private float jumpTimeCounter;
    private bool isJumpInputDown = false;
    private bool isJumpInputHeld = false;
    private bool isJumpInputUp = false;
    private bool isJumpStarted = false;
    private bool isJumpStopped = false;

    private KeyCode jumpInput = KeyCode.Space;

    readonly int m_isJumping = Animator.StringToHash("IsJumping");
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Store inputs
        movementDirection = Input.GetAxis("Horizontal");
        isJumpInputDown = Input.GetKeyDown(jumpInput);
        isJumpInputHeld = Input.GetKey(jumpInput);
        isJumpInputUp = Input.GetKeyUp(jumpInput);
    }


    void FixedUpdate()
    {
        // Update physics
        UpdateFacing();
        UpdateHorizontalMovement();
        UpdateJump();
    }

    public void UpdateHorizontalMovement()
    {
        rigidbody.AddForce(new Vector2(movementDirection * movementForce, 0), ForceMode2D.Impulse);
        if (rigidbody.velocity.x > maxHorizontalSpeed) {
            rigidbody.velocity = new Vector2(maxHorizontalSpeed, rigidbody.velocity.y);
        } else if (rigidbody.velocity.x < -maxHorizontalSpeed) {
            rigidbody.velocity = new Vector2(-maxHorizontalSpeed, rigidbody.velocity.y);
        }

        if (movementDirection == 0 && IsGrounded()) {
            velocity = rigidbody.velocity;
            velocity.x *= (1 - friction);
            rigidbody.velocity = velocity;
        }
    }

    public bool IsGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return collider != null && rigidbody.velocity.y <= 0;
    }

    public bool CanJump()
    {
        return IsGrounded();
    }

    public void UpdateJump()
    {
        if (isJumpInputDown && CanJump()) {
            isJumpStarted = true;
            isJumpStopped = false;
        }

        if (!isJumpStopped && (isJumpInputUp && isJumpStarted || jumpTimeCounter <= 0)) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            currentJumpForce *= jumpSmoothStopForceModifier;
            rigidbody.AddForce(new Vector2(0, currentJumpForce), ForceMode2D.Impulse);
            isJumpStopped = true;
        }

        if (isJumpInputHeld && !isJumpStopped) {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            currentJumpForce = jumpForce * jumpTimeCounter / jumpTime;
            rigidbody.AddForce(new Vector2(0, currentJumpForce), ForceMode2D.Impulse);
            jumpTimeCounter -= Time.fixedDeltaTime;
        }

        if (IsGrounded()) {
            isJumpStarted = false;
            jumpTimeCounter = jumpTime;
        }
    }

    public void UpdateFacing()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (moveHorizontal > 0 && spriteRenderer.flipX || moveHorizontal < 0 && !spriteRenderer.flipX) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
