using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HotBar : MonoBehaviourPunCallbacks
{
    [SerializeField] InventorySlot[] slots;
    [SerializeField] GameObject gameUI = null;
    [SerializeField] Transform playerPos = null;


    Transform currentEquip;
    Item lastItem;
    Item currentItem;
    
   
    int currentItemAmount;
    string dropItemName;
    Vector3 itemSpawnPos = Vector3.zero;

    InventoryManager IM;


    int currentSlotIndex;

    private void Start()

    {
        playerPos = this.gameObject.transform;
        gameUI = GameObject.FindWithTag("GameUI");
        slots = gameUI.GetComponentsInChildren<InventorySlot>();
        IM = GameObject.FindObjectOfType<InventoryManager>();

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
        if (Input.GetKeyDown(KeyCode.Q) && slots[currentSlotIndex].currentItem != null)
        {
            if (photonView.IsMine)
            {
                DropItem(slots[currentSlotIndex].currentItem, slots[currentSlotIndex].currentItemAmount);
                IM.RemoveCurrentItem(currentSlotIndex, slots[currentSlotIndex].currentItem, slots[currentSlotIndex].currentItemAmount);
               
                
            }
            else
            {
                
            }
            


            


        }

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Weapon")
            {
                ItemPickUp itemPick = collision.transform.GetComponent<ItemPickUp>();
                int remaining = InventoryManager.AddItemToInventory(itemPick._item, itemPick.itemAmount);

                if (remaining > 0)
                {
                    itemPick.itemAmount = remaining;
                }
                else
                {
                   itemPick.KillMe();
                }
            }
        }
       
    }
    

    public void DropItem(Item item, int amount, bool removeCurrentItem = true)
    {
      
        if (item == null)
            return;
        Vector3 random = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0f, 0.2f), Random.Range(-0.2f, 0.2f));
        Vector3 direction = playerPos.forward + random;
        itemSpawnPos = playerPos.position + direction * 5;
        if (item.dropGameObj().name == "OilSprayGun")
        {
            dropItemName = "OilSprayGun";
        }
        else
        {
            dropItemName = "GumBomb";
        }

       // ItemPickUp drop = (PhotonNetwork.Instantiate(dropItemName, itemSpawnPos, Quaternion.identity) as GameObject).GetComponent<ItemPickUp>();
        //photonView.RPC("DropRPC", RpcTarget.Others, dropItemName, itemSpawnPos);
        // 建立網路物件
        GameObject dropTemp = PhotonNetwork.Instantiate(dropItemName, itemSpawnPos, Quaternion.identity);
        ItemPickUp tempItemPickUp = dropTemp.GetComponent<ItemPickUp>();
        // 要求該物件改變(該物件自己同步)
        tempItemPickUp.SetUpPickupable(item.name, amount);

        
        if (removeCurrentItem)
        {
            currentItem = null;
            currentItemAmount = 0;

        }

        
    }
   
}

