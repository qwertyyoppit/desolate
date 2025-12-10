using UnityEngine;

public class ItemProjectileDamageEnemy : MonoBehaviour
{
    public float Damage;

    private ItemProjectileManager projectileManager;

    private void Start()
    {
        projectileManager = GetComponent<ItemProjectileManager>();

        projectileManager.projectileHit += OnHit;
    }

    private void OnHit(Projectile projectile)
    {
        var obj = projectile.HitInfo.collider.gameObject;
        if (obj.CompareTag("enemy"))
        {
            var health = obj.GetComponent<Health>();
            health.Damage(Damage);
        }
    }
}
