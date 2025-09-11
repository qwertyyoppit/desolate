using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFirstPersonLook : MonoBehaviour
{
    private float pitch;
    private float yaw;

    [SerializeField] private Transform player;
    [Space]
    [SerializeField] private float lookSpeed;

    private void Update()
    {
        transform.position = player.position;
        if (Cursor.lockState == CursorLockMode.None)
            return;
        var look = Mouse.current.delta.value * (lookSpeed * Time.deltaTime);

        pitch -= look.y;
        yaw += look.x;
        pitch = Mathf.Clamp(pitch, -89.5f, 89.5f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }
}
