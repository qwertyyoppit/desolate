using UnityEngine;

public class SanityConstantOnCollision : MonoBehaviour
{
    [SerializeField] private float strength = 5f;
    [SerializeField] private bool interpolate;
    private SphereCollider sphere;

    private void Awake()
    {
        sphere = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var accounted = other.transform.position;
        accounted.y = transform.position.y;
        accounted += (transform.position - accounted).normalized * other.bounds.extents.x;
        var power = interpolate ? strength * Mathf.InverseLerp(sphere.radius, 0, Vector3.Distance(transform.TransformPoint(sphere.center), accounted)) : strength;
        print(power);

        Sanity.SanityEffect?.Invoke(new SanityEffectArgs(SanityEffectType.Instant, power * Time.deltaTime));
    }
}
