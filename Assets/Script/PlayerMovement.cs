using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] Rigidbody rb = null;
    [SerializeField] Transform playerPos = null;
    [SerializeField] Transform cameraPos = null;
    float Speed = 5f;
    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        // 如果我是本尊
        if (photonView.IsMine)
        {
            cameraPos = GameObject.Find("CameraPos").transform;
            photonView.RPC("SetSkin", RpcTarget.All, SaveManager.instance.nowData.characterID);
        }
        else // 如果我是鏡像
        {

        }
    }
    [PunRPC]
    public void SetSkin(int cid)
    {

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Movement();
        }
        else
        {
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Movement();
        }
        else
        {
            // 鏡像要漸變到本尊位置
            rb.MovePosition(Vector3.Lerp(rb.position, rpcPos, Time.deltaTime * 鏡像同步速率));
        }
    }

    Vector3 lestPos = Vector3.zero;
    [SerializeField] float 多遠要同步 = 0.1f;
    void Movement()
    {
        float horizotalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(horizotalMove * Speed, Mathf.Clamp(rb.velocity.y, -20f, 20f), verticalMove * Speed);
        // 消極的同步座標 如果本尊移動位置就同步發送給鏡像
        float d = Vector3.Distance(rb.position, lestPos);
        if (d > 多遠要同步)
        {
            photonView.RPC("GetPos", RpcTarget.Others, rb.position);
        }
    }
    Vector3 rpcPos = Vector3.zero;
    [PunRPC]
    public void GetPos(Vector3 pos)
    {
        // 只有鏡像會收到位置訊息
        rpcPos = pos;
    }
    [SerializeField] float 鏡像同步速率 = 10f;
    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            if (cameraPos != null)
            {
                cameraPos.transform.position = playerPos.transform.position;
            }
        }
        else
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.name == "爐子")
            {
                collision.gameObject.GetComponent<爐子>().OpenOrClose();
            }
        }
        else
        {

        }
    }
}