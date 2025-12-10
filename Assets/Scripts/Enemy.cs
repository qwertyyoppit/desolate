using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private new CapsuleCollider collider;

    private Transform player;

    public Transform[] routePoints;
    [SerializeField] private float completeProximity = 0.5f;
    [SerializeField] private bool backwards;
    [SerializeField] private bool pingPong;
    [Space]
    [SerializeField] private Transform visionPoint;
    public float viewDistance = 10f;
    [Range(0f, 360f)] public float fov = 90f;
    public float noticeTime = 1f;
    public float huntTime = 10f;

    public float PatrolSpeed = 3.5f;
    public float PursueSpeed = 5f;

    public float noticeTimer;
    public float huntTimer;

    private int currentPoint;

    private bool seesPlayer;
    private Vector3 playerLastSeen;

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
        CheckVision();

        switch (state)
        {
            case DetectionState.Patrolling: Patrol(); break;
            case DetectionState.Hunting: Hunt(); break;
            case DetectionState.Pursuing: Pursue(); break;
        };

        agent.destination = goal;
    }

    private void CheckVision()
    {
        seesPlayer = false;

        if (Vector3.Angle(player.position - transform.position, visionPoint.forward) > fov) return;
        if (Vector3.Distance(player.position, visionPoint.position) > viewDistance) return;

        var playerRay = new Ray(visionPoint.position, player.position - visionPoint.position);
        if (Physics.Raycast(playerRay, out var info, viewDistance))
        {
            if (info.collider.gameObject == player.gameObject)
            {
                seesPlayer = true;
                playerLastSeen = player.position;
            }
        }
    }

    private void Patrol()
    {
        if (seesPlayer)
        {
            noticeTimer += Time.deltaTime;
            if (noticeTimer > noticeTime)
            {
                SetPursuing();
                return;
            }
        }
        else
        {
            noticeTimer = Mathf.Max(noticeTimer - Time.deltaTime, 0);
        }
        var pointPos = routePoints[currentPoint].position;
        goal = GroundPoint(pointPos);

        if (routePoints.Length > 1 && Vector3.Distance(transform.position, goal) < completeProximity)
        {
            NextPoint();
        }
    }

    private void Hunt()
    {
        // set when seen player but not pursuing, would go to where it saw player.
        // if no see player when get to lastseen, we go to patrolling
        // poopen farten shitten i stayed up so late :(
    }

    private void Pursue()
    {
        huntTimer -= Time.deltaTime;
        if (huntTimer  <= 0)
        {
            SetPatrolling();
            return;
        }
        if (seesPlayer)
        {
            huntTimer = huntTime;
            goal = player.position;
            return;
        }
        goal = playerLastSeen;
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
        agent.speed = PatrolSpeed;
    }

    public void SetHunting()
    {
        state = DetectionState.Hunting;
        noticeTimer = 0.0f;
    }

    public void SetPursuing()
    {
        state = DetectionState.Pursuing;
        agent.speed = PursueSpeed;
        huntTimer = huntTime;
    }

    public Vector3 GroundPoint(Vector3 point)
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