using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPing : MonoBehaviour
{
    Text pingText;

    private void Awake()
    {
        pingText = GetComponent<Text>();
    }


    private void FixedUpdate()
    {
        pingText.text = string.Format("{0}/ms", Photon.Pun.PhotonNetwork.GetPing().ToString());
    }
}
