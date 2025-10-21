using System;

[System.Serializable]
public class Stat
{
    public string key;
    public Type type;
    public Stat()
    {
        type = typeof(float);
    }
}

public class StatValue<T>
{
    public T value;
    public StatValue()
    {

    }
}