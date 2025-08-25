using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject[] enableOnNodeUsage;
    public GameObject[] disableOnNodeUsage;

    public virtual void Activate()
    {
        foreach (var obj in enableOnNodeUsage)
            obj.SetActive(true);
        foreach (var obj in disableOnNodeUsage)
            obj.SetActive(false);
    }
}

public enum NodeDirection
{
    Straight,
    Left,
    Right
}
