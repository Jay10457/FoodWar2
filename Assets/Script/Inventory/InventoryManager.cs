using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeField] Transform playerPos;
    

    public static List<InventorySlot> slots;
    public List<Item> items;

    public Item currentItem { get; private set; }
    public int currentItemAmout { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        slots = new List<InventorySlot>();
        items = new List<Item>();

        foreach (InventorySlot slot in GetComponentsInChildren<InventorySlot>())
        {
            if (slot.includeInInventory)
            {
                slots.Add(slot);// init slot
                //Debug.LogError(slots.Count);
            }
        }
    }
    public int AddItemToInventoryInCurrentSlot(Item item, int amount, IngredientSlot currentSlot)
    {
        int remaining = amount;
        if (currentSlot.currentItem == item)
        {
            int overflow = currentSlot.AddItemToSlot(item, remaining);
            if (overflow > 0)
            {
                remaining = overflow;
            }
            else
            {
                remaining = 0;
            }
        }
        if (remaining <= 0)
        {
            return 0;
        }
        if (currentSlot.currentItem == null)
        {
            remaining = currentSlot.AddItemToSlot(item, amount);
        }
        return remaining;
    }
    public static int AddItemToInventory(Item item, int amount)
    {
        int remaining = amount;
        //Debug.LogError("add");

        //check slots that contain same item
        foreach (InventorySlot slot in slots)
        {
            if (slot.currentItem == item)
            {
                int overflow = slot.AddItemToSlot(item, remaining);

                if (overflow > 0)
                {
                    remaining = overflow;
                }
                else
                {
                    remaining = 0;
                }
            }
        }
        if (remaining <= 0)
        {
            return 0;
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.currentItem == null)
            {
                remaining = slot.AddItemToSlot(item, remaining);
            }
            if (remaining <= 0)
            {
                break;

            }
        }

        return remaining;
    }
    
    public void RemoveCurrentItem(int slotIndex, Item _currentItem, int _currentAmount)
    {
        int remaining = _currentAmount;
        if (slots[slotIndex].currentItem == _currentItem)
        {
            if (remaining >= slots[slotIndex].currentItemAmount)
            {
                remaining -= slots[slotIndex].currentItemAmount;
                slots[slotIndex].SetItemInSlot(null, 0);
            }
            else
            {
                slots[slotIndex].SetItemInSlot(_currentItem, slots[slotIndex].currentItemAmount - remaining);
                remaining = 0;
            }
            if (remaining <= 0)
            {
                return;
            }
        }
       
    }
   
    public bool CheckItem(Item item, int amount)
    {
        int remaining = amount;

        foreach (InventorySlot slot in slots)
        {
            if (slot.currentItem == item)
            {
                remaining -= slot.currentItemAmount;
            }

            if (remaining <= 0)
            {
                return true;
            }
        }

        return false;
    }
    public void RemoveItem(Item _currentItem, int _currentItemAmount)
    {
        _currentItem = null;
        _currentItemAmount = 0;
    }

    
    

}
