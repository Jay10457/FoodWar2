using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinOrLost : MonoBehaviour
{
    public Animator gameOverAni;
    public TMP_Text rScore;
    public TMP_Text gScore;
    public Slider bar;
    public static WinOrLost instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void Button()
    {
        RoomManager.instance.MeLeftRoom();
    }
}
