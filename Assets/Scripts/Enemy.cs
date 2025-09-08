using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private new CapsuleCollider collider;

    private Transform player;

    [SerializeField] private Transform[] routePoints;
    [SerializeField] private float completeProximity = 0.5f;
    [SerializeField] private bool backwards;
    [SerializeField] private bool pingPong;

    private int currentPoint;

    [SerializeField] private DetectionState detection;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (routePoints.Length < 2) return;
        var pointPos = routePoints[currentPoint].position;
        pointPos.y += 0.1f;
        Physics.Raycast(new Ray(pointPos, Vector3.down), out RaycastHit groundHit, 10000f, LayerMask.GetMask("Terrain"));
        var goal = new Vector3(pointPos.x, groundHit.point.y + collider.height / 2, pointPos.z);

        if (Vector3.Distance(transform.position, goal) < completeProximity)
        {
            NextPoint();
        }

        agent.destination = goal;
    }

    private void NextPoint()
    {
        currentPoint += backwards ? -1 : 1;
        if (currentPoint >= routePoints.Length)
        {
            if (pingPong) 
            {
                backwards = true;
                currentPoint -= 2;
            }
            else currentPoint = 0;
        }
        else if (currentPoint < 0) 
        {
            if (pingPong)
            {
                backwards = false;
                currentPoint += 2;
            }
            else currentPoint = routePoints.Length - 1; 
        }
    }
}

public enum DetectionState
{
    Unaware,
    Alerted,
    Aware
}