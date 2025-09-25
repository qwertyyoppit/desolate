using System;
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

    public float walkAcceleration;
    public float walkMaxAcceleration;
    public float walkMaxSpeed;
    public float rotateSpeed = 120f;

    [SerializeField] private float jumpForce;

    [SerializeField] private float groundCheck;
    [SerializeField] private float checkStart;

    [NonSerialized] private bool grounded;
    [NonSerialized] public bool crouching;
    [NonSerialized] public bool jumping;

    private Vector2 movement;

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
            transform.TransformPoint(new Vector3(ActiveCollider.center.x, ActiveCollider.bounds.min.y + ActiveCollider.radius/2, ActiveCollider.center.z)), 
            ActiveCollider.radius - 0.02f,
            -transform.up,
            out var _,
            groundCheck + 0.02f, 
            LayerMask.GetMask("Terrain"));
    }

    private void Update()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>().normalized;
        var moveDir = (cam.transform.forward * movement.y + cam.transform.right * movement.x);
        moveDir.y = 0f;
        moveDir.Normalize();
        var topVelocity = moveDir * (walkMaxSpeed * (crouching ? 0.6f : 1));//+ velocity of platform below
        var moveVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        var magnitude = walkAcceleration * Time.deltaTime;
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
