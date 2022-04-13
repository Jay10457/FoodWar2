using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : MonoBehaviourPunCallbacks
{
   public void SendScoreRequest(int _score)
    {
        photonView.RPC("SendAddScoreRequest", RpcTarget.MasterClient, _score);
    }

    [PunRPC]
    private void SendAddScoreRequest(int _score)
    {
        Debug.LogError(_score);
    }
}
