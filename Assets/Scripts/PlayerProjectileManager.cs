using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileManager : MonoBehaviour
{
    public static PlayerProjectileManager instance;

    public List<Projectile> projectiles = new();

    public Action<Projectile> projectileFired = delegate { };
    public Action<Projectile> projectileHit = delegate { };
    private void Awake()
    {
        instance = this;
    }

    public Projectile FireProjectile(Vector3 position, Vector3 direction, ProjectileData data)
    {
        var projectile = Projectile.CreateProjectile(position, direction, data);
        projectiles.Add(projectile);
        projectileFired?.Invoke(projectile);
        projectile.projectileHit += OnProjectileHit;
        projectile.projectileDestroyed += OnProjectileDestroy;
        return projectile;
    }

    private void OnProjectileHit(Projectile projectile)
    {
        projectileHit?.Invoke(projectile);
    }

    private void OnProjectileDestroy(Projectile projectile)
    {
        projectiles.Remove(projectile);
    }
}
