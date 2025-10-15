using System;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemProjectileUsage : MonoBehaviour
{
    private Item item;

    public Action<Shot> projectileHit = delegate { };

    private void Start()
    {
        item = GetComponent<Item>();

        item.usageStarted += FireShot;
    }

    private void FireShot(ItemUsageStartedArgs args)
    {
        print((Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 2f)) - transform.position).normalized);
        var shot = Shot.CreateShot(transform.position, (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 2f)) - transform.position).normalized, 5f);
        shot.shotHit += OnShotHit;
    }

    private void OnShotHit(Shot shot)
    {
        projectileHit?.Invoke(shot);
    }
}
