using UnityEngine;
using UnityEngine.InputSystem;

public class Refs : MonoBehaviour
{
    private static Player _player;
    public static Player player
    {
        get
        {
            if (!_player)
            {
                _player = FindAnyObjectByType<Player>();
            }
            return _player;
        }
    }

    private static PlayerInput _playerInput;
    public static PlayerInput playerInput
    {
        get
        {
            if (!_playerInput)
            {
                _playerInput = FindAnyObjectByType<PlayerInput>();
            }
            return _playerInput;
        }
    }
}
