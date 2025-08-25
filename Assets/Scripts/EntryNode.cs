using UnityEngine;

public class EntryNode : Node
{
    [Header("NOTE: direction for entry relates to the way it comes in, not the side of the room.\nFor example, an entry on the left side of a section would have Right direction.")]
    public PathDirection direction;
}