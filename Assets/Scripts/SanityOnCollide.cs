using UnityEngine;

public class SanityOnCollide : MonoBehaviour
{
    [SerializeField] SanityEffectArgs effectArgs;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Sanity.SanityEffect?.Invoke(effectArgs);
    }
}
