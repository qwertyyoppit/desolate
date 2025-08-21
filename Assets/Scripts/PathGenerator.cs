using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private int segmentAmount = 15;
    [SerializeField] private int maxConsecutiveOutdoors = 8;
    [SerializeField] private int maxConsecutiveIndoors = 6;

    private List<PathSegment> pathSegments = new();

    public bool drawPathGizmo;

    private void Awake()
    {
        GeneratePath();
    }

    public void GeneratePath()
    {
        pathSegments.Clear();

        var cycleRemaining = 0;
        for (var i = 0; i < segmentAmount; i++)
        {
            var currentSegment = new PathSegment();
            pathSegments.Add(currentSegment);
            var validDirections = new List<PathDirections>
                { PathDirections.Straight, PathDirections.Left, PathDirections.Right };//, PathDirections.Up, PathDirections.Down };

            var validActions = new List<PathActions> { PathActions.Ascend, PathActions.Descend, PathActions.EnterBuilding, PathActions.ExitBuilding };

            var validLengths = new List<PathLengths> { PathLengths.Short, PathLengths.Long };
            var validWidths = new List<PathWidths> { PathWidths.Thin, PathWidths.Wide };

            var previousSegment = i > 0 ? pathSegments[i - 1] : null;
            if (previousSegment != null)
            {
                validActions.Remove(previousSegment.Action);

                switch (previousSegment.Direction)
                {
                    case PathDirections.Left or PathDirections.Right:
                        validDirections.Remove(PathDirections.Left);
                        validDirections.Remove(PathDirections.Right);
                        break;
                    case PathDirections.Up or PathDirections.Down:
                        validDirections.Remove(PathDirections.Up);
                        validDirections.Remove(PathDirections.Down);
                        break;
                }

                if (previousSegment.Action is PathActions.Ascend or PathActions.Descend)
                {
                    validActions.Remove(PathActions.Ascend);
                    validActions.Remove(PathActions.Descend);
                }
                if (previousSegment.Indoors)
                {
                    validActions.Remove(PathActions.EnterBuilding);
                    currentSegment.Indoors = true;
                }
                else
                {
                    validActions.Remove(PathActions.ExitBuilding);
                }
                if (Random.value > 0.5f)
                {
                    validLengths.Remove(previousSegment.Length);
                }
                else
                {
                    validWidths.Remove(previousSegment.Width);
                }
            }
            currentSegment.Direction = validDirections[Random.Range(0, validDirections.Count)];
            currentSegment.Action = Random.value < 0.8f ? PathActions.None : validActions[Random.Range(0, validActions.Count)];
            currentSegment.Length = validLengths[Random.Range(0, validLengths.Count)];
            currentSegment.Width = validWidths[Random.Range(0, validWidths.Count)];
            currentSegment.Indoors = currentSegment.Action switch
            {
                PathActions.EnterBuilding => true,
                PathActions.ExitBuilding => false,
                _ => currentSegment.Indoors
            };
        }
    }

    public void PrintPath()
    {
        foreach (var segment in pathSegments)
        {
            Debug.Log($"{segment.Direction}, {segment.Action}, {segment.Length}, {segment.Width}, {segment.Indoors}");
        }
    }

    public void EnablePathDraw()
    {
        drawPathGizmo = !drawPathGizmo;
    }

    private void OnDrawGizmos()
    {
        if (!drawPathGizmo)
            return;

        var currentPoint = Vector3.zero;
        var previousPoint = Vector3.zero;
        foreach (var segment in pathSegments)
        {
            Gizmos.color = segment.Indoors ? Color.blue : Color.red;

            currentPoint = segment.Direction switch
            {
                PathDirections.Straight => Vector3.forward,
                PathDirections.Left => Vector3.left,
                PathDirections.Right => Vector3.right,
                PathDirections.Up => Vector3.up,
                PathDirections.Down => Vector3.down,
                _ => currentPoint
            };
            if (segment.Length == PathLengths.Long)
            {
                currentPoint.z *= 2f;
            }

            if (segment.Width == PathWidths.Wide)
            {
                currentPoint.x *= 2f;
            }
            
            switch (segment.Action)
            {
                case PathActions.Descend:
                    currentPoint += new Vector3(0, -1f, 0);
                    break;
                case PathActions.Ascend:
                    currentPoint += new Vector3(0, 1f, 0);
                    break;
            }

            currentPoint += previousPoint;
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }
}

public class PathSegment
{
    public PathDirections Direction;
    public PathActions Action;
    public PathLengths Length;
    public PathWidths Width;

    public bool Indoors;
}

public enum PathDirections
{
    Straight,
    Left,
    Right,
    Up,
    Down
}

public enum PathActions
{
    None,
    Ascend,
    Descend,
    EnterBuilding,
    ExitBuilding
}

public enum PathLengths
{
    Short,
    Long
}

public enum PathWidths
{
    Thin,
    Wide

}
