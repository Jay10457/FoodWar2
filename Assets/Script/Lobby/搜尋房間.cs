using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class 搜尋房間 : Windows<搜尋房間>
{
    [SerializeField] InputField 房名 = null;
    public void 搜尋房間按鈕()
    {
        if (房名.text == "")
        {
            幹話.ins.講幹話("房名不可為空。");
            return;
        }
        Lobby.ins.進入房間(房名.text);
        Close();
    }
}
