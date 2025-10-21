using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Action<Projectile> projectileHit = delegate { };
    public float Speed;
    public RaycastHit HitInfo;

    private Ray projectileRay;

    public static Projectile CreateProjectile(Vector3 position, Vector3 direction, ProjectileData data)
    {
        var shotObj = data ? Instantiate(data.Prefab) : new GameObject();
        shotObj.transform.position = position;
        shotObj.transform.forward = direction;
        var projectile = shotObj.TryGetComponent<Projectile>(out var projFound) ? projFound : shotObj.AddComponent<Projectile>();
        projectile.Speed = data.Speed;
        return projectile;
    }

    private void Update()
    {
        projectileRay = new Ray(transform.position, transform.forward);
        var hit = Physics.Raycast(projectileRay, out HitInfo, Speed * Time.deltaTime);
        if (hit)
        {
            projectileHit?.Invoke(this);
            Destroy(gameObject);
            return;
        }

        transform.position += transform.forward * (Speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        projectileHit?.Invoke(this);
        Destroy(gameObject);
    }
}
