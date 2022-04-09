using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager
{
    public List<RecipeScriptableOBJ> recipes = new List<RecipeScriptableOBJ>();
    public List<RecipeScriptableOBJ> twoMatRecipes = new List<RecipeScriptableOBJ>();
    public List<RecipeScriptableOBJ> threeMatRecipes = new List<RecipeScriptableOBJ>();
    static RecipeManager _instance = null;
    public static RecipeManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RecipeManager();
                _instance.recipes = new List<RecipeScriptableOBJ>(Resources.LoadAll<RecipeScriptableOBJ>("Data"));
                foreach (RecipeScriptableOBJ recipe in _instance.recipes)
                {
                    if (recipe.ingredients.Count == 3)
                    {
                        _instance.threeMatRecipes.Add(recipe);
                        //Debug.LogError(recipe.name);
                    }
                    else if (recipe.ingredients.Count == 2)
                    {
                        _instance.twoMatRecipes.Add(recipe);
                        //Debug.LogError(recipe.name);
                    }
                }
                //foreach (RecipeScriptableOBJ i in _instance.recipes)
                //Debug.LogError(i.name);
            }
            return _instance;
        }
    }


    public RecipeScriptableOBJ GetRecipeByName(string recipeName)
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            if (recipes[i].name == recipeName)
                return recipes[i];
        }
        return null;
    }
}
