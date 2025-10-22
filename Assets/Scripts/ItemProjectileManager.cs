using System;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemProjectileManager : MonoBehaviour
{
    [SerializeField] private ProjectileData projectileData;
    private Item item;
    private PlayerProjectileManager playerProjectileManager;

    public Action<Projectile> projectileFired = delegate { };
    public Action<Projectile> projectileHit = delegate { };

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    private void Start()
    {
        playerProjectileManager = PlayerProjectileManager.instance;
    }

    private void OnEnable()
    {
        item.usageStarted += FireShot;
    }

    private void OnDisable()
    {
        item.usageStarted -= FireShot;
    }

    private void FireShot(ItemUsageStartedArgs args)
    {
        FireProjectile();
    }

    public void FireProjectile()
    {
        var shot = Projectile.CreateProjectile(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 2f)) - transform.position).normalized, projectileData);
        projectileFired?.Invoke(shot);
        playerProjectileManager.projectileFired?.Invoke(shot);
        shot.projectileHit += OnShotHit;
    }

    private void OnShotHit(Projectile shot)
    {
        projectileHit?.Invoke(shot);
        playerProjectileManager.projectileHit?.Invoke(shot);
    }
}
