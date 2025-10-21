using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float Speed;
    public GameObject Prefab;
}
