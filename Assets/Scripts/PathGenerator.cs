using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private int segmentAmount = 15;

    private List<PathSegment> pathSegments = new();

    public bool drawPathGizmo;

    public void GeneratePath()
    {
        pathSegments.Clear();

        for (var i = 0; i < segmentAmount; i++)
        {
            var currentSegment = new PathSegment();
            pathSegments.Add(currentSegment);
            var validDirections = new List<PathDirections>() { PathDirections.Straight, PathDirections.Left, PathDirections.Right, PathDirections.Up, PathDirections.Down };

            var validActions = new List<PathActions>() { PathActions.Ascend, PathActions.Descend, PathActions.EnterBuilding, PathActions.ExitBuilding };

            var validLengths = new List<PathLengths> { PathLengths.Short, PathLengths.Long };
            var validWidths = new List<PathWidths> { PathWidths.Thin, PathWidths.Wide };

            var previousSegment = pathSegments[i - 1];
            if (previousSegment != null)
            {
                validDirections.Remove(previousSegment.direction);
                validActions.Remove(previousSegment.action);

                if (previousSegment.direction == PathDirections.Left || previousSegment.direction == PathDirections.Right)
                {
                    validDirections.Remove(PathDirections.Left);
                    validDirections.Remove(PathDirections.Right);
                }
                if (previousSegment.action == PathActions.Ascend || previousSegment.action == PathActions.Descend)
                {
                    validActions.Remove(PathActions.Ascend);
                    validActions.Remove(PathActions.Descend);
                }
                if (previousSegment.is_indoors)
                {
                    validActions.Remove(PathActions.EnterBuilding);
                    currentSegment.is_indoors = true;
                }
                else
                {
                    validActions.Remove(PathActions.ExitBuilding);
                }
                if (Random.value > 0.5f)
                {
                    validLengths.Remove(previousSegment.length);
                }
                else
                {
                    validWidths.Remove(previousSegment.width);
                }
            }
            currentSegment.direction = validDirections[Random.Range(0, validDirections.Count)];
            currentSegment.action = Random.value > 0.8f ? PathActions.None : validActions[Random.Range(0, validActions.Count)];
            if (currentSegment.action == PathActions.EnterBuilding)
            {
                currentSegment.is_indoors = true;
            }
            else if (currentSegment.action == PathActions.ExitBuilding)
            {
                currentSegment.is_indoors = false;
            }
        }
    }

    public void PrintPath()
    {
        foreach (var segment in pathSegments)
        {
            Debug.Log($"{segment.direction}, {segment.action}, {segment.length}, {segment.width}, {segment.is_indoors}");
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
            if (segment.is_indoors)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            previousPoint = currentPoint;
        }
    }
}

public class PathSegment
{
    public PathDirections direction;
    public PathActions action;
    public PathLengths length;
    public PathWidths width;

    public bool is_indoors;
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