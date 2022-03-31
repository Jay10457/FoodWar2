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
    public Cooker currentCooker;
    PhotonView PV;
    public string userId;
    

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
        SendCookUIRequest();
    }
    

    private void SendCookUIRequest()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCookerArea && !CookUI.instance.gameObject.activeSelf)
        {
            //Open
            if (PV.IsMine)
            {
                currentCooker.SendOpenRequestServerCooker(userId);
                
            }

              
            
        }
        else if (Input.GetKeyDown(KeyCode.E) && inCookerArea && CookUI.instance.gameObject.activeSelf)
        {
            //Close
            if (PV.IsMine)
            {
                currentCooker.SendCloseRequestServerCooker(userId);
            }
        }
    }

  





    #region CheckPot

    private void FixedUpdate()
    {
        if (playerController.isGameBegin)
        {
            GetPot(5, 60);
        }
    }
    
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
                        if (!CookUI.instance.gameObject.activeSelf)
                        {
                            currentCooker.openRemain.SetActive(true);
                        }
                        else
                        {
                            currentCooker.openRemain.SetActive(false);
                        }
                       
                        currentCookerTrans = cols[i].transform;
                        inCookerArea = true;
                    }

                }
                else if (currentCooker != null)
                {
                    currentCooker.SendCloseRequestServerCooker(userId);
                    currentCooker.openRemain.SetActive(false);
                    inCookerArea = false;
                    currentCooker = null;
                    currentCookerTrans = null;
                }



            }
        }
        else if (currentCooker != null)
        {
            currentCooker.SendCloseRequestServerCooker(userId);
            currentCooker.openRemain.SetActive(false);
            inCookerArea = false;
            currentCooker = null;
            currentCookerTrans = null;
        }

    }
#endregion


}