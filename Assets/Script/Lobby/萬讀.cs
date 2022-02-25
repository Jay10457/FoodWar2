using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class 萬讀 : MonoBehaviour
{
    public static 萬讀 ins = null;
    private void Awake()
    {
        ins = this;
    }
    [SerializeField] Animator anim = null;
    public bool isOpen
    {
        get { return anim.GetBool("IsRun"); }
        set { anim.SetBool("IsRun", value); }
    }
    [SerializeField] Text 內容 = null;
    public string info
    {
        get { return 內容.text; }
        set { 內容.text = value; }
    }
}
