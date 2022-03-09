using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

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

    public void RemoveItemFromInventory(Item item, int amount)
    {
        int remaining = amount;

        foreach (InventorySlot slot in slots)
        {
            if (slot.currentItem == item)
            {
                if (remaining >= slot.currentItemAmount)
                {
                    remaining -= slot.currentItemAmount;
                    slot.SetItemInSlot(null, 0);
                }
                else
                {
                    slot.SetItemInSlot(item, slot.currentItemAmount - remaining);
                    remaining = 0;
                }

                if (remaining <= 0)
                    return;
            }

        }
    }

    public void DropItem(Item item, int amount, bool removeCurrentItem = true)
    {

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
    public void RemoveItem()
    {
        currentItem = null;
        currentItemAmout = 0;
    }

    public void BinItem()
    {
        RemoveItem();
    }

}
