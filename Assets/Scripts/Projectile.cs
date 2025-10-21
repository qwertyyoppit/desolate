using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Action<Projectile> projectileHit = delegate { };
    public float Speed;
    public RaycastHit HitInfo;

    private Ray projectileRay;

    public static Projectile CreateProjectile(Vector3 position, Vector3 direction, float speed, GameObject prefab = null)
    {
        var shotObj = prefab ? Instantiate(prefab) : new GameObject();
        shotObj.transform.position = position;
        shotObj.transform.forward = direction;
        var shotScript = shotObj.AddComponent<Projectile>();
        shotScript.Speed = speed;
        return shotScript;
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
