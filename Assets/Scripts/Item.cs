using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public bool Autouse;
    public float UseTime;
    public string ItemName;

    public Transform GripPosition;

    public Action<ItemAttemptUsageArgs> attemptedUsage = delegate { };
    public Action<ItemUsageStartedArgs> usageStarted = delegate { };
    public Action<ItemUsageStoppedArgs> usageStopped = delegate { };

    private bool wasUsed;

    public void Use(ItemActivationTypes activationType = ItemActivationTypes.Primary)
    {
        wasUsed = false;
        var attemptUsageArgs = new ItemAttemptUsageArgs() { activationType = activationType };
        attemptedUsage?.Invoke(attemptUsageArgs);

        if (attemptUsageArgs.canceled) return;

        var usageStartedArgs = new ItemUsageStartedArgs() { activationType = activationType };
        usageStarted?.Invoke(usageStartedArgs);

        wasUsed = true;

        //explodistosion
    }

    private void Update()
    {
        if (!wasUsed)
        {
            var usageStoppedArgs = new ItemUsageStoppedArgs();
            usageStopped?.Invoke(usageStoppedArgs);
        }
    }

    public static Item CreateItem(ItemData data = null, Transform parent = null)
    {
        var itemObj = Instantiate(data.ItemPrefab);
        if (parent) itemObj.transform.SetParent(parent);
        var item = itemObj.GetComponent<Item>();
        item.Data = data;
        itemObj.name = item.Data.Name;
        item.ItemName = item.Data.Name;
        item.UseTime = item.Data.UseTime;
        return item;
    }
}

public enum ItemActivationTypes
{
    Primary,
    Secondary,
    Tertiary
}