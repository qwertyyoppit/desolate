using UnityEngine;

public class StatOperationEvaluation
{
    public static float Evaluate(float baseValue, float statValue, StatOperationType type)
    {
        var result = type switch
        {
            StatOperationType.Additive => baseValue + statValue,
            StatOperationType.Multiplicative => baseValue * statValue,
            StatOperationType.Override => statValue,
            _ => statValue
        };

        return result;
    }
}
