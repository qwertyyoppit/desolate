[System.Serializable]
public class Stat<T> : IStat
{
    public T value;

    public Stat(T value)
    {
        this.value = value;
    }
}

public interface IStat { }