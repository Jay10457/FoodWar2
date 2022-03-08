using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{
    InventorySlot[] slots;
    Transform currentEquip;
    Item lastItem;

    int currentSlotIndex;

    private void Start()
    {
        slots = GetComponentsInChildren<InventorySlot>();
    }
    private void Update()
    {
        //Scale up currently selected slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == currentSlotIndex)//&& slots[i].currentItem != null
            {
                slots[i].transform.localScale = Vector3.one * 1.1f;
                slots[currentSlotIndex].transform.GetChild(1).GetComponent<Image>().enabled = true;

            }

            else
            {
                slots[i].transform.localScale = Vector3.one;
                slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            }

        }
        if (Input.mouseScrollDelta.y < 0)
        {
            if (currentSlotIndex >= slots.Length - 1)
                currentSlotIndex = 0;
            else
                currentSlotIndex++;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            if (currentSlotIndex <= 0)
                currentSlotIndex = slots.Length - 1;
            else
                currentSlotIndex--;

        }
        if (lastItem != slots[currentSlotIndex].currentItem)
        {
            lastItem = slots[currentSlotIndex].currentItem;

            //Destroy previously equipped item
            if (currentEquip)
            {
                Destroy(currentEquip.gameObject);
            }
            //Instantiate equip item if currentItem type = Hand
            if (slots[currentSlotIndex].currentItem != null/*&& slots[currentSlotIndex].currentItem.type == Item.Type.Weapons*/)
            {
                //Debug.LogError(string.Format("current equip is {0}", slots[currentSlotIndex].currentItem));
                //currentEquip = Instantiate(slots[currentSlotIndex].currentItem.equipPrefab, equipParent).transform;
                //currentEquip.localPosition = Vector3.zero;
            }

        }
    }
}
