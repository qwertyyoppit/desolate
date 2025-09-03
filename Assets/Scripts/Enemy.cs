using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private new CapsuleCollider collider;

    [SerializeField] private float walkingSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 5.0f;

    [SerializeField] private Transform[] routePoints;
    private int currentPoint;
    private float completeProximity = 0.5f;
    private bool backwards;

    [SerializeField] private DetectionState detection;

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        if (routePoints.Length < 2) return;
        var pointPos = routePoints[currentPoint].position;
        Physics.Raycast(new Ray(pointPos, Vector3.down), out RaycastHit groundHit, 10000f, LayerMask.GetMask("Terrain"));
        var goal = new Vector3(pointPos.x, groundHit.point.y + collider.height / 2, pointPos.z);

        var moved = transform.position + (goal - transform.position).normalized * (Time.deltaTime * walkingSpeed);

        if (Vector3.Distance(moved, goal) < completeProximity)
        {
            NextPoint();
        }

        transform.position = moved;
    }

    private void NextPoint()
    {
        currentPoint += backwards ? -1 : 1;
        if (currentPoint >= routePoints.Length) currentPoint = 0;
        else if (currentPoint < 0) currentPoint = routePoints.Length - 1;
    }
}

public enum DetectionState
{
    Unaware,
    Alerted,
    Aware
}