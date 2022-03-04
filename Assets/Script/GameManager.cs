using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager
{
    public static GameManager instance
    {
        get
        {
            // 當有人需要我的時候 因為我像阿飄一樣沒有實體 就自我創造在虛空中
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }
    static GameManager _instance = null;

    public bool 從房間退房 = false;

    
}

public enum FoodTeam
{
    /// <summary>0~4是好食物的英雄腳色</summary>
    GOOD,
    /// <summary>5~9是不健康食物的英雄腳色</summary>
    BAD
}

