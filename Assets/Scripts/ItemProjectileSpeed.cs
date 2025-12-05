using UnityEngine;

public class ItemProjectileSpeed : MonoBehaviour
{
    private ItemProjectileManager itemProjectiles;

    public float speedValue;
    public StatOperationType statOperation;

    private void Start()
    {
        itemProjectiles = GetComponent<ItemProjectileManager>();

        itemProjectiles.projectileFired += ApplySpeed;
    }

    private void ApplySpeed(Projectile projectile)
    {
        projectile.Speed = StatOperationEvaluation.Evaluate(projectile.Speed, speedValue, statOperation);
    }
}
