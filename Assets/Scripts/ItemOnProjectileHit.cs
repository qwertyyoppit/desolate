using UnityEngine;

[RequireComponent(typeof(Item))]
public abstract class ItemOnProjectileHit : MonoBehaviour
{
    protected ItemProjectileUsage itemProjectile;

    private void Start()
    {
        itemProjectile = GetComponent<ItemProjectileUsage>();

        itemProjectile.projectileHit += OnHit;
    }

    protected abstract void OnHit(Shot shot);
}
