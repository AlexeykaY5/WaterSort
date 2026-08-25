using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{

    public DifficultySettings settings;

    void Start()
    {
        Tube[] tubes = LevelGenerator.Generate(settings);



        foreach (Tube tube in tubes) 
        {
            Debug.Log("Tube contents: " + string.Join(", ", tube.GetContents()));
        }
        foreach (Tube tube in tubes)
        {
            Debug.Log(tube.PourInto(tubes[0]));
        }
        foreach (Tube tube in tubes)
        {
            Debug.Log("Tube contents: " + string.Join(", ", tube.GetContents()));
        }
    }

    void Update()
    {
        
    }
}