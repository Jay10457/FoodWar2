using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class 創建房間 : Windows<創建房間>
{
    [SerializeField] InputField 房名 = null;
    [SerializeField] Button plusConnection;
    [SerializeField] Button minusConnection;
    [SerializeField] TMP_Text count;
    int connectionCount = 0;
    public override void OnOpen()
    {
        base.OnOpen();
        房名.text = Random.Range(1000, 9999).ToString();
        connectionCount = 4;
        count.text = 4.ToString();
    }

    public void OnConnectionPlus()
    {
        connectionCount = 6;
        count.text = 6.ToString();

    }
    public void OnConnectionMinus()
    {
        connectionCount = 4;
        count.text = 4.ToString();
    }
    public void 創建房間按鈕()
    {
        if (房名.text == "")
        {
            幹話.ins.講幹話("房名不可為空。");
            return;
        }
        Lobby.ins.建立房間(房名.text, connectionCount);
        Close();
    }
}