using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class 創建房間 : Windows<創建房間>
{
    [SerializeField] InputField 房名 = null;
    [SerializeField] Slider 人數拉桿 = null;
    public override void OnOpen()
    {
        base.OnOpen();
        房名.text = Random.Range(1000, 9999).ToString();
    }
    public void 創建房間按鈕()
    {
        if (房名.text == "")
        {
            幹話.ins.講幹話("房名不可為空。");
            return;
        }
        Lobby.ins.建立房間(房名.text, (int)人數拉桿.value);
        Close();
    }
}