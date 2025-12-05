using UnityEngine;

public class PlayerItemGripPosition : MonoBehaviour
{
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = Refs.PlayerInventory;
    }

    private void Update()
    {
        var currentItem = playerInventory.hotbarSlots[playerInventory.selectedItem].ItemHeld;

        currentItem.transform.position = transform.position + currentItem.GripPosition.position - currentItem.transform.position + Camera.main.transform.position - Refs.Player.transform.position;

        //need to separate visual position from physical (e.g. projectile origin) position.... ... . . .. . just like camera.position (lateupdate) vs. player.position (update)
    }
    private void LateUpdate()
    {
        var currentItem = playerInventory.hotbarSlots[playerInventory.selectedItem].ItemHeld;

        currentItem.transform.position = transform.position + currentItem.GripPosition.position - currentItem.transform.position + Camera.main.transform.position - Refs.Player.transform.position;
    }
}
