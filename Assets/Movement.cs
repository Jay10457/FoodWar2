using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Movement : MonoBehaviourPunCallbacks
{
    Animator ani;
    Rigidbody rb;
    //[SerializeField] Transform playerPos;
    [SerializeField] Transform camLookAt = null;
    [SerializeField] CinemachineVirtualCamera Vcam = null;
    [SerializeField] Vector3 playerMoveInput = Vector3.zero;
    [SerializeField] float yaw = 0;
    [SerializeField] float pitch = 0;
    [SerializeField] float velocity = 10;
    [SerializeField] Vector3 moveDir = Vector3.zero;
    [SerializeField] float MOUSE_SENTIVY = 10f;
    [SerializeField] float syncRate = 10f;
    [SerializeField] float distanceToSync = 0.1f;
    public List<GameObject> characters;
    float angle = 0;
    Vector3 angles = Vector3.zero;

    Vector3 rpcPos = Vector3.zero;
    Quaternion rpcRot = Quaternion.identity;
    Vector3 lastPos = Vector3.zero;

    
    Quaternion nextRotation;



  
    private void Start()
    {
        

      

        Cursor.lockState = CursorLockMode.Locked;
        if (photonView.IsMine)
        {

            
            photonView.RPC("SetCharacter", RpcTarget.All, 1);//SaveManager.instance.nowData.characterID
            Vcam = FindObjectOfType<CinemachineVirtualCamera>();
            camLookAt = this.gameObject.transform.Find("camLookAt");

            if (true)// if team == green
            {
                camLookAt.transform.localPosition = new Vector3(0, 3.5f, 0);
            }
            else // if team == red 
            {
                camLookAt.transform.localPosition = new Vector3(0, 3, 0);
            }
           
           
          



        }
        else
        {

        }
        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

    }
    [PunRPC]
    public void SetCharacter(int cid)
    {
        characters[cid].SetActive(true);
    }

   

    //PlayerInput;
    private void Update()
    {
        InputMagnitude();
        
    }

    void InputMagnitude()
    {
        playerMoveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        yaw = -Input.GetAxis("Mouse Y");
        pitch = Input.GetAxis("Mouse X");
        
        
    }

    private void LateUpdate()
    {
        if (photonView.IsMine)
        {
            
            Vcam.Follow = camLookAt;
            CameraRotation();
        }
        else
        {

        }
        
    }

    private void Move()
    {
        moveDir = Vector3.ClampMagnitude(transform.TransformDirection(playerMoveInput), 1) * velocity;
        //moveDir = transform.TransformDirection(playerMoveInput) * velocity;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

        // 消極的同步座標 如果本尊移動位置就同步發送給鏡像
        float d = Vector3.Distance(rb.position, lastPos);
        if (d > distanceToSync)
        {
            photonView.RPC("GetPos", RpcTarget.Others, rb.position);
        }


        AnimatePlayer();

    }
    [PunRPC]

    public void GetPos(Vector3 pos)
    {
        rpcPos = pos;
    }
    [PunRPC]

    public void GetRot(Quaternion rot)
    {
        rpcRot = rot;
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
            PlayerRotate();
        }
        else
        {
            rb.MovePosition(Vector3.Lerp(rb.position, rpcPos, Time.deltaTime * syncRate));
        }
      
    }

    private void CameraRotation()
    {
        camLookAt.transform.rotation *= Quaternion.AngleAxis(pitch * MOUSE_SENTIVY, Vector3.up);
        camLookAt.transform.rotation *= Quaternion.AngleAxis(yaw * MOUSE_SENTIVY, Vector3.right);
        angles = camLookAt.transform.localEulerAngles;
        angles.z = 0;
        angle = camLookAt.transform.localEulerAngles.x;


        //Clamp down and Up
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 45)
        {
            angles.x = 45;
        }
        

        camLookAt.transform.localEulerAngles = angles;
        //nextRotation = Quaternion.Lerp(camLookAt.transform.rotation, nextRotation, Time.deltaTime * 0.5f);
        
       
    }

    private void AnimatePlayer()
    {
        ani.SetFloat("MoveX", playerMoveInput.x);
        ani.SetFloat("MoveZ", playerMoveInput.z);
    }

    private void PlayerRotate()
    {
       ;
        Quaternion camDir = Quaternion.Euler(0, camLookAt.transform.rotation.eulerAngles.y, 0);
        //playerPos.rotation = Quaternion.Lerp(playerPos.rotation, camDir, Time.fixedDeltaTime * 10f);

        //rb.MoveRotation(Quaternion.Lerp(rb.rotation, camDir, Time.fixedDeltaTime * 2f));
        
        rb.MoveRotation(camDir);
        camLookAt.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

    }


}
