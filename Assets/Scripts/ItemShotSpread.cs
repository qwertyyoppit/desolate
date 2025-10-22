using UnityEngine;

[RequireComponent(typeof(ItemProjectileManager))]
public class ItemShotSpread : MonoBehaviour
{
    [SerializeField] private float spreadX;
    [SerializeField] private float spreadY;

    private ItemProjectileManager itemProjectileManager;

    private void Awake()
    {
        itemProjectileManager = GetComponent<ItemProjectileManager>();

    }

    private void OnEnable()
    {
        itemProjectileManager.projectileFired += OnProjectileFired;
    }
    private void OnDisable()
    {
        itemProjectileManager.projectileFired -= OnProjectileFired;
    }

    private void OnProjectileFired(Projectile shot)
    {
        var spreadAmount = new Vector3(spreadY * Random.value, spreadX * Random.value, 0);
        shot.transform.localEulerAngles += spreadAmount;
    }
}
