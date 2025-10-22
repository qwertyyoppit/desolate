using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLerpColorOfHit : MonoBehaviour
{
    private PlayerProjectileManager playerProjectileManager;

    [SerializeField] private Color hitColor;

    private class MaterialTrack { public float amount; public Color baseColor; }
    Dictionary<Material, MaterialTrack> trackedMaterials = new();

    private void Start()
    {
        playerProjectileManager = PlayerProjectileManager.instance;
        playerProjectileManager.projectileHit += OnHit;
    }

    private void Update()
    {
        List<Material> toRemove = new();
        foreach (var tracked in trackedMaterials)
        {
            tracked.Key.color = Color.Lerp(tracked.Value.baseColor, hitColor, tracked.Value.amount);
            tracked.Value.amount -= Time.deltaTime;
            if (tracked.Value.amount < 0)
            {
                tracked.Key.color = tracked.Value.baseColor;
                toRemove.Add(tracked.Key);
            }
        }
        foreach (var removed in toRemove)
        {
            trackedMaterials.Remove(removed);
        }
    }

    private void OnHit(Projectile projectile)
    {
        if (projectile.HitInfo.collider.TryGetComponent<MeshRenderer>(out var renderer) && renderer.material)
        {
            if (trackedMaterials.TryGetValue(renderer.material, out var track))
            {
                track.amount = 1.0f;
                return;
            }
            trackedMaterials.Add(renderer.material, new MaterialTrack() { amount = 1.0f, baseColor = renderer.material.color });
        }    
    }
}

