using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Section : MonoBehaviour
{
    public GameObject SectionGroups;
    public GameObject EntryNodes;
    public GameObject ExitNodes;

    public bool Indoors;
    
    public EntryNode[] ValidEntryNodes => EntryNodes.GetComponentsInChildren<EntryNode>(true).ToArray();

    public ExitNode[] ValidExitNodes => ExitNodes.GetComponentsInChildren<ExitNode>(true).ToArray();

    public EntryNode[] GetEntryNodesOfDirection(PathDirection direction)
    {
        return ValidEntryNodes.Where(node => node.direction == direction).ToArray();
    }
    
    public ExitNode[] GetExitNodesOfDirection(PathDirection direction)
    {
        return ValidExitNodes.Where(node => node.direction == direction).ToArray();
    }
}
