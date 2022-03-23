using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class 房間列表 : WindowsPun<房間列表>
{
    public override void Open()
    {
        base.Open();
        // 沒進就近一下 然後不用出來沒差
        if (PhotonNetwork.InLobby == false)
        PhotonNetwork.JoinLobby();
    }
    public override void OnClose()
    {
        base.OnClose();
        myRoomList.Clear();
    }

    Dictionary<string, RoomInfo> myRoomList = new Dictionary<string, RoomInfo>();
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for(int i = 0; i < 垃圾桶.Count; i++)
        {
            Destroy(垃圾桶[i]);
        }
        垃圾桶.Clear();

        foreach (var r in roomList)
        {
            if (!r.IsOpen || !r.IsVisible || r.RemovedFromList)
            {
                if (myRoomList.ContainsKey(r.Name))
                {
                    myRoomList.Remove(r.Name);
                }
                continue;
            }
            if (myRoomList.ContainsKey(r.Name))
            {
                myRoomList[r.Name] = r;
            }
            else
            {
                myRoomList.Add(r.Name, r);
            }
        }

        // 顯示內容
        //背景.sizeDelta = new Vector2(背景.sizeDelta.x, 100f* roomList.Count);
        foreach (var r in roomList)
        {
            GameObject temp = Instantiate(房間物件, 背景);
            temp.transform.GetChild(0).GetComponent<Text>().text = r.Name;
            temp.transform.GetChild(1).GetComponent<TMP_Text>().text = r.PlayerCount + " / " + r.MaxPlayers;
            temp.GetComponent<房間物件>().房名 = r.Name;
            垃圾桶.Add(temp);
        }
    }
    [SerializeField] RectTransform 背景 = null;
    [SerializeField] GameObject 房間物件 = null;
    List<GameObject> 垃圾桶 = new List<GameObject>();
}
