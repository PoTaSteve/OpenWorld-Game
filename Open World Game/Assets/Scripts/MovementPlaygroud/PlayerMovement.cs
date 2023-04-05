using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MovementState
{
    FREEZE,
    UNLIMITED,
    WALKING,
    SPRINTING,
    CROUCHING,
    SLIDING,
    WALLRUNNING,
    CLIMBING,
    AIR
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float climbSpeed;
    public float wallrunSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    public bool isGrounded;
    public bool isInAir;
    public bool sliding;
    public bool wallrunning;
    public bool climbing;

    public bool usingGravity;

    public bool freeze;
    public bool unlimited;

    public bool restricted; // Set active to avoid player movement with keys

    public bool canRotate = true;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftControl;
    public KeyCode crouchKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsground;

    [Header("Slope Handler")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")]
    private Climbing climbingScript;

    public Transform orientation;
    public Transform cam;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        climbingScript = GetComponent<Climbing>();

        readyToJump = true;

        startYScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        usingGravity = rb.useGravity;

        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsground);

        if (!isGrounded && !wallrunning && !climbing)
            isInAir = true;
        else
            isInAir = false;

        InputMethod();
        SpeedControl();
        StateHandler();

        // Handle drag
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Register inputs for movement
    private void InputMethod()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // When to jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            // Allows continuos jumps by keeping jump button pressed
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

            // I want to add force down since the center of the player is 1 unit over the ground
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        // Stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    // Set the state of the player and the desiredSpeed -> moveSpeed
    private void StateHandler()
    {
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.FREEZE;
            rb.velocity = Vector3.zero;
            desiredMoveSpeed = 0;
        }
        // Mode - Unlimited
        else if (unlimited)
        {
            state = MovementState.UNLIMITED;
            moveSpeed = 999f;
            return;
        }
        // Mode - Climbing
        else if (climbing)
        {
            state = MovementState.CLIMBING;
            desiredMoveSpeed = climbSpeed;
        }
        // Mode - Wallrunning
        else if (wallrunning)
        {
            state = MovementState.WALLRUNNING;
            desiredMoveSpeed = wallrunSpeed;
        }
        // Mode - Sliding
        else if (sliding)
        {
            state = MovementState.SLIDING;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.CROUCHING;
            desiredMoveSpeed = crouchSpeed;
        }
        // Mode - Sprinting
        else if (isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.SPRINTING;
            desiredMoveSpeed = sprintSpeed;
        }
        // Mode - Walk
        else if (isGrounded)
        {
            state = MovementState.WALKING;
            desiredMoveSpeed = walkSpeed;
        }
        // Mode - Air
        else if (isInAir)
        {
            state = MovementState.AIR;
        }

        // Check if desiredMoveSpeed has changed drastically (a bit more then the difference between the walkSpeed and sprintingSpeed)
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // Smoothly lerps movementSpeed to desired value
        float time = 0;
        float differrence = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < differrence)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / differrence);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // While restricted pressing any keys for movement has no effect
        if (restricted)
            return;

        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            // Avoid bumbing-like movement
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // On ground
        else if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        
        // In air
        else if(isInAir) // Changed from !isGrounded
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // Turn off gravity while on slope
        if (!wallrunning && !climbing)
        {
            rb.useGravity = !OnSlope();
        }
    }

    private void SpeedControl()
    {
        // Limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        // Limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0f;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
