using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFirstPersonLook : MonoBehaviour
{
    private PlayerInventory playerInventory;

    public Transform player;

    [Space]
    [SerializeField] private float lookSpeed;
    [Header("Item Viewmodel")]
    [SerializeField] private Vector3 itemOffset;
    [SerializeField] private Vector3 itemRotationOffset;

    private float pitch;
    private float yaw;

    public Action CameraUpdated = delegate { };

    private void Start()
    {
        playerInventory = Refs.PlayerInventory;
    }

    private void LateUpdate()
    {
        transform.position = player.position;
        if (Cursor.lockState == CursorLockMode.None)
        {
            CameraUpdated?.Invoke();
            return;
        }
        var look = Mouse.current.delta.value * (lookSpeed * Time.deltaTime);

        pitch -= look.y;
        yaw += look.x;
        pitch = Mathf.Clamp(pitch, -89.5f, 89.5f);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0.0f);

        var item = playerInventory.HeldItem;
        item.transform.eulerAngles = transform.eulerAngles + item.GripPosition.eulerAngles - item.transform.eulerAngles;
        item.transform.position = transform.position;
        var offset = itemOffset + item.GripPosition.position - item.transform.position;
        item.transform.position += offset.x * transform.right + offset.y * transform.up + offset.z * transform.forward;

        CameraUpdated?.Invoke();
    }
}
