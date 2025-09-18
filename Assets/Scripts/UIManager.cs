using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ToggleFreeCursor()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
