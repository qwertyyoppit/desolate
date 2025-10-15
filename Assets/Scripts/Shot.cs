using System;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public Action<Shot> shotHit = delegate { };
    public Vector3 Direction;
    public float Speed;
    public RaycastHit HitInfo;

    private Ray shotRay;

    public static Shot CreateShot(Vector3 position, Vector3 direction, float speed, GameObject prefab = null)
    {
        var shotObj = prefab ? Instantiate(prefab) : new GameObject();
        shotObj.transform.position = position;
        shotObj.transform.forward = direction;
        var shotScript = shotObj.AddComponent<Shot>();
        shotScript.Direction = direction;
        shotScript.Speed = speed;
        return shotScript;
    }

    private void Update()
    {
        transform.forward = Direction;
        shotRay = new Ray(transform.position, Direction);
        var hit = Physics.Raycast(shotRay, out RaycastHit HitInfo, Speed * Time.deltaTime);
        if (hit)
        {
            shotHit?.Invoke(this);
            Destroy(gameObject);
            return;
        }

        transform.position = transform.TransformPoint(new Vector3(0, 0, Speed * Time.deltaTime));
    }
}
