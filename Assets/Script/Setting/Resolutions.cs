using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolutions : MonoBehaviour
{
    public static Resolutions instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public List<Resolution> GetResolutions()
    {
        
       
        //Filters out all resolutions with low refresh rate:
        Resolution[] resolutions = Screen.resolutions;
        HashSet<Tuple<int, int>> uniqResolutions = new HashSet<Tuple<int, int>>();
        Dictionary<Tuple<int, int>, int> maxRefreshRates = new Dictionary<Tuple<int, int>, int>();
        for (int i = 0; i < resolutions.GetLength(0); i++)
        {
            //Add resolutions (if they are not already contained)
            Tuple<int, int> resolution = new Tuple<int, int>(resolutions[i].width, resolutions[i].height);
            uniqResolutions.Add(resolution);
            //Get highest framerate:
            if (!maxRefreshRates.ContainsKey(resolution))
            {
                maxRefreshRates.Add(resolution, resolutions[i].refreshRate);
            }
            else
            {
                maxRefreshRates[resolution] = resolutions[i].refreshRate;
            }
        }
        //Build resolution list:
        List<Resolution> uniqResolutionsList = new List<Resolution>(uniqResolutions.Count);
        foreach (Tuple<int, int> resolution in uniqResolutions)
        {
            Resolution newResolution = new Resolution();
            newResolution.width = resolution.Item1;
            newResolution.height = resolution.Item2;
            if (maxRefreshRates.TryGetValue(resolution, out int refreshRate))
            {
                newResolution.refreshRate = refreshRate;
            }
            uniqResolutionsList.Add(newResolution);
        }
        return uniqResolutionsList;


    }
}
