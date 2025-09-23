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

    [SerializeField] private Transform following;
    [SerializeField] private Vector3 offset;
    private float _sinTime;

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

        var sinValue = Mathf.Cos((_sinTime-0.5f) * Mathf.PI);

        var sinAmount = -Mathf.Abs(effectIntensity * sinValue);

        Vector3 sinAmountX = effectIntensityX * Mathf.Cos(_sinTime) * following.right;

        transform.position += new Vector3(
            0,
            -sinAmount,
            0);
    }
}
