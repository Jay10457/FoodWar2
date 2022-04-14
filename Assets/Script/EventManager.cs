using System;


public class EventManager
{
    static EventManager _instance = null;
    public static EventManager instance
    {
        get
        {
            // 當有人需要我的時候 因為我像阿飄一樣沒有實體 就自我創造在虛空中
            if (_instance == null)
                _instance = new EventManager();
            return _instance;
        }
    }

    public Action<int> AddScore;
    public Action GameOver;


}
