using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookUI : MonoBehaviour
{
    public static CookUI instance;
    [SerializeField] Button[] cookSlots;
    [SerializeField] Button startCooking;
    [SerializeField] GameObject[] BGs;
    [SerializeField] Vector3 offset;

    int cId;
    
    public bool _isCookerOpen;
    Vector3 lookAt;



    private Camera cam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cam = Camera.main;
        cId = SaveManager.instance.nowData.characterID;

        if (cId <= 4)
        {
            BGs[1].SetActive(true);
        }
        else
        {
            BGs[0].SetActive(true);
        }
    }
 
    
    public void SetCookerUIBillboard(Vector3 lookAtPos)
    {
       
        Vector3 pos = cam.WorldToScreenPoint(lookAtPos + offset);
        
        if (transform.position != pos) transform.position = pos;
    }
    

}
