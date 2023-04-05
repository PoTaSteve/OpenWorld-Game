using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineOrbitalTransposer;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerGFX;
    private Rigidbody rb;
    private PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxStamina;
    public float currentStamina;
    private float horizontalInput;
    private float verticalInput;

    public bool climbing;

    [Header("ClimbJumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;

    public KeyCode jumpKey;
        
    [Header("Detection")]
    public float detectionLength;
    public float sphereCastradius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        WallCheck();
        StateMachine();

        if (pm.isGrounded && currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // State 1 - Start Climbing
        if (!climbing && wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle && !exitingWall && currentStamina > 0)
        {
            StartClimbing();
        }

        // State 2 - Climbing
        else if (climbing && !exitingWall)
        {
            ClimbingMovement();

            // Climb Jump
            if (wallFront && Input.GetKeyDown(jumpKey))
            {
                ClimbJump();
            }

            // Stamina management
            if (currentStamina > 0)
            {
                currentStamina -= Time.deltaTime;
            }
            if (currentStamina <= 0)
            {
                RunOutOfStamina();
            }

            if (pm.isGrounded)
            {
                StopClimbing();
            }

            // Check for vault
            bool vaultDown = Physics.Raycast(transform.position + Vector3.down * 0.95f, playerGFX.forward, detectionLength, whatIsWall);
            bool vaultUp = Physics.Raycast(transform.position + Vector3.up * 0.95f, playerGFX.forward, detectionLength, whatIsWall);
            if (!vaultDown && !vaultUp)
            {
                StopClimbing();

                //rb.velocity = Vector3.zero;
                //rb.AddForce(playerGFX.forward * 5f, ForceMode.Force);

                StartCoroutine(PushUntilGrounded());
            }
        }

        // State 3 - Exiting
        else if (exitingWall)
        {
            if (climbing)
            {
                StopClimbing();
            }

            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            if (exitWallTimer <= 0)
            {
                exitingWall = false;
                pm.restricted = false;
            }
        }

        // State 4 - None
        else
        {
            if (climbing)
            {
                StopClimbing();
            }
        }        
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastradius, playerGFX.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(playerGFX.forward, -frontWallHit.normal);
    }

    private void StartClimbing()
    {
        Debug.Log("Starting climb");

        pm.canRotate = false;

        playerGFX.forward = -frontWallHit.normal;

        // Snap to wall
        if (Vector3.Distance(transform.position, frontWallHit.point) > 0.5f)
        {
            rb.position = frontWallHit.point + frontWallHit.normal * 0.5f;
        }

        climbing = true;
        pm.climbing = true;

        rb.useGravity = false;
    }

    private void ClimbingMovement()
    {
        transform.position += (playerGFX.up * verticalInput + playerGFX.right * horizontalInput) * climbSpeed * Time.deltaTime;
    }

    private void StopClimbing()
    {
        Debug.Log("Stop Climbing");

        climbing = false;
        pm.climbing = false;

        rb.useGravity = true;

        pm.canRotate = true;
    }

    private void ClimbJump()
    {
        exitingWall = true;
        pm.restricted = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void RunOutOfStamina()
    {
        climbing = false;
        pm.climbing = false;

        rb.useGravity = true;

        currentStamina = 0;
    }

    private IEnumerator PushUntilGrounded()
    {
        while (!pm.isGrounded)
        {
            rb.AddForce(playerGFX.forward, ForceMode.Impulse);

            if (rb.velocity.x < 0.1f && rb.velocity.z < 0.1f)
            {
                rb.AddForce(playerGFX.up, ForceMode.Force);
            }
            else
            {
                rb.AddForce(-playerGFX.up * 5f, ForceMode.Force);
            }

            yield return new WaitForEndOfFrame();
        }

        rb.velocity = Vector3.zero;
    }
}
