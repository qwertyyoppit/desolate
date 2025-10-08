using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemUsetime : MonoBehaviour
{
    private Item item;

    private void Start()
    {
        item = GetComponent<Item>();

        if (!item)
        {
            Debug.LogError($"Object {gameObject.name} is not an Item!");
            Destroy(this);
        }
    }
}
