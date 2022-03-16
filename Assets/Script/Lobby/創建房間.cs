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
    [SerializeField] Image fourCount;
    [SerializeField] Image sixCount;
    int connectionCount = 0;
    public override void OnOpen()
    {
        base.OnOpen();
        房名.text = Random.Range(1000, 9999).ToString();
        connectionCount = 4;
        fourCount.gameObject.SetActive(true);
        
    }

    public void OnConnectionPlus()
    {
        connectionCount = 6;
        fourCount.gameObject.SetActive(false);
        sixCount.gameObject.SetActive(true);


    }
    public void OnConnectionMinus()
    {
        connectionCount = 4;
        fourCount.gameObject.SetActive(true);
        sixCount.gameObject.SetActive(false);
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