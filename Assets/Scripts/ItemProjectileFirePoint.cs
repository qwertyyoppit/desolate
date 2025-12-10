using UnityEngine;

public class ItemProjectileFirePoint : MonoBehaviour
{
    private ItemProjectileManager projectileManager;

    [SerializeField] private Transform firePoint;

    private void Start()
    {
        projectileManager = GetComponent<ItemProjectileManager>();
        projectileManager.projectileFired += ProjectileFired;
    }

    private void ProjectileFired(Projectile projectile)
    {
        projectile.transform.position = firePoint.position;
    }
}
