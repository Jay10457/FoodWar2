using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    
    [Tooltip("stack Limit")]
    public int stackLimit = 5;
    public Sprite itemSprite;
    [Tooltip("Game model of Item")]
    public GameObject itemPrefab;
    public Type type;
    [Tooltip("Color of the item slot border.")]
    public Color itemBorderColor = new Color(1, 1, 1, 1);
    [Tooltip("If this is an equipable item, this is what GameObject will spawn when held/equipped.")]
    public GameObject equipPrefab;
    public int dropAmount;
   


    public enum Type
    {
        dish,
        weapons
    }
}
