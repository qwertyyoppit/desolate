using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathYay : MonoBehaviour
{
    [SerializeField] private GameObject deathimage;
    [SerializeField] private float DeathTime = 3.0f;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.gameObject.CompareTag("Player")) return;
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        deathimage.SetActive(true);
        yield return new WaitForSeconds(DeathTime);
        SceneManager.LoadScene("SampleScene");
    }
}
