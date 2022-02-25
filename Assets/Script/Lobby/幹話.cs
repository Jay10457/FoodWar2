using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class 幹話 : Windows<幹話>
{
    public void 講幹話(string v)
    {
        內容.text = v;
        Open();
    }
    public Text 內容 = null;
}
