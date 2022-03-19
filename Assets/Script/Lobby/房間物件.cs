using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 房間物件 : MonoBehaviour
{
    public string 房名 = "";
    public void 按鈕進房()
    {
        房間列表.ins.Close();
        Lobby.ins.進入房間(房名);
        
    }
    
}