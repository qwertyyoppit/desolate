using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private CapsuleCollider standCollider;
    [SerializeField] private CapsuleCollider crouchCollider;

    private CapsuleCollider ActiveCollider => crouching ? crouchCollider : standCollider;

    [SerializeField] private float walkAcceleration;
    [SerializeField] private float walkMaxAcceleration;
    [SerializeField] private float walkMaxSpeed;

    [SerializeField] private float jumpForce;

    private bool grounded;
    [SerializeField] private float groundCheck;
    [SerializeField] private float checkStart;

    private bool crouching;

    private Vector2 movement;

    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Crouch"].started += _ => StartCrouch();
        playerInput.actions["Crouch"].canceled += _ => StopCrouch();
        playerInput.actions["Jump"].started += _ => Jump();
    }

    private void FixedUpdate()
    {
        grounded = Physics.SphereCast(
            new Vector3(ActiveCollider.center.x, ActiveCollider.bounds.min.y + ActiveCollider.radius - checkStart, ActiveCollider.center.z),
            ActiveCollider.radius,
            Vector3.down,
            out var _,
            groundCheck + checkStart, 
            LayerMask.GetMask("Terrain"));
    }

    private void Update()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>().normalized;
        var moveDir = (cam.transform.forward * movement.y + cam.transform.right * movement.x);
        moveDir.y = 0f;
        moveDir.Normalize();
        var topVelocity = moveDir * (walkMaxSpeed);//+ velocity of platform below
        var moveVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        var magnitude = walkAcceleration * Time.deltaTime;
        if (Vector3.Dot(moveVelocity, moveDir) < 0) magnitude *= 2;
        var force = Vector3.MoveTowards(moveVelocity, topVelocity, magnitude);
        
        rb.linearVelocity = new Vector3(force.x, rb.linearVelocity.y, force.z);
    }

    private void LateUpdate()
    {
        if (playerInput.actions["Cancel"].triggered)
        {
            UIManager.ToggleFreeCursor();
        }
    }

    private void StartCrouch()
    {
        crouching = true;
        standCollider.enabled = false;
        crouchCollider.enabled = true;
    }

    private void StopCrouch()
    {
        crouching = false;
        standCollider.enabled = true;
        crouchCollider.enabled = false;
    }

    private void Jump()
    {
        if (!grounded)
            return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }
}
