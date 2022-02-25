using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 設定頭像 : MonoBehaviour
{
    public List<GameObject> 頭像們;
    public void 選擇頭像(int id)
    {
        頭像們[id].SetActive(true);
        if (id <= 4)
        {
            // 如果我是好食物 我就排在前面
            this.transform.SetAsFirstSibling();
        }
      
    }
}
