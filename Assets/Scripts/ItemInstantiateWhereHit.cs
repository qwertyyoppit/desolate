using UnityEngine;

public class ItemInstantiateWhereHit : ItemOnProjectileHit
{
    [SerializeField] private GameObject InstantiatedObject;

    protected override void OnHit(Projectile shot)
    {
        if (!InstantiatedObject) return;
        var obj = Instantiate(InstantiatedObject);
        obj.transform.position = shot.HitInfo.point;
    }
}
