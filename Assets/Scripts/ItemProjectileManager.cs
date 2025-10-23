using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemProjectileManager : MonoBehaviour
{
    [SerializeField] private ProjectileData projectileData;
    private Item item;
    private PlayerProjectileManager playerProjectileManager;

    public List<Projectile> projectiles = new();

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
        item.usageStarted += FireProjectile;
    }

    private void OnDisable()
    {
        item.usageStarted -= FireProjectile;
    }

    private void FireProjectile(ItemUsageStartedArgs args)
    {
        if (args.activationType == ItemActivationTypes.Primary)
            FireProjectile();
    }

    public void FireProjectile()
    {
        var projectile = playerProjectileManager.FireProjectile(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 2f)) - transform.position).normalized, projectileData);
        projectiles.Add(projectile);
        projectileFired?.Invoke(projectile);
        projectile.projectileHit += OnProjectileHit;
        projectile.projectileDestroyed += OnProjectileDestroyed;
    }

    private void OnProjectileHit(Projectile projectile)
    {
        projectileHit?.Invoke(projectile);
    }

    private void OnProjectileDestroyed(Projectile projectile)
    {
        projectiles.Remove(projectile);
    }
}
