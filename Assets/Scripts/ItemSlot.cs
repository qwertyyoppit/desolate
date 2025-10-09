using UnityEngine;

[System.Serializable]
public class ItemSlot : MonoBehaviour
{
    public Item ItemHeld;
    public static ItemData bata;

    private void Awake()
    {
        ItemHeld = Item.CreateItem(Refs.PlayerInventory.ItemData, transform);
    }
}
