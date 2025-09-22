using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] int slotCapacity;
    [SerializeField] private ItemSlot[] itemSlots;

    public int selectedItem = 0;

    private void Awake()
    {
        itemSlots = new ItemSlot[slotCapacity];
    }
}
