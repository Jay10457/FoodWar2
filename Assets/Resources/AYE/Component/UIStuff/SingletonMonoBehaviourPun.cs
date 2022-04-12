using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SingletonMonoBehaviourPun<T> : MonoBehaviourPunCallbacks where T : class
{
    static public T ins = null;
    virtual protected void Awake()
    {
        ins = this as T;
    }
}
