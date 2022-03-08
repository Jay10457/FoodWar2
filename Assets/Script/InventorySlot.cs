using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public Item currentItem { get; protected set; }
    public int currentItemAmount { get; protected set; }
    [Tooltip("Whether or not to include in the inventory")]
    public bool includeInInventory = true;
    [Tooltip("What type of items this slot can hold")]
    public Item.Type type;
    public bool interactable = true;

    [SerializeField] protected Image itemImage;
    [SerializeField] protected Image borderImage;
    [SerializeField] protected Image stackImage;
    [SerializeField] protected Text stackText;


    private void Update()
    {
        if (currentItemAmount <= 0)
        {
            currentItem = null;
        }
        SetUI();
    }

    protected virtual void SetUI()
    {
        if (currentItem)
        {
            itemImage.sprite = currentItem.itemSprite;
            itemImage.color = Color.white;
            stackImage.color = currentItem.itemBorderColor;


            stackImage.gameObject.SetActive(currentItemAmount > 1);
            stackText.text = currentItemAmount.ToString();
        }
        else
        {
            itemImage.sprite = null;
            stackImage.color = Color.white;
            itemImage.color = Color.clear;
            stackImage.gameObject.SetActive(false);
        }


    }

    public int AddItemToSlot(Item item, int amount)
    {
        if (!currentItem && !item) return 0;
        if (type != item.type) return amount;
       

        if (currentItem == item)
        {
            currentItemAmount += amount;
        }
        else
        {
            currentItem = item;
            currentItemAmount = amount;
        }

        int overFlow = currentItemAmount - currentItem.stackLimit;
        if (overFlow > 0)
        {
            currentItemAmount = currentItem.stackLimit;
        }
        if (currentItemAmount == 0)
        {
            currentItem = null;
        }
        return overFlow;
    }
    public void SetItemInSlot(Item item, int amount)
    {
        currentItem = item;
        currentItemAmount = amount;
    }
    public bool CheckItemComatible(Item item)
    {
        return (item == null || type == item.type);
    }
}   
