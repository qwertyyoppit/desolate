using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanityVisualEffect : MonoBehaviour
{
    private PlayerSanity sanity;

    [SerializeField] private Volume postProcessingVolume;

    [SerializeField] private VolumeProfile postProfile;

    private Vignette _vignette;

    private void Start()
    {
        sanity = FindAnyObjectByType<PlayerSanity>();
        if (postProcessingVolume)
        {
            postProcessingVolume.profile = postProfile;
            if (postProfile.TryGet<Vignette>(out var vignette))
            {
                _vignette = vignette;
            }
        }
    }

    private void LateUpdate()
    {
        if (_vignette) _vignette.intensity.value = Mathf.Lerp(0.8f, 0.2f, sanity.sanity / 100f);
    }


}
