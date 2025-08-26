using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SectionDatabase", menuName = "Scriptable Objects/SectionDatabase")]
public class SectionDatabase : ScriptableObject
{
    private static SectionDatabase _instance;
    public static SectionDatabase Instance
    {
        get
        {
            if (!_instance)
                _instance = CreateInstance<SectionDatabase>();
            return _instance;
        }
    }

    public GameObject[] sections;
    
    public Section[] GetSections()
    {
        var temp = new List<Section>();

        foreach (var obj in sections)
        {
            var section = obj.GetComponent<Section>();
            if (section)
                temp.Add(section);
            else
                Debug.LogError($"Object {obj} has no Section! Fix it or remove it from the database.");
        }
        return temp.ToArray();
    }
}
