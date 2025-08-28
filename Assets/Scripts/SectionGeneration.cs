using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SectionGeneration : MonoBehaviour
{
    private List<PathSegment> pathSegments;

    private List<Section> sections = new();
    
    [SerializeField] private GameObject sectionPrefab;

    public SectionDatabase sectionSelection;

    private void Start()
    {
        pathSegments = PathGenerator.PathSegments;
    }

    public void Generate()
    {
        PathSegment previousSegment = null;
        ExitNode previousExit = null;
        foreach (var segment in pathSegments)
        {
            Section currentSection;
            EntryNode currentEntry;
            if (previousSegment != null)
            {
                List<(Section section, EntryNode entry)> possibleSections = new();
                foreach (var section in sectionSelection.GetSections())
                {
                    var possibleEntries = 
                        (from entry in (from entry in section.GetEntryNodesOfDirection(previousExit.direction) where CompareEntrySegmentElevation(segment, entry) select entry)
                        //get entries going the previous exit's direction
                        from exit in section.GetExitNodesOfDirection(segment.Direction == PathDirection.None ? PathDirection.Straight : segment.Direction) 
                        //get exits going the current direction
                        where !entry.disableOnNodeUsage.Contains(exit.gameObject)
                         //return entry if there is any valid exit that would not get disabled by it
                        //where (from enabled in entry.enableOnNodeUsage where enabled.TryGetComponent<ExitNode>(out _) select enabled).ToArray().Length > 0
                        //&& entry.enableOnNodeUsage.Contains(exit.gameObject)
                        select entry).ToArray();
                    var determinedEntry = possibleEntries[Random.Range(0, possibleEntries.Length)];
                    possibleSections.Add(new ValueTuple<Section, EntryNode>(section, determinedEntry));
                    //list of sections and their predetermined entries, so we only use the entry that made the section valid
                }
                var chosen = possibleSections[Random.Range(0, possibleSections.Count)];
                currentSection = Instantiate(chosen.section.gameObject).GetComponent<Section>();
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
                currentSection = Instantiate(sectionPrefab).GetComponent<Section>();
            }

            sections.Add(currentSection);
            
            var possibleExits = (from exit in currentSection.GetExitNodesOfDirection(segment.Direction == PathDirection.None ? PathDirection.Straight : segment.Direction)
                                 where CompareExitSegmentElevation(segment, exit) select exit).ToArray();
            previousExit = possibleExits[Random.Range(0, possibleExits.Length)];
            previousExit.Activate();
            
            previousSegment = segment;
        }
    }

    private bool CompareEntrySegmentElevation(PathSegment segment, EntryNode entry)
    {
        return segment.ElevationAction switch
        {
            PathElevationActions.None  => !entry.elevated,
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
