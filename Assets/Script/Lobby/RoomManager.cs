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
    [SerializeField] Transform[] 生成點 = new Transform[0];
    [SerializeField] Transform[] goodSpawnPoint = new Transform[0];
    [SerializeField] Transform[] badSpawnPoint = new Transform[0];
    public CookManager myPlayer = null;
    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < goodSpawnPoint.Length; i++)
            {
                PhotonNetwork.Instantiate("GoodPot", goodSpawnPoint[i].position, Quaternion.identity);
                PhotonNetwork.Instantiate("BadPot", badSpawnPoint[i].position, Quaternion.identity);
            }
            
            
            
        }
       
        // 每個人都生成自己的角色到場地上
        GameObject.Instantiate(mainCam, mainCam.transform.position, mainCam.transform.rotation);
        myPlayer = PhotonNetwork.Instantiate("Player", 生成點[Random.Range(0, 生成點.Length)].position, Quaternion.identity).GetComponent<CookManager>();
        GameObject.Instantiate(playerUI, playerUI.transform.position, playerUI.transform.rotation);
        


    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "S01")
        {





        }


    }
}

