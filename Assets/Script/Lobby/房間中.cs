using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class 房間中 : SingletonMonoBehaviourPun<房間中>
{
    
   
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        刷新人數顯示();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        刷新人數顯示();
    }
    [SerializeField] GameObject StartGameButtom = null;
    [SerializeField] GameObject ClientButtom = null;
    public void Start()
    {
      
      
        刷新人數顯示();
        Invoke("CreateMe", 0.2f);
    }
    void CreateMe()
    {
        // 為自己生成一個玩家實體
         PhotonNetwork.Instantiate("UIPlayer", Vector3.zero, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            
            StartGameButtom.SetActive(true);
            ClientButtom.SetActive(false);
        }
        else
        {
            ClientButtom.SetActive(true);
            StartGameButtom.SetActive(false);
        }

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartGameButtom.SetActive(true);
            ClientButtom.SetActive(false);
        }
        else
        {
            ClientButtom.SetActive(true);
            StartGameButtom.SetActive(false);
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    [SerializeField] Text 人數 = null;
    [SerializeField] Text RoomId = null;
    void 刷新人數顯示()
    {
        人數.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        RoomId.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void 退房()
    {
        PhotonNetwork.LeaveRoom();
        //Debug.LogError("leaveRoom");

    }

    [SerializeField] RectTransform 背景 = null;
    [SerializeField] GameObject 頭像 = null;
    List<GameObject> 所有頭像 = new List<GameObject>();
    public List<int> characterIDList = new List<int>();
    public void 新增頭像(string userID, string 玩家名稱, int characterID)
    {
        GameObject temp = Instantiate(頭像, 背景);
        temp.transform.GetChild(11).GetComponent<Text>().text = 玩家名稱;
        temp.GetComponent<設定頭像>().選擇頭像(characterID);
        temp.name = userID;
        所有頭像.Add(temp);
        characterIDList.Add(characterID);
        //Debug.LogError(characterID);
    }
    public void 移除頭像(string userID, int characterID)
    {
        characterIDList.Remove(characterID);
        for (int i = 0; i < 所有頭像.Count; i++)
        {
            if (所有頭像[i].name == userID)
            {
                Destroy(所有頭像[i]);
                所有頭像.RemoveAt(i);
            }
        }
    }
  
    public void StartGame()
    {

        PhotonNetwork.CurrentRoom.IsOpen = false;
        //SceneManager.LoadSceneAsync("S01");
        PhotonNetwork.LoadLevel("Loading");
        //async = SceneManager.LoadSceneAsync("S01");
        //async.allowSceneActivation = false;
        //loadingPanel.gameObject.SetActive(true);






    }


}
