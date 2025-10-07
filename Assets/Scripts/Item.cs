using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public string ItemName;
    public void Activate()
    {
        Sanity.SanityEffect?.Invoke(new SanityEffectArgs(SanityEffectType.OverTime, -10.0f, 1.0f));
    }

    private void Awake()
    {
        ItemName = Data.Name;
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