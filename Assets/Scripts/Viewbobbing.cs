using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera)), RequireComponent(typeof(CameraFirstPersonLook))]
public class Viewbobbing : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction playerMovement;
    private CameraFirstPersonLook firstPersonCam;
    private Player player;

    [SerializeField] private float effectIntensity;
    [SerializeField] private float effectIntensityX;
    [SerializeField] private float effectSpeed;
    [SerializeField] private float effectSpeedX;

    [SerializeField] private Transform following;
    [SerializeField] private Vector3 offset;
    private float _sinTime;
    private float _sinXTime;

    private void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        if (playerInput)
        {
            playerMovement = playerInput.actions["Move"];
        }
        player = FindAnyObjectByType<Player>();
        firstPersonCam = GetComponent<CameraFirstPersonLook>();

        firstPersonCam.CameraUpdated += Apply;
    }

    private void Apply()
    {
        if (!player) return;
        var move = playerMovement.ReadValue<Vector2>();

        _sinTime = Mathf.MoveTowards(_sinTime, move.magnitude > 0 && !player.jumping ? 1f : 0.5f, Time.deltaTime * effectSpeed * (player.crouching ? 0.5f : 1f)) % 1;

        _sinXTime = Mathf.MoveTowards(_sinXTime, move.magnitude > 0 && !player.jumping ? 2f : 1f, Time.deltaTime * effectSpeedX * (player.crouching ? 0.5f : 1f)) % 2;

        var sinValue = Mathf.Sin((_sinTime-0.5f) * Mathf.PI);

        var sinAmount = -Mathf.Abs(effectIntensity * sinValue);

        Vector3 sinAmountX = effectIntensityX * Mathf.Sin((_sinXTime) * Mathf.PI) * transform.right;

        transform.position += new Vector3(
            sinAmountX.x,
            sinAmountX.y + sinAmount,
            sinAmountX.z);
    }
}
