using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager 
{
    List<RecipeScriptableOBJ> recipes = new List<RecipeScriptableOBJ>();
    static RecipeManager _instance = null;
   public static RecipeManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RecipeManager();
                _instance.recipes = new List<RecipeScriptableOBJ>(Resources.LoadAll<RecipeScriptableOBJ>("Data"));
                //foreach (RecipeScriptableOBJ i in _instance.recipes)
                 //Debug.LogError(i.name);
            }
            return _instance;
        }
    }
}