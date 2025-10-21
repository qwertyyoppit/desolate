using System;
using UnityEngine;

public class PlayerProjectileManager : MonoBehaviour
{
    public static PlayerProjectileManager instance;

    public Action<Projectile> projectileFired = delegate { };
    public Action<Projectile> projectileHit = delegate { };
    private void Awake()
    {
        instance = this;
    }
}
