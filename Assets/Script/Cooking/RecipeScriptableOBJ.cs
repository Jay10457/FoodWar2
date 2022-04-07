using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe")]

public class RecipeScriptableOBJ : ScriptableObject
{
    public List<Item> ingredients = new List<Item>();
    public Item resultDish;
    public Sprite resultDishImage;
    public float cookinTime = 15;
    public int resultDishAmount = 1;

}
  
