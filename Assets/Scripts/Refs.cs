using UnityEngine;
using UnityEngine.InputSystem;

public class Refs : MonoBehaviour
{
    private static GameObject _player;
    public static GameObject Player
    {
        get
        {
            if (!_player)
            {
                _player = GameObject.FindWithTag("Player");
            }
            return _player;
        }
    }

    private static PlayerMovement _playerMovement;
    public static PlayerMovement PlayerMovement
    {
        get
        {
            if (!_playerMovement)
            {
                _playerMovement = FindAnyObjectByType<PlayerMovement>();
            }
            return _playerMovement;
        }
    }

    private static PlayerInput _playerInput;
    public static PlayerInput PlayerInput
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

    private static PlayerInventory _playerInventory;
    public static PlayerInventory PlayerInventory
    {
        get
        {
            if (!_playerInput)
            {
                _playerInventory = FindAnyObjectByType<PlayerInventory>();
            }
            return _playerInventory;
        }
    }

    private static PlayerProjectileManager _playerProjectileManager;
    public static PlayerProjectileManager PlayerProjectileManager
    {
        get
        {
            if (!_playerProjectileManager)
            {
                _playerProjectileManager = FindAnyObjectByType<PlayerProjectileManager>();
            }
            return _playerProjectileManager;
        }
    }
}
