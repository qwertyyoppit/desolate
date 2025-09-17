using System;
using UnityEngine;

public class Sanity
{
    public static Action<SanityEffectArgs> SanityEffect = delegate { };
}

public class SanityEffectArgs
{
    public readonly SanityEffectType EffectType;
    public readonly float Duration;
    public readonly float SanityAmount;

    public readonly GameObject Invoker;

    public SanityEffectArgs(SanityEffectType effectType, float sanityAmount, float duration = 0f, GameObject invoker = null)
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