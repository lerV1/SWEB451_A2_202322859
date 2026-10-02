using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMotor : MonoBehaviour
{
    [Header("Shared Data")]
    [SerializeField] private GameConfig config;
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;
    [Header("sprintMultiplier")]
    [SerializeField] private float sprintMultiplier = 1.5f;
    [Header("Air Control")]
    [SerializeField, Range(0f, 1f)] private float airControlMultiplier = 0.35f;
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField, Min(0.01f)]

    private float groundPadding = 0.15f;

    private Rigidbody body;
    private Collider bodyCollider;
    private Vector2 moveInput;
    private bool jumpRequested;
    private Vector3 currentRespawnPosition;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<Collider>();
        currentRespawnPosition = config.respawnPosition;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        jumpAction.action.performed += QueueJump;
        sprintAction.action.Enable();
    }
    private void OnDisable()
    {
        jumpAction.action.performed -= QueueJump;
        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
    }
    private void ReadMoveInput()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
    }
    private float GetActiveMoveSpeed()
    {
        float activeMoveSpeed = config.moveSpeed;

        if (sprintAction.action.IsPressed())
        {
            activeMoveSpeed *= sprintMultiplier;
        }

        return activeMoveSpeed;
    }
    private float GetActiveAcceleration(bool grounded)
    {
        if (grounded)
        {
            return config.acceleration;
        }

        return config.acceleration * airControlMultiplier;
    }
    private void ApplyHorizontalMovement(
    float activeMoveSpeed,
    float activeAcceleration)
    {
        Vector3 desiredHorizontal =
            new Vector3(moveInput.x, 0f, moveInput.y);

        if (desiredHorizontal.sqrMagnitude > 1f)
        {
            desiredHorizontal.Normalize();
        }

        desiredHorizontal *= activeMoveSpeed;

        Vector3 currentVelocity = body.linearVelocity;

        Vector3 currentHorizontal = new Vector3(
            currentVelocity.x,
            0f,
            currentVelocity.z);

        Vector3 nextHorizontal = Vector3.MoveTowards(
            currentHorizontal,
            desiredHorizontal,
            activeAcceleration * Time.fixedDeltaTime);

        body.linearVelocity = new Vector3(
            nextHorizontal.x,
            currentVelocity.y,
            nextHorizontal.z);
    }
    private void TryJump(bool grounded)
    {
        if (jumpRequested && grounded)
        {
            body.AddForce(
                Vector3.up * config.jumpImpulse,
                ForceMode.Impulse);
        }

        jumpRequested = false;
    }

    private void Update()
    {
        ReadMoveInput();
    }

    private void FixedUpdate()
    {
        bool grounded = IsGrounded();

        float activeMoveSpeed = GetActiveMoveSpeed();
        float activeAcceleration =
            GetActiveAcceleration(grounded);

        ApplyHorizontalMovement(
            activeMoveSpeed,
            activeAcceleration);

        TryJump(grounded);
    }
    private void QueueJump(InputAction.CallbackContext context)
    {
        jumpRequested = true;
    }
    private bool IsGrounded()
    {
        float distance =
        bodyCollider.bounds.extents.y + groundPadding;
        return Physics.Raycast(
        bodyCollider.bounds.center,
        Vector3.down,
        distance,
        groundLayers,
        QueryTriggerInteraction.Ignore);
    }
    public void SetRespawnPosition(Vector3 newPosition)
    {
        currentRespawnPosition = newPosition;
    }
    public void Respawn()
    {
        body.position = currentRespawnPosition;
        body.rotation = Quaternion.identity;
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Collider currentCollider = bodyCollider != null
        ? bodyCollider
        : GetComponent<Collider>();
        if (currentCollider == null)
        {
            return;
        }
        float distance =
        currentCollider.bounds.extents.y + groundPadding;
        Vector3 origin = currentCollider.bounds.center;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
        origin,
        origin + Vector3.down * distance);
    }
}