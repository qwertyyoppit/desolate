using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private CapsuleCollider standCollider;
    [SerializeField] private CapsuleCollider crouchCollider;

    private CapsuleCollider ActiveCollider => crouching ? crouchCollider : standCollider;

    public float walkAcceleration;
    public float walkMaxSpeed;
    public float rotateSpeed = 120f;
    public float crouchSpeedFactor;
    public float sprintSpeedFactor;

    public float effectiveSpeedScale => (sprinting ? sprintSpeedFactor : 1) * (crouching ? crouchSpeedFactor : 1);
    public float effectiveMaxSpeed => walkMaxSpeed * effectiveSpeedScale;

    public float jumpForce;

    [SerializeField] private float groundCheck;
    [SerializeField] private float checkStart;

    public bool grounded;
    public bool sprinting;
    public bool crouching;
    public bool jumping;

    public Vector2 movement;

    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerInput.actions["Crouch"].started += _ => StartCrouch();
        playerInput.actions["Crouch"].canceled += _ => StopCrouch();
        playerInput.actions["Jump"].started += _ => StartJump();
        playerInput.actions["Jump"].canceled += _ => StopJump();
        playerInput.actions["Exit"].started += _ => UIManager.ToggleFreeCursor();
    }

    private void FixedUpdate()
    {
        grounded = Physics.SphereCast(
            transform.TransformPoint(new Vector3(ActiveCollider.center.x, ActiveCollider.center.y - ActiveCollider.height/2 + ActiveCollider.radius/2, ActiveCollider.center.z)), 
            ActiveCollider.radius - checkStart,
            -transform.up,
            out var _,
            groundCheck + checkStart, 
            LayerMask.GetMask("Terrain"));
    }

    private void Update()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>().normalized;
        sprinting = !crouching && playerInput.actions["Sprint"].IsPressed();
        var moveDir = (cam.transform.forward * movement.y + cam.transform.right * movement.x);
        moveDir.y = 0;
        moveDir.Normalize();
        var topVelocity = moveDir * effectiveMaxSpeed;//+ velocity of platform below
        var moveVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        var magnitude = walkAcceleration * effectiveSpeedScale * Time.deltaTime;
        if (Vector3.Dot(moveVelocity, moveDir) < 0) magnitude *= 2;
        var force = Vector3.MoveTowards(moveVelocity, topVelocity, magnitude);
        
        rb.linearVelocity = new Vector3(force.x, rb.linearVelocity.y, force.z);
        transform.forward = Vector3.RotateTowards(transform.forward, moveDir, rotateSpeed * Time.deltaTime, 0.0f);
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

    private void StartJump()
    {
        if (!grounded)
            return;
        jumping = true;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private void StopJump()
    {
        if (!jumping) return;
        print(jumping);
        jumping = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Min(rb.linearVelocity.y, rb.linearVelocity.y * 0.1f), rb.linearVelocity.z);
    }
}
