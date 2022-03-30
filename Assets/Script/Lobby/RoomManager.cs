using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance = null;
    [SerializeField] GameObject playerUI = null;
    [SerializeField] GameObject mainCam = null;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] Transform[] �ͦ��I = new Transform[0];
    [SerializeField] Transform[] goodSpawnPoint = new Transform[0];
    [SerializeField] Transform[] badSpawnPoint = new Transform[0];
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("GoodPot", goodSpawnPoint[0].position, Quaternion.identity);

            PhotonNetwork.Instantiate("BadPot", badSpawnPoint[0].position, Quaternion.identity);
        }
       
        // �C�ӤH���ͦ��ۤv���������a�W
        GameObject.Instantiate(mainCam, mainCam.transform.position, mainCam.transform.rotation);
        PhotonNetwork.Instantiate("Player", �ͦ��I[Random.Range(0, �ͦ��I.Length)].position, Quaternion.identity);
        GameObject.Instantiate(playerUI, playerUI.transform.position, playerUI.transform.rotation);
        


    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "S01")
        {





        }


    }
}

