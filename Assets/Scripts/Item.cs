using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public string ItemName;

    public Dictionary<Type, object> Stats = new();

    private void AddStat<T, V>(T stat) where T : Stat<V>
    {
        Stats[typeof(T)] = stat;
    }

    private void Awake()
    {
        ItemName = Data.Name;
        AddStat<Stat<float>, float>(new Stat<float>(5f));
    }
}