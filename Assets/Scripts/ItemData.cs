using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public string Name = "";
    public string Description = "";
    [SerializeField]
    private GameObject ItemPrefab;
}
