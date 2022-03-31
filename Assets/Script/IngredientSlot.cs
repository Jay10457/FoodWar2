using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : InventorySlot
{
    public Button addIngredientButtom;
    [SerializeField] int index;
    
    
    private void Awake()
    {
        includeInInventory = false;
        addIngredientButtom = this.GetComponent<Button>();
        

    }

   
}
