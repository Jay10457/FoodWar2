using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class CookManager : MonoBehaviourPunCallbacks
{

    public bool inCookerArea;
    public bool isOpenCooker;
    [SerializeField] Transform currentCookerTrans = null;
    [SerializeField] CookUI cookUI;
    [SerializeField] PlayerController playerController;
    PhotonView PV;
    string userId;
    

    private void Start()
    {
        PV = this.GetComponent<PhotonView>();
        userId = PV.Owner.UserId;
        cookUI = CookUI.instance;
        cookUI.gameObject.SetActive(false);
        playerController = gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        SendOpenCookUIRequest();
    }
    

    private void SendOpenCookUIRequest()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCookerArea)
        {
            if (PV.IsMine)
            {
                currentCookerTrans.gameObject.GetComponent<Cooker>().SendToServerCooker(userId);
            }

              
            
        }
    }

  




    /*
    private void CloseCooker()
    {
        if (cookUI.gameObject.activeSelf)
        {
            cookUI.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpenCooker = false;
            cookUI._isCookerOpen = false;
            if (currentCooker != null)
            {
                currentCooker.isOccupy = false;
            }

        }




    }
    private void OpenCooker()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCookerArea)
        {
            playerController.yaw = 0f;
            playerController.pitch = 0f;

            cookUI.gameObject.SetActive(true);
            if (cookUI.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                currentCooker.isOccupy = true;
                isOpenCooker = true;
                cookUI._isCookerOpen = true;
                currentCooker.openRemain.SetActive(false);
                //Debug.LogError(currentCooker.gameObject.name);

            }


        }
        else if (Input.GetKeyDown(KeyCode.E) && inCookerArea && currentCooker.isOccupy && cookUI.gameObject.activeSelf)
        {
            CloseCooker();

            currentCooker.openRemain.SetActive(true);
        }






    }
    */

    #region CheckPot

    private void FixedUpdate()
    {
        if (playerController.isGameBegin)
        {
            GetPot(5, 60);
        }
    }
    public Cooker currentCooker;
    public void GetPot(int radius, int angle)
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, radius, 1 << LayerMask.NameToLayer("Pot"));

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {

                Vector3 dir = cols[i].transform.position - this.transform.position;
                if (Vector3.Angle(dir, this.transform.forward) < angle)
                {

                    currentCooker = cols[i].GetComponent<Cooker>();
                    
                    if (currentCooker.cookerTeam.ToString() == playerController.teamValue.ToString())
                    {
                        currentCookerTrans = cols[i].transform;
                        inCookerArea = true;
                    }

                }
                else if (currentCooker != null)
                {
                    inCookerArea = false;
                    currentCooker = null;
                    currentCookerTrans = null;
                }



            }
        }
        else if (currentCooker != null)
        {

            inCookerArea = false;
            currentCooker = null;
            currentCookerTrans = null;
        }

    }
#endregion


}
