using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe")]
public class RecipeScriptableOBJ : ScriptableObject
{
    public Ingredient[] ingredients;
    public Item resultDish;
    public float cookinTime = 15;
    public int resultDishAmount = 1;
  
    [System.Serializable]
    public struct Ingredient
    {
        public Item material;
        public int amount;
    }
}
