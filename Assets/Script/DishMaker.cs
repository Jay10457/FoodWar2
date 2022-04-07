using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public partial class Cooker
{
    public List<RecipeScriptableOBJ> recipes;
    public RecipeScriptableOBJ currentRecipe;
    public List<Item> ingredients;
    Predicate<RecipeScriptableOBJ> CheckIngredient;
    int m;
    Item current;
    public bool CheckIngredientsMethod(RecipeScriptableOBJ recipe)
    {



        if (recipe.ingredients.Count == 2)
        {
            if (recipe.ingredients.Find(t => recipe.ingredients.Contains(ingredients[0]) && recipe.ingredients.Contains(ingredients[1])))
            {
                return true;
            }
            
        }
        else if (recipe.ingredients.Count == 3)
        {
            if (recipe.ingredients.Find(t => recipe.ingredients.Contains(ingredients[0]) && recipe.ingredients.Contains(ingredients[1]) && recipe.ingredients.Contains(ingredients[2])))
            {
                return true;
            }

        }

        return false;

    }

    public RecipeScriptableOBJ CheckThreeMatRecipe()
    {
        foreach (RecipeScriptableOBJ recipe in RecipeManager.instance.threeMatRecipes)
        {
            if (recipe.ingredients.Find(r => recipe.ingredients.Contains(ingredients[0]) && recipe.ingredients.Contains(ingredients[1])
            && recipe.ingredients.Contains(ingredients[2])))
            {
                return recipe;
            }
            else if (cookerTeam == FoodTeam.GOOD)
            {
                return RecipeManager.instance.GetRecipeByName("MixShit_R");
            }
            else if (cookerTeam == FoodTeam.BAD)
            {
                return RecipeManager.instance.GetRecipeByName("Fat_R");
            }
        }
        return null;
    }
    public RecipeScriptableOBJ CheckTwoMatRecipe()
    {



        foreach (RecipeScriptableOBJ recipe in RecipeManager.instance.twoMatRecipes)
        {

            if (recipe.ingredients.Find(r => recipe.ingredients.Contains(ingredients[0]) && recipe.ingredients.Contains(ingredients[1])))
            {
                return recipe;
            }

        }


       
        return null;






    }

}
