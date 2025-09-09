using System.Collections.Generic;
using UnityEngine;

public class PathGeneration : MonoBehaviour
{
    [SerializeField] private int segmentAmount = 15;
    [SerializeField] private int maxConsecutiveOutdoors = 8;
    [SerializeField] private int maxConsecutiveIndoors = 6;

    [SerializeField, Range(0f, 1f)] private float chanceOfAction = 0.2f;

    [SerializeField, Range(0f, 1f)] private float chanceOfNoneDirectionOnElevationChange = 0.5f;

    public static readonly List<PathSegment> PathSegments = new();

    public bool drawPathGizmo;

    public static PathGeneration _instance;
    public static PathGeneration Instance
    {
        get
        {
            if (!_instance)
                _instance = new GameObject("PathGeneration").AddComponent<PathGeneration>();
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void GeneratePath()
    {
        PathSegments.Clear();

        var cycle = Random.Range(1, maxConsecutiveOutdoors);
        for (var i = 0; i < segmentAmount; i++)
        {
            var currentSegment = new PathSegment();
            PathSegments.Add(currentSegment);
            var validDirections = new List<PathDirection>
                { PathDirection.Straight, PathDirection.Left, PathDirection.Right };

            var validElevationActions = new List<PathElevationActions> { PathElevationActions.Ascend, PathElevationActions.Descend };

            var validLengths = new List<PathLengths> { PathLengths.Short, PathLengths.Long };
            var validWidths = new List<PathWidths> { PathWidths.Thin, PathWidths.Wide };

            var previousSegment = i > 0 ? PathSegments[i - 1] : null;
            if (previousSegment != null)
            {
                switch (previousSegment.Direction)
                {
                    case PathDirection.Left or PathDirection.Right:
                        validDirections.Remove(PathDirection.Left);
                        validDirections.Remove(PathDirection.Right);
                        break;
                }

                if (previousSegment.ElevationAction is PathElevationActions.Ascend or PathElevationActions.Descend)
                {
                    validElevationActions.Remove(PathElevationActions.Ascend);
                    validElevationActions.Remove(PathElevationActions.Descend);
                }
                if (previousSegment.Indoors)
                {
                    currentSegment.Indoors = true;
                }
                if (Random.value > 0.5f)
                {
                    validLengths.Remove(previousSegment.Length);
                }
                else
                {
                    validWidths.Remove(previousSegment.Width);
                }
                if (cycle <= 0)
                {
                    currentSegment.BuildingAction = previousSegment.Indoors ? PathBuildingActions.ExitBuilding : PathBuildingActions.EnterBuilding;
                    cycle = Random.Range(1, previousSegment.Indoors ? maxConsecutiveOutdoors : maxConsecutiveIndoors);
                }
            }
            currentSegment.ElevationAction = Random.value > chanceOfAction || validElevationActions.Count <= 0 ? PathElevationActions.None : validElevationActions[Random.Range(0, validElevationActions.Count)];
            currentSegment.Direction = Random.value > chanceOfNoneDirectionOnElevationChange && currentSegment.ElevationAction != PathElevationActions.None 
                ? PathDirection.None : validDirections[Random.Range(0, validDirections.Count)];
            currentSegment.Length = validLengths[Random.Range(0, validLengths.Count)];
            currentSegment.Width = validWidths[Random.Range(0, validWidths.Count)];
            currentSegment.Indoors = currentSegment.BuildingAction switch
            {
                PathBuildingActions.EnterBuilding => true,
                PathBuildingActions.ExitBuilding => false,
                _ => currentSegment.Indoors
            };
            cycle--;
        }
    }

    public void PrintPath()
    {
        foreach (var segment in PathSegments)
        {
            Debug.Log($"{segment.Direction}, {segment.ElevationAction}, {segment.BuildingAction}, {segment.Length}, {segment.Width}, {segment.Indoors}");
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
        foreach (var segment in PathSegments)
        {
            Gizmos.color = segment.Indoors ? Color.blue : Color.red;

            currentPoint = segment.Direction switch
            {
                PathDirection.None => Vector3.zero,
                PathDirection.Straight => Vector3.forward,
                PathDirection.Left => Vector3.left,
                PathDirection.Right => Vector3.right,
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
            
            switch (segment.ElevationAction)
            {
                case PathElevationActions.Descend:
                    currentPoint += new Vector3(0, -1f, 0);
                    break;
                case PathElevationActions.Ascend:
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
    public PathDirection Direction;
    public PathElevationActions ElevationAction;
    public PathBuildingActions BuildingAction;
    public PathLengths Length;
    public PathWidths Width;

    public bool Indoors;
}

public enum PathDirection
{
    None,
    Straight,
    Left,
    Right
}

public enum PathElevationActions
{
    None,
    Ascend,
    Descend
}

public enum PathBuildingActions
{
    None,
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
