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
    [SerializeField] GameObject 開始鈕 = null;
    public void Start()
    {
        刷新人數顯示();
        Invoke("CreateMe", 0.3f);
    }
    void CreateMe()
    {
        // 為自己生成一個玩家實體
        PhotonNetwork.Instantiate("UIPlayer", Vector3.zero, Quaternion.identity);
        開始鈕.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    [SerializeField] Text 人數 = null;
    void 刷新人數顯示()
    {
        人數.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers+"\n"+ PhotonNetwork.CurrentRoom.Name;
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
        GameObject temp =  Instantiate(頭像, 背景);
        temp.transform.GetChild(0).GetComponent<Text>().text = 玩家名稱;
        temp.GetComponent<設定頭像>().選擇頭像(characterID);
        temp.name = userID;
        所有頭像.Add(temp);
        characterIDList.Add(characterID);
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
        SceneManager.LoadSceneAsync("S01");
    }
}
