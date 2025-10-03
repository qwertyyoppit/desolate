using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    public Item ItemHeld;

    public ItemSlot()
    {
        ItemHeld = new Item();
    }
}
