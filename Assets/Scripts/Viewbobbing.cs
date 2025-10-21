using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera)), RequireComponent(typeof(CameraFirstPersonLook))]
public class Viewbobbing : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction playerMovement;
    private CameraFirstPersonLook firstPersonCam;
    private PlayerMovement player;

    [SerializeField] private float effectIntensity;
    [SerializeField] private float effectIntensityX;
    [SerializeField] private float effectSpeed;
    [SerializeField] private float effectSpeedX;
    [SerializeField] private float effectAmount;
    [SerializeField] private float effectAmountX;

    [SerializeField] private Transform following;
    [SerializeField] private Vector3 offset;
    private float _sinTime = 2f;
    private float _sinXTime;

    private void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        if (playerInput)
        {
            playerMovement = playerInput.actions["Move"];
        }
        player = Refs.PlayerMovement;
        firstPersonCam = GetComponent<CameraFirstPersonLook>();

        firstPersonCam.CameraUpdated += Apply;
    }

    private void Apply()
    {
        if (!player) return;
        var move = playerMovement.ReadValue<Vector2>();
        var moving = move.sqrMagnitude > 0;

        var playerBody = player.GetComponent<Rigidbody>();

        _sinTime = Mathf.MoveTowards(_sinTime, moving && player.grounded ? 2f : 1f, Time.deltaTime * effectSpeed * (new Vector3(playerBody.linearVelocity.x, 0f, playerBody.linearVelocity.z).magnitude) / player.effectiveMaxSpeed) % 2;

        var sinValue = Mathf.Sin((_sinTime) * Mathf.PI);

        effectAmount = moving ? Mathf.MoveTowards(effectAmount, -Mathf.Abs(effectIntensity * sinValue), effectSpeedX * Time.deltaTime) : Mathf.MoveTowards(effectAmount, 0, effectSpeedX * Time.deltaTime);

        effectAmountX = moving ? Mathf.MoveTowards(effectAmountX, effectIntensityX * Mathf.Sin((_sinTime) * Mathf.PI), effectSpeedX * Time.deltaTime) : Mathf.MoveTowards(effectAmountX, 0, effectSpeedX * Time.deltaTime);

        Vector3 sinAmountX = effectAmountX * transform.right;

        transform.position += new Vector3(
            sinAmountX.x,
            sinAmountX.y + effectAmount,
            sinAmountX.z);
    }
}
