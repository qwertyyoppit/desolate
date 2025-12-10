using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        var pathGen = FindAnyObjectByType<PathGeneration>();
        var sectionGen = FindAnyObjectByType<SectionGeneration>();

        pathGen.GeneratePath();
        sectionGen.Generate();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
