using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemReverseProjectiles : MonoBehaviour
{
    private Item item;
    private PlayerProjectileManager projectileManager;

    [SerializeField] private float reverseSpeed = 1;

    private bool active;

    public ItemActivationTypes activationType;

    private void Awake()
    {
        item = GetComponent<Item>();
        projectileManager = Refs.PlayerProjectileManager;
    }

    private void Update()
    {
        foreach (var proj in projectileManager.projectiles)
        {
            proj.Speed = Mathf.Lerp(proj.Speed, proj.Data.Speed * (active ? -1 : 1), Time.deltaTime * reverseSpeed);
            print(proj.Speed);
        }
    }

    private void OnEnable()
    {
        item.usageStarted += OnUse;
    }

    private void OnUse(ItemUsageStartedArgs args)
    {
        print(args.activationType);
        if (args.activationType != activationType) return;

        active = !active;
    }
}
