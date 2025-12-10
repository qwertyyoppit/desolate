using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SectionGeneration : MonoBehaviour
{
    private List<PathSegment> pathSegments;

    public List<Section> sections = new();

    public List<(Enemy enemy, int index)> newborns = new();
    
    [SerializeField] private GameObject sectionPrefab;
    private static GameObject _sectionsObject;
    private static GameObject SectionsObject
    {
        get
        {
            if (!_sectionsObject)
            {
                _sectionsObject = new GameObject("sections");
            }
            return _sectionsObject;
        }
    }

    public SectionDatabase sectionSelection;

    public static SectionGeneration _instance;
    public static SectionGeneration Instance
    {
        get
        {
            if (!_instance)
                _instance = new GameObject("SectionGeneration").AddComponent<SectionGeneration>();
            return _instance;
        }
    }

    public GameObject mrEvil;
    [Range(0, 1)] public float mrEvilChance = 0.2f;

    public GameObject death;

    private void Awake()
    {
        _instance = this;
    }

    public void Generate()
    {
        foreach (Transform sectionObject in SectionsObject.transform) Destroy(sectionObject.gameObject);
        pathSegments = PathGeneration.PathSegments;
        PathSegment previousSegment = null;
        ExitNode previousExit = null;
        var originpoint = new GameObject("origin");
        foreach (var newborn in newborns) Destroy(newborn.enemy.gameObject);
        newborns = new();
        for (var i = 0; i < pathSegments.Count; i++)
        {
            var segment = pathSegments[i];
            Section currentSection;
            EntryNode currentEntry;
            if (previousSegment != null)
            {
                List<(Section section, EntryNode entry)> possibleSections = new();
                foreach (var section in (from section in sectionSelection.GetSections() where section.Indoors == segment.Indoors select section))
                {
                    var possibleEntries = 
                        (from entry in section.GetEntryNodesOfDirection(previousExit.direction) where CompareEntrySegmentElevation(segment, entry)
                        //get entries going the previous exit's direction
                        from exit in section.GetExitNodesOfDirection(segment.Direction == PathDirection.None ? PathDirection.Straight : segment.Direction) 
                        //get exits going the current direction
                        where !entry.disableOnNodeUsage.Contains(exit.gameObject)
                         //return entry if there is any valid exit that would not get disabled by it
                        where CompareExitSegmentElevation(segment, exit)
                        //where (from enabled in entry.enableOnNodeUsage where enabled.TryGetComponent<ExitNode>(out _) select enabled).ToArray().Length > 0
                        //&& entry.enableOnNodeUsage.Contains(exit.gameObject)
                        select entry).ToArray();
                    if (possibleEntries.Length <= 0) continue;
                    var determinedEntry = possibleEntries[Random.Range(0, possibleEntries.Length)];
                    possibleSections.Add(new ValueTuple<Section, EntryNode>(section, determinedEntry));
                    //list of sections and their predetermined entries, so we only use the entry that made the section valid
                }
                (Section section, EntryNode entry) chosen;
                try
                { 
                    chosen = possibleSections[Random.Range(0, possibleSections.Count)];
                }
                catch (System.Exception)
                {
                    GameManager.Instance.GenerateLevel();
                    return;
                }
                currentSection = Instantiate(chosen.section.gameObject, SectionsObject.transform).GetComponent<Section>();
                //We need a reference to the same entry in the section instance, as
                //using chosen.entry would edit the prefab file (which we do not want).
                //Instead, we get the index of the entry in the section prefab so we
                //know where to find it in the instance.
                var entryIndex = Array.IndexOf(chosen.section.ValidEntryNodes, chosen.entry);
                currentEntry = currentSection.ValidEntryNodes[entryIndex];

                // indoor exit > indoor entry > exit > entry; decides which shows their objects.
                // Regardless, they disable the objects they would have disabled before.
                // If neither disable each other, both show up; this is why disableEntry in exits is true by default.
                if (currentEntry.disablesExit)
                {
                    if (previousExit.disablesEntry)
                    {
                        if (segment.Indoors)
                        {
                            if (previousSegment.Indoors)
                            {
                                previousExit.Activate();
                                currentEntry.Activate(false);
                            }
                            else
                            {
                                previousExit.Activate(false);
                                currentEntry.Activate();
                            }
                        }
                        else
                        {
                            previousExit.Activate();
                            currentEntry.Activate(false);
                        }
                    }
                    else
                    {
                        previousExit.Activate();
                        currentEntry.Activate(false);
                    }
                }
                else
                {
                    previousExit.Activate();
                    currentEntry.Activate(!previousExit.disablesEntry);
                }
                currentSection.transform.position =
                currentSection.transform.position - currentEntry.transform.position + previousExit.transform.position;
            }
            else
            {
                currentSection = Instantiate(sectionPrefab, SectionsObject.transform).GetComponent<Section>();
            }

            if (i % 2 == 0) currentSection.transform.position += new Vector3(0.001f, 0.001f, 0.001f); //Solve any possible z-fighting
            else currentSection.transform.position -= new Vector3(0.001f, 0.001f, 0.001f);

            sections.Add(currentSection);
            
            var possibleExits = (from exit in currentSection.GetExitNodesOfDirection(segment.Direction == PathDirection.None ? PathDirection.Straight : segment.Direction)
                                 where CompareExitSegmentElevation(segment, exit) select exit).ToArray();
            try
            {
                previousExit = possibleExits[Random.Range(0, possibleExits.Length)];
            }
            catch (System.Exception)
            {
                GameManager.Instance.GenerateLevel();
                return;
            }
            previousSegment = segment;

            if (mrEvilChance > Random.value)
            {
                var newborn = Instantiate(mrEvil);
                newborn.transform.position = currentSection.transform.position;
                newborn.transform.position += new Vector3(0, 0.5f, 0);
                newborn.transform.localEulerAngles = previousExit.direction switch
                {
                    PathDirection.Left => new Vector3(0, -90, 0),
                    PathDirection.Right => new Vector3(0, 90, 0),
                    _ => new Vector3(0, 0, 0)
                };
                var enemy = newborn.GetComponent<Enemy>();
                newborns.Add(new (enemy, i));
            }
        }
        previousExit.Activate();

        var newDeath = Instantiate(death);
        newDeath.transform.position = new Vector3(previousExit.transform.position.x, previousExit.transform.position.y + 1.5f, previousExit.transform.position.z);
        
        var navGen = FindAnyObjectByType<NavMeshSurface>();
        navGen.BuildNavMesh();
        
        foreach (var newborn in newborns)
        {
            int startIndex = Mathf.Max(newborn.index - Random.Range(2, 5), 0);
            int endIndex = Mathf.Min(newborn.index + Random.Range(2, 5), pathSegments.Count - 1);
            newborn.enemy.routePoints = new Transform[] { sections[startIndex].transform, sections[endIndex].transform };
        }
    }

    private bool CompareEntrySegmentElevation(PathSegment segment, EntryNode entry)
    {
        return segment.ElevationAction switch
        {
            PathElevationActions.None => !entry.elevated,
            PathElevationActions.Descend => entry.elevated,
            _ => true,
        };
    }

    private bool CompareExitSegmentElevation(PathSegment segment, ExitNode exit)
    {
        return segment.ElevationAction switch
        {
            PathElevationActions.None => !exit.elevated,
            PathElevationActions.Ascend => exit.elevated,
            _ => true,
        };
    }
}
