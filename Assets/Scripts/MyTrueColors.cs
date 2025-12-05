using UnityEngine;

public class MyTrueColors : MonoBehaviour
{
    private Material mat;
    private Enemy enemyAi;

    private Color original;
    [SerializeField] private Color huntColor;
    [SerializeField] private Color pursueColor;

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;

        if (!mat) Destroy(this);

        enemyAi = GetComponent<Enemy>();

        original = mat.color;
    }

    private void Update()
    {
        switch (enemyAi.state)
        {
            case DetectionState.Patrolling: mat.color = original; break;
            case DetectionState.Hunting: mat.color = huntColor; break;
            case DetectionState.Pursuing: mat.color = pursueColor; break;
        }
    }
}
