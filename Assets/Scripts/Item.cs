using System;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData Data;
    public string ItemName;
    public void Activate()
    {
        Sanity.SanityEffect?.Invoke(new SanityEffectArgs(SanityEffectType.OverTime, -10.0f, 1.0f));
    }
}