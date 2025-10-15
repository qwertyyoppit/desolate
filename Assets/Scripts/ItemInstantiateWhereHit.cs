using UnityEngine;

public class ItemInstantiateWhereHit : ItemOnProjectileHit
{
    [SerializeField] private GameObject InstantiatedObject;

    protected override void OnHit(Shot shot)
    {
        var obj = Instantiate(InstantiatedObject);
        obj.transform.position = shot.HitInfo.point;
    }
}
