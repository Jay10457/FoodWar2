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
    [SerializeField] Transform[] �ͦ��I = new Transform[0];
    private void Start()
    {
        // �C�ӤH���ͦ��ۤv���������a�W
        PhotonNetwork.Instantiate("Player", �ͦ��I[Random.Range(0, �ͦ��I.Length)].position, Quaternion.identity);
    }
}
