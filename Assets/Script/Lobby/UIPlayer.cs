using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class UIPlayer : MonoBehaviourPunCallbacks
{
    public string playerName = "";
    int characterID = -1;
    private void Start()
    {
        // 如果我是本尊就設定玩家名稱
        if (photonView.IsMine)
        {
            // 決定用誰
            this.gameObject.name = photonView.Owner.UserId;
            int characterID = CharacterSelection();
            SaveManager.instance.nowData.characterID = characterID;
            photonView.RPC("SetPlayerName", RpcTarget.AllBuffered, SaveManager.instance.nowData.playerName, characterID);
        }
    }

    [PunRPC]
    public void SetPlayerName(string v, int characterID)
    {
        playerName = v;
        this.characterID = characterID;
        // 有名子之後要在房間中新增一個自己的介面
        房間中.ins.新增頭像(photonView.Owner.UserId, v, characterID);
    }
    private void OnDestroy()
    {
        if (房間中.ins != null)
        房間中.ins.移除頭像(photonView.Owner.UserId, characterID);
    }

    // 1.確認雙方哪邊人比較多
    // 2.從人比較少的一個隊伍抽ID
    // 2-2. 如果兩邊人數一樣就隨機選個隊伍抽
    // 3. 同步並生成UI

    /// <summary>指定取得隊伍的隨機英雄</summary>
    public int GetOkCharacterID(FoodTeam team)
    {
        int temp = 0;
        if (team == FoodTeam.GOOD)
            temp = Random.Range(0, 5);
        else if (team == FoodTeam.BAD)
            temp = Random.Range(6, 10);
        // 如果這個ID跟房中的任何人重複
        for (int i = 0; i < 房間中.ins.characterIDList.Count; i++)
        {
            if (房間中.ins.characterIDList[i] == temp)
            {
                return GetOkCharacterID(team);
            }
        }
        return temp;
    }
    /// <summary>回傳有幾個玩家</summary>
    public int GetTeamCount(FoodTeam team)
    {
        int toteal = 0;
        for (int i = 0; i < 房間中.ins.characterIDList.Count; i++)
        {
            if (team == FoodTeam.GOOD && 房間中.ins.characterIDList[i] <= 4)
            {
                toteal++;
            }
            if (team == FoodTeam.BAD && 房間中.ins.characterIDList[i] >= 5)
            {
                toteal++;
            }
        }
        return toteal;
    }

    public int CharacterSelection()
    {
        int 好食物的數量 = GetTeamCount(FoodTeam.GOOD);
        int 壞食物的數量 = GetTeamCount(FoodTeam.BAD);

        

        if (好食物的數量 == 壞食物的數量)
        {
            if (Random.Range(0f, 100f) > 50f)
                return GetOkCharacterID(FoodTeam.GOOD);
            else
                return GetOkCharacterID(FoodTeam.BAD);
        }
        else if (好食物的數量 > 壞食物的數量)
        {
            return GetOkCharacterID(FoodTeam.BAD);
        }
        else
        {
            return GetOkCharacterID(FoodTeam.GOOD);
        }
    }
}
