using System.Collections.Generic;
using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    public float sanity = 100f;

    public List<SanityOvertimeEffect> sanityDrainEffects = new();

    private void Start()
    {
        Sanity.SanityEffect += AffectSanity;
    }

    private void Update()
    {
        foreach (var effect in sanityDrainEffects.ToArray())
        {
            sanity = Mathf.Lerp(sanity, sanity - effect.Amount, Mathf.Min(Time.deltaTime, effect.Duration));
            effect.Duration -= Time.deltaTime;
            if (effect.Duration <= 0)
            {
                sanityDrainEffects.Remove(effect);
            }
        }
    }

    private void AffectSanity(SanityEffectArgs args)
    {
        switch (args.EffectType)
        {
            case SanityEffectType.Instant:
                sanity -= args.SanityAmount;
                return;
            case SanityEffectType.OverTime:
                sanityDrainEffects.Add(new SanityOvertimeEffect(args.SanityAmount, args.Duration));
                return;
        }
    }
}

public class SanityOvertimeEffect
{
    public float Amount;
    public float Duration;

    public SanityOvertimeEffect(float amount, float duration)
    {
        Amount = amount;
        Duration = duration;
    }
}
