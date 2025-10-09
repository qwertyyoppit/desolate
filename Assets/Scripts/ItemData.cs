using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public string Name = "";
    public string Description = "";
    public float UseTime = 1.0f;
    public GameObject ItemPrefab;
}
