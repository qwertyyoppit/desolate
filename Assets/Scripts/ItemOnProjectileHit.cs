using UnityEngine;

[RequireComponent(typeof(Item))]
public abstract class ItemOnProjectileHit : MonoBehaviour
{
    protected ItemProjectileManager itemProjectile;

    private void Start()
    {
        itemProjectile = GetComponent<ItemProjectileManager>();

        itemProjectile.projectileHit += OnHit;
    }

    protected abstract void OnHit(Projectile shot);
}
