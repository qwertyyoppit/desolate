using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class Viewbobbing : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction playerMovement;

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
    }

    private void LateUpdate()
    {
        var move = playerMovement.ReadValue<Vector2>();
        if (move.magnitude <= 0)
        {
            _sinTime = 0f;
            return;
        }

        _sinTime += Time.deltaTime;
        var sinValue = Mathf.Sin(_sinTime);

        var sinAmount = Mathf.Abs(effectIntensity * sinValue);

        Vector3 sinAmountX = following.right * Mathf.Cos(_sinTime) * effectIntensityX;

        
    }
}
