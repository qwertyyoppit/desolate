using System;
using UnityEngine;

public class PlayerProjectileManager : MonoBehaviour
{
    public Action<Projectile> projectileFired = delegate { };
    public Action<Projectile> projectileHit = delegate { };
}
