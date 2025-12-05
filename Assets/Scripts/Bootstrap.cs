using Unity.AI.Navigation;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        var pathGen = FindAnyObjectByType<PathGeneration>();
        var sectionGen = FindAnyObjectByType<SectionGeneration>();
        var navGen = FindAnyObjectByType<NavMeshSurface>();

        pathGen.GeneratePath();
        sectionGen.Generate();
        navGen.BuildNavMesh();
    }
}
