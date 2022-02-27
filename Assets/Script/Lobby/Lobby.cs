using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class Lobby : MonoBehaviourPunCallbacks
{
    #region 連線
    public static Lobby ins = null;
    private void Awake()
    {
        ins = this;
        

    }

    void Start()
    {
       
        // 自動將所有人拉到房主的場景
        PhotonNetwork.AutomaticallySyncScene = true;

        // 載入玩家資料
        SaveManager.instance.LoadPlayerData_0();

        // 設定暱稱
        PhotonNetwork.NickName = SaveManager.instance.nowData.playerName;

        // 如果已經連過了就別連
        if (PhotonNetwork.IsConnected)
        {
            OnConnectedToMaster();
            return;
        }
        
        萬讀.ins.info = "開始連線到伺服器...";
        萬讀.ins.isOpen = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        萬讀.ins.info = "連上伺服器...";
        萬讀.ins.isOpen = false;
        連上了 = true;
        
        OpenMenu();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        萬讀.ins.isOpen = false;
        搜尋房間.ins.Close();
        房間列表.ins.Close();
        創建房間.ins.Close();
        連上了 = false;
        if (cause == DisconnectCause.MaxCcuReached)
        {
            幹話.ins.講幹話("遊戲爆滿了請稍後在試。");
            Invoke("Start", 5f);
        }
        else
        {
            幹話.ins.講幹話("斷線了。");
            Invoke("Start", 5f);
        }
    }
    public bool 連上了 = false;
    // ----------------------------------------------
    #endregion
    public void OpenMenu()
    {
        if (連上了)
        {
            StartGameLobby.ins.Open();
        }
    }
    #region 創建房間
    public void 開主選單()
    {
        if (連上了)
        {
            主選單.ins.Open();
            BlockUI.ins.Open();
        }
        else
        {
            幹話.ins.講幹話("目前沒有連上機房，請稍後再試。");
        }
           
    }
    public void 建立房間(string v, int number)
    {
        萬讀.ins.info = "創建房間中...";
        萬讀.ins.isOpen = true;

        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = (byte)number;
        ro.PublishUserId = true;
        PhotonNetwork.CreateRoom(v, ro);
    }
    public override void OnJoinedRoom()
    {
        // 任何情況下進入房間都會來到此
        萬讀.ins.isOpen = false;
        // 房主決定到哪個場景 房客會自動跟隨
        if (PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene(1);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        萬讀.ins.isOpen = false;
        幹話.ins.講幹話("房間名稱衝突了。");
    }
    #endregion

    public void 進入房間(string 房名)
    {
        萬讀.ins.isOpen = true;
        萬讀.ins.info = "正在加入房間...";
        PhotonNetwork.JoinRoom(房名);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        萬讀.ins.isOpen = false;
        幹話.ins.講幹話("房間人數已滿或房間不存在。");
    }
}