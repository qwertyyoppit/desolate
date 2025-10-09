using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] int hotbarCapacity;
    [SerializeField] int inventoryCapacity;
    [SerializeField] private ItemSlot[] hotbarSlots;
    [SerializeField] private ItemSlot[] inventorySlots;

    public ItemData ItemData;

    private GameObject _playerInventoryObj;
    private GameObject PlayerInventoryObj
    {
        get
        {
            if (!_playerInventoryObj)
            {
                _playerInventoryObj = new GameObject("Player Inventory");
                _playerInventoryObj.transform.SetParent(Refs.Player.transform);
            }
            return _playerInventoryObj;
        }
    }
    private GameObject _hotbarObj;
    private GameObject HotbarObj
    {
        get
        {
            if (!_hotbarObj)
            {
                _hotbarObj = new GameObject("Hotbar");
                _hotbarObj.transform.SetParent(PlayerInventoryObj.transform);
            }
            return _hotbarObj;
        }
    }
    private GameObject _inventoryObj;
    private GameObject InventoryObj
    {
        get
        {
            if (!_inventoryObj)
            {
                _inventoryObj = new GameObject("Inventory");
                _inventoryObj.transform.SetParent(PlayerInventoryObj.transform);
            }
            return _inventoryObj;
        }
    }

    public int selectedItem = 0;

    private void Awake()
    {
        hotbarSlots = new ItemSlot[hotbarCapacity];
        inventorySlots = new ItemSlot[inventoryCapacity];
        for (int i = 0; i < hotbarCapacity; i++)
        {
            var slotObj = new GameObject("Hotbar Slot");
            slotObj.transform.SetParent(HotbarObj.transform);
            hotbarSlots[i] = slotObj.AddComponent<ItemSlot>();
        }
        for (int i = 0; i < inventoryCapacity; i++)
        {
            var slotObj = new GameObject("Inventory Slot");
            slotObj.transform.SetParent(InventoryObj.transform);
            inventorySlots[i] = slotObj.AddComponent<ItemSlot>();
        }

        Refs.PlayerInput.actions["Scroll"].performed += ScrollInput;
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
        hotbarSlots[selectedItem].ItemHeld.Use();
    }

    private void ScrollInput(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() < 0)
            ScrollUpSelectedItem();
        else
            ScrollDownSelectedItem();
    }

    public void ScrollUpSelectedItem()
    {
        selectedItem = (selectedItem + hotbarCapacity + 1) % hotbarCapacity;
    }
    public void ScrollDownSelectedItem()
    {
        selectedItem = (selectedItem + hotbarCapacity - 1) % hotbarCapacity;
    }
}
