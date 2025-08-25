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
        grounded = Physics.CapsuleCast(
            new Vector3(ActiveCollider.center.x, ActiveCollider.bounds.max.y - ActiveCollider.radius, ActiveCollider.center.z),
            new Vector3(ActiveCollider.center.x, ActiveCollider.bounds.min.y + ActiveCollider.radius, ActiveCollider.center.z),
            ActiveCollider.radius - 0.01f,
            Vector3.down,
            groundCheck + 0.01f, 
            LayerMask.GetMask("Terrain"));
    }

    private void Update()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>().normalized;
        var moveDir = (cam.transform.forward * movement.y + cam.transform.right * movement.x);
        moveDir.y = 0f;
        moveDir.Normalize();
        var topVelocity = moveDir * (walkMaxSpeed);//+ velocity of platform below
        var force = Vector3.MoveTowards(rb.linearVelocity, topVelocity, walkAcceleration * Time.deltaTime);
        
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
