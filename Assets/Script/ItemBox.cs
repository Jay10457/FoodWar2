using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemBox : MonoBehaviourPunCallbacks
{
    [SerializeField] ParticleSystem pickUpEffect;
    public Item[] itemDrops;
    public GameObject itemPrefab;
    public int dropAmount;
    public int dropPerItem;
    
    MeshRenderer mr;
    [SerializeField] [Range(0, 1)] float lerpTime;
    [SerializeField] Color[] lightColor;
    int colorIndex = 0;
    float t;
    int colorLenth;


    static GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
        mr = GetComponent<MeshRenderer>();
        colorLenth = lightColor.Length;
        floating();
        
    }
    void floating()
    {
        LeanTween.moveLocalY(gameObject, this.gameObject.transform.position.y + 1, 1).setLoopPingPong();
       
    }
    private void Update()
    {
        ColorChange();
    }
    void ColorChange()
    {
        
        mr.material.color = Color.Lerp(mr.material.color, lightColor[colorIndex], lerpTime);
        
        t = Mathf.Lerp(t, 1f, lerpTime);
        if (t > 0.9f)
        {
            t = 0;
            colorIndex++;
            colorIndex = (colorIndex >= colorLenth) ? 0 : colorIndex;
        }
    }


    void DropItem()
    {
        for (int i = 0; i < dropAmount; i++)
        {
            //Spawn force and position. Random so they all pop out in different directions
            Vector3 force = new Vector3(Random.Range(-2f, 2f), 2, Random.Range(-2f, 2f));
            ItemPickUp drop = (Instantiate(itemPrefab, transform.position + (force / 4f), Quaternion.identity) as GameObject).GetComponent<ItemPickUp>();
            //drop.SetUpPickupable(itemDrop, dropPerItem);
            drop.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            RandomWeapon();
        }
    }

    void RandomWeapon()
    {
        int WeaponIndex = Random.Range(0, itemDrops.Length);
        dropAmount = itemDrops[WeaponIndex].dropAmount;
        int remaining = InventoryManager.AddItemToInventory(itemDrops[WeaponIndex], dropAmount);
        if (remaining > 0 && remaining == dropAmount)
        {
            
            dropAmount = remaining;
            
            
        }
        else
        {
            
            photonView.RPC("PickUpSend", RpcTarget.All);
            
        }
        



    }
    [PunRPC]

    public void PickUpSend()
    {
        this.gameObject.SetActive(false);
        Instantiate(pickUpEffect, transform.position, Quaternion.identity);
    }
}
