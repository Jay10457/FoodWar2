using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : InventorySlot
{
    [SerializeField] Button addIngredientButtom;
    private event Action addIngredient;
    [SerializeField] Item ingredient;
    private void Awake()
    {
        includeInInventory = false;
        addIngredientButtom = this.GetComponent<Button>();
        addIngredientButtom.onClick.AddListener(delegate () { AddIngredient(); });

    }

    private void AddIngredient()
    {
        InventoryManager.instance.AddItemToInventoryInCurrentSlot(ingredient, 1, this);
    }
}
