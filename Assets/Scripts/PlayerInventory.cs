using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] int hotbarCapacity;
    [SerializeField] int inventoryCapacity;
    [SerializeField] private ItemSlot[] hotbarSlots;
    [SerializeField] private ItemSlot[] inventorySlots;

    public int selectedItem = 0;

    private void Awake()
    {
        hotbarSlots = new ItemSlot[hotbarCapacity];
        inventorySlots = new ItemSlot[inventoryCapacity];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ActivateItem();
        }
    }

    private void ActivateItem()
    {
        hotbarSlots[selectedItem].ItemHeld.Activate();
    }
}
