using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemUsetime : MonoBehaviour
{
    private Item item;

    public float Cooldown;

    private void Start()
    {
        item = GetComponent<Item>();

        if (!item)
        {
            Debug.LogError($"Object {gameObject.name} is not an Item!");
            Destroy(this);
        }

        item.attemptedUsage += UsageCheck;
        item.usageStarted += StartCooldown;
    }

    private void Update()
    {
        if (Cooldown > 0) Cooldown -= Time.deltaTime;
    }

    private void UsageCheck(ItemAttemptUsageArgs args)
    {
        if (Cooldown > 0) args.canceled = true;
    }

    private void StartCooldown(ItemUsageStartedArgs args)
    {
        Cooldown = item.UseTime;
    }
}
