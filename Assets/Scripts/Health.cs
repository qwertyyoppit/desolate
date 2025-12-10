using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public GameObject winText;
    public float winDuration;

    public float amount;
    public float max;

    private void Awake()
    {
        amount = max;
    }

    public void Damage(float damageAmount)
    {
        amount -= damageAmount;
        if (amount <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        if (CompareTag("Player"))
        {
            Destroy(gameObject);
            StartCoroutine(Winner());
            return;
        }
        else if (CompareTag("enemy"))
        {
            var rb = GetComponent<Rigidbody>();
            var enemy = GetComponent<Enemy>();
            var ai = GetComponent<NavMeshAgent>();
            enemy.enabled = false;
            ai.enabled = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;
            var force = Quaternion.Euler(0, Random.value * 360, 0) * Vector3.forward;
            rb.AddForce(force * Random.Range(5f, 11f), ForceMode.Impulse);
        }
    }

    IEnumerator Winner()
    {
        winText.SetActive(true);
        yield return new WaitForSeconds(winDuration);
        SceneManager.LoadScene("SampleScene");
    }
}
