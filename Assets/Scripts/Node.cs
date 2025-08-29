using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject[] enableOnNodeUsage;
    public GameObject[] disableOnNodeUsage;

    [Tooltip("If the node has an elevation which would not be considered to be the same 'story' as the lowest floor in the section." +
        "\nThis is used by the section generator to detect which sections have an elevation change and to then filter the nodes accordingly.")]
    public bool elevated;

    public void Activate(bool enabling = true)
    {
        if (enabling)
        {
            foreach (var obj in enableOnNodeUsage)
                obj.SetActive(true);
        }
        foreach (var obj in disableOnNodeUsage)
            obj.SetActive(false);
    }
}