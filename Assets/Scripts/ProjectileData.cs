using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float Speed = 1f;
    public float MaxLifetime = 100f;
    public GameObject Prefab;
}
