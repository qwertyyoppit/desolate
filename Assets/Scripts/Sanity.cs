using System;
using UnityEngine;

public class Sanity
{
    public static Action<SanityEffectArgs> SanityEffect = delegate { };
}

[Serializable]
public class SanityEffectArgs
{
    public SanityEffectType EffectType;
    public float Duration;
    public float SanityAmount;

    public readonly GameObject Invoker;

    public SanityEffectArgs(SanityEffectType effectType = SanityEffectType.Instant, float sanityAmount = 10f, float duration = 0f, GameObject invoker = null)
    {
        EffectType = effectType;
        SanityAmount = sanityAmount;
        Duration = duration;
        Invoker = invoker;
    }
}

public enum SanityEffectType
{
    Instant,
    OverTime
}