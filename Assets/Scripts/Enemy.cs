using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float walkingSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 5.0f;

    [SerializeField] private Transform[] routePoints;
    private int currentPoint;
    private bool backwards;

    [SerializeField] private DetectionState detection;

    private void Update()
    {
        if (routePoints.Length < 2) return;

        var moved = transform.position + (routePoints[currentPoint].position - transform.position).normalized * (Time.deltaTime * walkingSpeed);

        if (Vector3.Dot(routePoints[currentPoint].position - transform.position, (routePoints[currentPoint].position - moved)) < 0)
        {
            NextPoint();
        }

        transform.position = moved;
    }

    private void NextPoint()
    {
        if (backwards)
        {
            currentPoint -= 1;
        }
        else
        {
            currentPoint += 1;
        }
        Mathf.Clamp(currentPoint, 0, routePoints.Length - 1);
    }
}

public enum DetectionState
{
    Unaware,
    Alerted,
    Aware
}