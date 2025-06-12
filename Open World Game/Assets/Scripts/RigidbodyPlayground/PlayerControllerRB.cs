using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRB : MonoBehaviour
{
    // Not implemented points 2.5 - 2.6 - 2.7 of the tutorial

    [SerializeField, Range(0f, 50f)]
    float maxSpeed = 10f, maxClimbSpeed = 2f, maxSwimSpeed = 5f;

    [SerializeField, Range(0f, 100f)]
    float maxClimbAcceleration = 20f; // maxAcceleration = 10f, maxAirAcceleration = 1f, maxSwimAcceleration = 5f;

    [SerializeField, Range(0f, 100f)]
    float maxSnapSpeed = 100f;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f, maxStairsAngle = 50f;

    [SerializeField, Range(90, 180)]
    float maxClimbAngle = 140f;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;

    [SerializeField, Min(0f)]
    float probeDistance = 1f;

    [SerializeField]
    float submergenceOffset = 0.5f;

    [SerializeField, Min(0.1f)]
    float submergenceRange = 2f;

    [SerializeField, Range(0f, 10f)]
    float waterDrag = 1f;

    [SerializeField, Min(0f)]
    float buoyancy = 1f;

    [SerializeField, Range(0.01f, 1f)]
    float swimThreshold = 0.5f;

    [SerializeField]
    LayerMask probeMask = -1, stairsMask = -1, climbMask = -1, waterMask = 0;

    [SerializeField]
    Transform playerInputSpace = default;

    int jumpPhase;

    Vector3 playerInput;

    Vector3 velocity;
    Vector3 takeOffVelocity;
    Vector3 connectionVelocity;
    Vector3 contactNormal, steepNormal, climbNormal, lastClimbNormal;

    Vector3 connectionWorldPosition, connectionLocalPosition;

    bool desiredJump;

    int groundContactCount, steepContactCount, climbContactCount;

    bool OnGround => groundContactCount > 0;
    bool OnSteep => steepContactCount > 0;
    bool Climbing => climbContactCount > 0 && stepsSinceLastJump > 2;
    bool InWater => submergence > 0f;
    bool Swimming => submergence >= swimThreshold;

    float submergence;

    Rigidbody rb, connectedBody, previousConnectedBody;

    float minGroundDotProduct, minStairsDotProduct, minClimbDotProduct;
    int stepsSinceLastGrounded, stepsSinceLastJump;

    Vector3 upAxis, rightAxis, forwardAxis;

    [SerializeField]
    Material normalMaterial = default, climbingMaterial = default, swimmingMaterial = default;

    [SerializeField]
    MeshRenderer Capsule, Cube;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public bool canUpdateMovement;

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
        minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

        OnValidate();
    }

    // Update is called once per frame
    void Update()
    {
        if (canUpdateMovement)
        {
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput.z = Swimming ? Input.GetAxis("UpDownSwim") : 0f; // axis buttons -> shift - space
            playerInput = Vector3.ClampMagnitude(playerInput, 1f);

            if (playerInputSpace)
            {
                rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
                forwardAxis = ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
            }
            else
            {
                rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
                forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
            }

            if (!Swimming)
            {
                desiredJump |= Input.GetKeyDown(KeyCode.Space);
            }

            Capsule.material = Climbing ? climbingMaterial : Swimming ? swimmingMaterial : normalMaterial;
            Cube.material = Climbing ? climbingMaterial : Swimming ? swimmingMaterial : normalMaterial;
        }
    }

    private void FixedUpdate()
    {
        if (canUpdateMovement)
        {
            Vector3 gravity = CustomGravity.GetGravity(rb.position, out upAxis);
            UpdateState();

            if (InWater)
            {
                velocity *= 1f - waterDrag * submergence * Time.deltaTime;
            }

            AdjustSnapVelocity();

            if (desiredJump)
            {
                desiredJump = false;
                Jump(gravity);
            }

            if (Climbing)
            {
                velocity -= contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
            }
            else if (InWater)
            {
                velocity += gravity * ((1f - buoyancy * submergence) * Time.deltaTime);
            }
            else if (OnGround && velocity.sqrMagnitude < 0.01f)
            {
                velocity += contactNormal * (Vector3.Dot(gravity, contactNormal) * Time.deltaTime);
            }
            else
            {
                if (OnGround)
                {
                    velocity += gravity * Time.deltaTime;
                }
                else
                {
                    takeOffVelocity += gravity * Time.deltaTime;

                    velocity = takeOffVelocity;
                }
            }

            rb.linearVelocity = velocity;

            SetRotation(velocity);

            ClearState();
        }
    }

    public void SetRotation(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.1f)
        {
            return;
        }
        else
        {
            direction = direction.normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;

        velocity = rb.linearVelocity;

        if (CheckClimbing() || CheckSwimming() || OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;

            if (stepsSinceLastJump > 1)
            {
                jumpPhase = 0;
            }

            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = upAxis;
        }

        if (connectedBody)
        {
            if (connectedBody.isKinematic || connectedBody.mass >= rb.mass)
            {
                UpdateConnectionState();
            }
        }
    }

    void ClearState()
    {
        groundContactCount = steepContactCount = climbContactCount = 0;
        contactNormal = steepNormal = connectionVelocity = climbNormal = Vector3.zero;
        previousConnectedBody = connectedBody;
        connectedBody = null;
        submergence = 0f;
    }

    void UpdateConnectionState()
    {
        if (connectedBody == previousConnectedBody)
        {
            Vector3 connectionMovement = connectedBody.transform.TransformPoint(connectionLocalPosition) - connectionWorldPosition;
            connectionVelocity = connectionMovement / Time.deltaTime;
        }

        connectionWorldPosition = rb.position;
        connectionLocalPosition = connectedBody.transform.InverseTransformPoint(connectionWorldPosition);
    }

    public void Jump(Vector3 gravity)
    {
        Vector3 jumpDirection;
        if (OnGround)
        {
            jumpDirection = contactNormal;
        }
        else if (OnSteep)
        {
            jumpDirection = steepNormal;
            jumpPhase = 0;
        }
        else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps)
        {
            if (jumpPhase == 0)
            {
                jumpPhase = 1;
            }
            jumpDirection = contactNormal;
        }
        else
        {
            return;
        }

        stepsSinceLastJump = 0;
        jumpPhase++;
        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
        if (InWater)
        {
            jumpSpeed *= Mathf.Max(0f, 1f - submergence / swimThreshold);
        }
        jumpDirection = (jumpDirection + upAxis).normalized;
        float alignedSpeed = Vector3.Dot(velocity, jumpDirection);
        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }
        velocity += jumpDirection * jumpSpeed;

        takeOffVelocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        if (Swimming)
        {
            return;
        }

        int layer = collision.gameObject.layer;
        float minDot = GetMinDot(layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if (upDot >= minDot)
            {
                groundContactCount += 1;
                contactNormal += normal;
                connectedBody = collision.rigidbody;
            }
            else
            {
                if (upDot > -0.01f)
                {
                    steepContactCount += 1;
                    steepNormal += normal;
                    if (groundContactCount == 0)
                    {
                        connectedBody = collision.rigidbody;
                    }
                }

                if (upDot >= minClimbDotProduct && (climbMask & (1 << layer)) != 0)
                {
                    climbContactCount += 1;
                    climbNormal += normal;
                    lastClimbNormal = normal;
                    connectedBody = collision.rigidbody;
                }
            }
        }
    }

    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    //void AdjustVelocity()
    //{
    //    float acceleration, speed;
    //    Vector3 xAxis, zAxis;
    //    if (Climbing)
    //    {
    //        acceleration = maxClimbAcceleration;
    //        speed = maxClimbSpeed;
    //        xAxis = Vector3.Cross(contactNormal, upAxis);
    //        zAxis = upAxis;
    //    }
    //    else if (InWater)
    //    {
    //        float swimFactor = Mathf.Min(1f, submergence / swimThreshold);
    //        acceleration = maxSwimAcceleration;
    //        speed = maxSwimSpeed;
    //        acceleration = Mathf.LerpUnclamped(OnGround ? maxAcceleration : maxAirAcceleration, maxSwimAcceleration, swimFactor);
    //        speed = Mathf.LerpUnclamped(maxSpeed, maxSwimSpeed, swimFactor);
    //        xAxis = rightAxis;
    //        zAxis = forwardAxis;
    //    }
    //    else
    //    {
    //        acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
    //        speed = maxSpeed;
    //        xAxis = rightAxis;
    //        zAxis = forwardAxis;
    //    }
    //    xAxis = ProjectDirectionOnPlane(xAxis, contactNormal);
    //    zAxis = ProjectDirectionOnPlane(zAxis, contactNormal);

    //    Vector3 relativeVelocity = velocity - connectionVelocity;
    //    float currentX = Vector3.Dot(relativeVelocity, xAxis);
    //    float currentZ = Vector3.Dot(relativeVelocity, zAxis);

    //    float maxSpeedChange = acceleration * Time.deltaTime;

    //    float newX = Mathf.MoveTowards(currentX, playerInput.x * speed, maxSpeedChange);
    //    float newZ = Mathf.MoveTowards(currentZ, playerInput.y * speed, maxSpeedChange);

    //    velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

    //    if (Swimming)
    //    {
    //        float currentY = Vector3.Dot(relativeVelocity, upAxis);
    //        float newY = Mathf.MoveTowards(currentY, playerInput.z * speed, maxSpeedChange);
    //        velocity += upAxis * (newY - currentY);
    //    }
    //}

    void AdjustSnapVelocity() // Add sliding forward when speed to high to stop instantly on input release
    {
        float speed;
        Vector3 xAxis, zAxis;

        if (Climbing)
        {
            speed = maxClimbSpeed;
            xAxis = Vector3.Cross(contactNormal, upAxis);
            zAxis = upAxis;
        }
        else if (InWater)
        {
            speed = maxSwimSpeed;
            xAxis = rightAxis;
            zAxis = forwardAxis;
        }
        else
        {
            speed = maxSpeed;
            xAxis = rightAxis;
            zAxis = forwardAxis;
        }
        xAxis = ProjectDirectionOnPlane(xAxis, contactNormal);
        zAxis = ProjectDirectionOnPlane(zAxis, contactNormal);

        Vector3 relativeVelocity = velocity - connectionVelocity;
        float currentX = Vector3.Dot(relativeVelocity, xAxis);
        float currentZ = Vector3.Dot(relativeVelocity, zAxis);

        float newX = playerInput.x * speed;
        float newZ = playerInput.y * speed;

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

        if (Swimming)
        {
            float currentY = Vector3.Dot(relativeVelocity, upAxis);
            float newY = playerInput.z * speed;
            velocity += upAxis * (newY - currentY);
        }
    }

    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2 || InWater)
        {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }
        if (!Physics.Raycast(rb.position, -upAxis, out RaycastHit hit, probeDistance, probeMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }
        float upDot = Vector3.Dot(upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        groundContactCount = 1;
        contactNormal = hit.normal;
        
        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot > 0f)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }

        connectedBody = hit.rigidbody;

        return true;
    }

    float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ? minGroundDotProduct : minStairsDotProduct;
    }

    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDotProduct)
            {
                steepContactCount = 0;
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }

    bool CheckClimbing()
    {
        if (Climbing)
        {
            if (climbContactCount > 1)
            {
                climbNormal.Normalize();
                float upDot = Vector3.Dot(upAxis, climbNormal);
                if (upDot >= minGroundDotProduct)
                {
                    climbNormal = lastClimbNormal;
                }
            }
            groundContactCount = 1;
            contactNormal = climbNormal;
            return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((waterMask & (1 << other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((waterMask & (1 << other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence(other);
        }
    }

    void EvaluateSubmergence(Collider collider)
    {
        if (Physics.Raycast(rb.position + upAxis * submergenceOffset, -upAxis, out RaycastHit hit, submergenceRange + 1f, waterMask, QueryTriggerInteraction.Collide))
        {
            submergence = 1f - hit.distance / submergenceRange;
        }
        else
        {
            submergence = 1f;
        }

        if (Swimming)
        {
            connectedBody = collider.attachedRigidbody;
        }
    }

    bool CheckSwimming()
    {
        if (Swimming)
        {
            groundContactCount = 0;
            contactNormal = upAxis;
            return true;
        }
        return false;
    }
}
