using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public string ItemName;

    public Action<ItemAttemptUsageArgs> attemptedUsage = delegate { };
    public Action<ItemUsageStartedArgs> usageStarted = delegate { };
    public Action<ItemUsageStoppedArgs> usageStopped = delegate { };

    private bool wasUsed;
    private bool usage;

    public void Use()
    {
        wasUsed = false;
        var attemptUsageArgs = new ItemAttemptUsageArgs();
        attemptedUsage?.Invoke(attemptUsageArgs);
        if (attemptUsageArgs.canceled) return;

        var usageStartedArgs = new ItemUsageStartedArgs();
        usageStarted?.Invoke(usageStartedArgs);

        wasUsed = true;

        Sanity.SanityEffect?.Invoke(new SanityEffectArgs(SanityEffectType.OverTime, -10.0f, 1.0f));
    }

    private void Awake()
    {
        ItemName = Data ? Data.Name : "boo";
    }

    private void Update()
    {
        usage = false || wasUsed;
        if (!usage)
        {
            var usageStoppedArgs = new ItemUsageStoppedArgs();
            usageStopped?.Invoke(usageStoppedArgs);
        }
    }

    public static Item CreateItem(ItemData data = null, Transform parent = null)
    {
        var itemObj = new GameObject();
        if (parent) itemObj.transform.SetParent(parent);
        var item = itemObj.AddComponent<Item>();
        item.Data = data;
        itemObj.name = item.ItemName ?? "boo";
        return item;
    }
}