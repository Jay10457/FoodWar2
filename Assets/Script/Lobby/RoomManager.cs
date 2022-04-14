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
    public CookController myPlayer = null;
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < goodSpawnPoint.Length; i++)
            {
                PhotonNetwork.InstantiateRoomObject("GoodPot", goodSpawnPoint[i].position, Quaternion.Euler(0, Random.Range(-360, 360), 0));
                PhotonNetwork.InstantiateRoomObject("BadPot", badSpawnPoint[i].position, Quaternion.Euler(0, Random.Range(-360, 360), 0));
            }
            PhotonNetwork.InstantiateRoomObject("ScoreManager", transform.position, Quaternion.identity);
            
            
            
        }
       
        // �C�ӤH���ͦ��ۤv���������a�W
        GameObject.Instantiate(mainCam, mainCam.transform.position, mainCam.transform.rotation);
        myPlayer = PhotonNetwork.Instantiate("Player", �ͦ��I[Random.Range(0, �ͦ��I.Length)].position, Quaternion.identity).GetComponent<CookController>();
        GameObject.Instantiate(playerUI, playerUI.transform.position, playerUI.transform.rotation);
        


    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "S01")
        {





        }


    }
    public void MeLeftRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}

