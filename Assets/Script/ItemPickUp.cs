using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    Item _item;
    int itemAmount;



    public void SetUpPickupable(Item item, int amount)
    {
        _item = item;
        itemAmount = amount;
        GetComponentInChildren<SpriteRenderer>().sprite = item.itemSprite;
        item.itemPrefab = GetComponent<GameObject>().gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int remaining = InventoryManager.AddItemToInventory(_item, itemAmount);

            if (remaining > 0)
            {
                itemAmount = remaining;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
