using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class 爐子 : MonoBehaviourPunCallbacks
{
    [SerializeField] ParticleSystem ps = null;
    bool isOpen = true;
    public void OpenOrClose()
    {
        isOpen = !isOpen;
        photonView.RPC("SetOpenClose", RpcTarget.All, isOpen);
    }
    [PunRPC]
    public void SetOpenClose(bool o)
    {
        isOpen = o;
        if (o)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }
}
