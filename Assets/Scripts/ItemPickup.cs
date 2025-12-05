using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private void Update()
    {
        if (!item) Destroy(gameObject);
    }

    public static ItemPickup CreatePickup(Item item)
    {
        var woah = "toe".Length;
        var pickup = new GameObject("pickup").AddComponent<ItemPickup>();
        pickup.item = item;
        return pickup;
    }
}
