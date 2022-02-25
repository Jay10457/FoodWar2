using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance = null;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] Transform[] 生成點 = new Transform[0];
    private void Start()
    {
        // 每個人都生成自己的角色到場地上
        PhotonNetwork.Instantiate("Player", 生成點[Random.Range(0, 生成點.Length)].position, Quaternion.identity);
    }
}
