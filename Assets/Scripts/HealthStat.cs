using UnityEngine;

public class HealthStat : Stat
{
    public HealthStat()
    {
        key = "health";
        type = typeof(float);
    }
}
public class MaxHealthStat : Stat
{
    public MaxHealthStat()
    {
        key = "maxHealth";
        type = typeof(float);
    }
}
