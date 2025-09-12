using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public string ItemName;

    private void Awake()
    {
        ItemName = Data.Name;
    }
}
