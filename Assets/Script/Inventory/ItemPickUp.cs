using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemPickUp : MonoBehaviourPunCallbacks
{
    public Item _item;
    public int itemAmount;
    float destroyTime = 5;

   
    public IEnumerator countDownToDetroy()
    {
        float startTime = Time.time;
        while (Time.time < startTime + destroyTime)
        {
            yield return null;
        }
        KillMe();
    }
    public void SetUpPickupable(string itemName, int amount)
    {
        photonView.RPC("RPCSetUpPickupable", RpcTarget.AllBuffered, itemName, amount);
    }

    [PunRPC]
    public void RPCSetUpPickupable(string itemName, int amount)
    {
        _item = ItemManager.instance.GetItmeByName(itemName);
        itemAmount = amount;
        GetComponentInChildren<SpriteRenderer>().sprite = _item.itemSprite;
    }
    public void KillMe()
    {
        // 本地端無論如何都先消失 關碰撞 不動
        this.transform.localScale = Vector3.zero;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;

        photonView.RPC("RpcKillMe", photonView.Owner);
    }
    [PunRPC]
    public void RpcKillMe()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    
}
