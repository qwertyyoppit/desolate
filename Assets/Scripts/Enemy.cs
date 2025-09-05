using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [SerializeField] private DetectionState detection;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        agent.destination = player.position;
    }
}

public enum DetectionState
{
    Unaware,
    Alerted,
    Aware
}