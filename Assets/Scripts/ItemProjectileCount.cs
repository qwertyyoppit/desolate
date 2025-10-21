using UnityEngine;

[RequireComponent(typeof(ItemProjectileManager))]
public class ItemProjectileCount : MonoBehaviour
{
    public int ProjectileAmount = 1;

    public float minShotDelay;
    public float maxShotDelay;

    private Item item;
    private ItemProjectileManager itemProjectileManager;

    private float delay;
    private int currentShotsRemaining;

    private void Awake()
    {
        item = GetComponent<Item>();
        itemProjectileManager = GetComponent<ItemProjectileManager>();

        item.attemptedUsage += OnAttemptUsage;
        item.usageStarted += OnUsage;
        itemProjectileManager.projectileFired += OnProjectileFire;
    }

    private void Update()
    {
        if (delay > 0)
            delay -= Time.deltaTime;
        if (currentShotsRemaining > 0 && delay <= 0)
        {
            itemProjectileManager.FireProjectile();
        }
    }

    private void OnAttemptUsage(ItemAttemptUsageArgs args)
    {
        if (currentShotsRemaining > 0 || delay > 0)
        {
            args.canceled = true;
        }
    }

    private void OnUsage(ItemUsageStartedArgs args)
    {
        currentShotsRemaining = ProjectileAmount;
        delay = Random.Range(minShotDelay, maxShotDelay);
    }

    private void OnProjectileFire(Projectile shot)
    {
        currentShotsRemaining -= 1;
        delay = Random.Range(minShotDelay, maxShotDelay);
    }
}
