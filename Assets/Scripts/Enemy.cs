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
    [Space]
    [SerializeField] private Transform visionPoint;
    public float viewDistance = 10f;
    [Range(0f, 360f)] public float fov = 90f;
    public float noticeTime = 1f;

    public float noticeTimer;

    private int currentPoint;

    private bool seesPlayer;

    public DetectionState state;

    private Vector3 goal;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<CapsuleCollider>();

        if (!visionPoint) visionPoint = transform;
    }

    private void Start()
    {
        player = Refs.Player.transform;
    }

    private void Update()
    {
        switch (state)
        {
            case DetectionState.Patrolling: Patrol(); break;
            case DetectionState.Hunting: Hunt(); break;
            case DetectionState.Pursuing: Pursue(); break;
        };

        CheckVision();

        if (seesPlayer)
        {
            noticeTimer = Mathf.Min(noticeTimer + Time.deltaTime, noticeTime);
            if (noticeTimer >= noticeTime)
            {
                SetPursuing();
            }
        }
        else
        {
            noticeTimer = Mathf.Max(0.0f, noticeTimer - Time.deltaTime);
            if (noticeTimer == 0.0f)
            {
                SetPatrolling();
            }
        }

        agent.destination = goal;
    }

    private void CheckVision()
    {
        seesPlayer = false;

        if (Vector3.Angle(player.position.normalized, visionPoint.forward) > fov) return;
        if (Vector3.Distance(player.position, visionPoint.position) > viewDistance) return;

        var playerRay = new Ray(player.position, player.position - visionPoint.position);
        if (!Physics.Raycast(playerRay, LayerMask.GetMask("Terrain")))
        {
            seesPlayer = true;
        }
    }

    private void Patrol()
    {
        var pointPos = routePoints[currentPoint].position;
        goal = GroundPoint(pointPos);

        if (routePoints.Length > 1 && Vector3.Distance(transform.position, goal) < completeProximity)
        {
            NextPoint();
        }
    }

    private void Hunt()
    {

    }

    private void Pursue()
    {
        goal = player.position;
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

    public void SetPatrolling()
    {
        state = DetectionState.Patrolling;
        noticeTimer = 0.0f;
        seesPlayer = false;
    }

    public void SetHunting()
    {
        state = DetectionState.Hunting;
        noticeTimer = 0.0f;
    }

    public void SetPursuing()
    {
        state = DetectionState.Pursuing;
    }

    private Vector3 GroundPoint(Vector3 point)
    {
        point.y += 0.1f;
        Physics.Raycast(new Ray(point, Vector3.down), out RaycastHit groundHit, 10000f, LayerMask.GetMask("Terrain"));
        return new Vector3(point.x, groundHit.point.y + collider.height / 2, point.z);
    }
}

public enum DetectionState
{
    Patrolling,
    Hunting,
    Pursuing
}