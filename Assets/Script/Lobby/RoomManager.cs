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
    private void Start()
    {
        OnSceneLoaded();
        // 每個人都生成自己的角色到場地上
        GameObject.Instantiate(mainCam, mainCam.transform.position, mainCam.transform.rotation);
        PhotonNetwork.Instantiate("Player", 生成點[Random.Range(0, 生成點.Length)].position, Quaternion.identity);
        GameObject.Instantiate(playerUI, playerUI.transform.position, playerUI.transform.rotation);


        
    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "S01")
        {
            //Debug.LogError("UI!");
        }
    }
}
