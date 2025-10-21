using UnityEngine;

public class PlayerOrangeMaterialOnHit : MonoBehaviour
{
    private PlayerProjectileManager playerProjectileManager;
    private Material material;

    private void Awake()
    {
        playerProjectileManager = GetComponent<PlayerProjectileManager>();

    }
}
