using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private BoxCollider standBox;
    [SerializeField] private BoxCollider crouchBox;
    [SerializeField] private BoxCollider standGroundedBox;
    [SerializeField] private BoxCollider crouchGroundedBox;

    private BoxCollider ActiveBox => crouching ? crouchBox : standBox;

    [SerializeField] private float walkAcceleration;
    [SerializeField] private float walkMaxSpeed;

    [SerializeField] private float jumpForce;

    private bool grounded;
    [SerializeField] private float groundCheck;

    private bool crouching;

    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Crouch"].started += _ => StartCrouch();
        playerInput.actions["Crouch"].canceled += _ => StopCrouch();
        playerInput.actions["Jump"].started += _ => Jump();
    }

    private void OnCollisionExit(Collision other)
    {
        grounded = false;
    }

    private void OnCollisionStay(Collision other)
    {
        grounded = other.gameObject.layer == 3 || grounded;
    }

    private void Update()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>().normalized;
        var moveDir = Camera.main.transform.forward.normalized * movement.y + Camera.main.transform.right.normalized * movement.x;
        moveDir.y = 0f;
        moveDir.Normalize();
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f) + moveDir * (walkAcceleration);
        if (new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude > walkMaxSpeed)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f) + new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).normalized * walkMaxSpeed;
        }
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
        standBox.enabled = false;
        standGroundedBox.enabled = false;
        crouchBox.enabled = true;
        crouchGroundedBox.enabled = true;
    }

    private void StopCrouch()
    {
        crouching = false;
        standBox.enabled = true;
        standGroundedBox.enabled = true;
        crouchBox.enabled = false;
        crouchGroundedBox.enabled = false;
    }

    private void Jump()
    {
        if (!grounded)
            return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
