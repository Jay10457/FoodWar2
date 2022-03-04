using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] Animator ani = null;
    [SerializeField] Rigidbody rb = null;
    [SerializeField] Collider characterCollider = null;
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
    [SerializeField] float jumpForce = 0;
    [SerializeField] float sprintForce = 0;
    [SerializeField] float raycastDistance = 0;
    [SerializeField] float sprintTime;
    [SerializeField] float cdTime = 10;
    [SerializeField] SkillCoolDown skillCoolDown;

    public List<GameObject> characters;
    bool jump;
    bool sprint;
    bool isSprinting;
    bool isSkillOk = true;
   

    float angle = 0;
    Color rayColor;

    Vector3 angles = Vector3.zero;

    Vector3 rpcPos = Vector3.zero;
    Vector3 lastPos = Vector3.zero;


    Vector2 lastAnim = Vector2.zero;







    private void Start()
    {


        

        Cursor.lockState = CursorLockMode.Locked;
        if (photonView.IsMine)
        {

            skillCoolDown = GameObject.FindObjectOfType<SkillCoolDown>();
            photonView.RPC("SetCharacter", RpcTarget.All, 0);//SaveManager.instance.nowData.characterID
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
        if (!photonView.IsMine)
        {
            rb.isKinematic = true;

        }

    }




    //PlayerInput;
    private void Update()
    {
        if (photonView.IsMine)
        {
            InputMagnitude(!isSprinting);
            Jump();
            Sprint();

        }


    }
   



    void InputMagnitude(bool sprinting)
    {
        playerMoveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        yaw = -Input.GetAxis("Mouse Y");
        pitch = Input.GetAxis("Mouse X");
        jump = Input.GetKeyDown(KeyCode.Space) ? true : false;
        sprint = Input.GetKeyDown(KeyCode.LeftShift) ? true : false;


    }


    private void LateUpdate()
    {
        if (photonView.IsMine)
        {

            Vcam.Follow = camLookAt;
            CameraRotation();
            AnimatePlayer();
            SyncAnime();
        }

    }

    /// <summary>
    /// Move the Character
    /// </summary>
    private void Move()
    {
        moveDir = Vector3.ClampMagnitude(transform.TransformDirection(playerMoveInput), 1) * velocity;
        //moveDir = transform.TransformDirection(playerMoveInput) * velocity;
        rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

        // �������P�B�y�� �p�G���L���ʦ�m�N�P�B�o�e���蹳
        float d = Vector3.Distance(rb.position, lastPos);
        if (d > distanceToSync)
        {
            photonView.RPC("GetPos", RpcTarget.Others, rb.position);
        }





    }
    private void Jump()
    {
        if (jump && isGrounded())
        {
            rb.velocity += Vector3.up * jumpForce;

        }
        else
        {
            // rb.AddForce(Physics.gravity * rb.mass, ForceMode.Force);
        }
    }

    private void Sprint()
    {
        if (sprint && isSkillOk && !isSprinting)
        {
            Debug.LogError("sprint");
            StartCoroutine(SprintCoroutine());

        }

    }
    private IEnumerator SprintCoroutine()
    {
        float startTime = Time.time;
        float originVelocity = velocity;

        isSkillOk = false;
        while (Time.time < startTime + sprintTime)
        {

            isSprinting = true;
            velocity = sprintForce;


            yield return null;
            isSprinting = false;
            
            velocity = originVelocity;

        }
        StartCoroutine(CoolDown());
    }
    private IEnumerator CoolDown()
    {
        float startTime = Time.time;
        skillCoolDown.timeText.gameObject.SetActive(true);
       
        while (Time.time < startTime + cdTime)
        {

           
            skillCoolDown.timeText.text = ((startTime + cdTime) - Time.time).ToString("0");
            skillCoolDown.cdProgress.fillAmount = ((startTime + cdTime) - Time.time) / cdTime;


            yield return null;
        }
        skillCoolDown.timeText.gameObject.SetActive(false);
        isSkillOk = true;

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
    private void SyncAnime()
    {
        float ���� = Vector2.Distance(lastAnim, new Vector2(playerMoveInput.x, playerMoveInput.z));
        if (���� > 0.01f)
        {
            lastAnim = new Vector2(playerMoveInput.x, playerMoveInput.z);
            // �P�B
            photonView.RPC("SendAnim", RpcTarget.Others, lastAnim.x, lastAnim.y);
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
        if (isSprinting)
        {
            ani.SetBool("Sprint", true);
        }
        else
        {
            ani.SetBool("Sprint", false);
        }


    }
    bool isGrounded()
    {


        if (Physics.Raycast(characterCollider.bounds.center, Vector3.down, characterCollider.bounds.extents.y + raycastDistance))
        {

            rayColor = Color.red;

            return true;
        }
        else
        {
            rayColor = Color.white;

            return false;
        }



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
    private void OnDrawGizmos()
    {
        if (characterCollider != null)
        {
            Debug.DrawRay(characterCollider.bounds.center, Vector3.down * (characterCollider.bounds.extents.y + raycastDistance), rayColor);
        }
    }



    #region RPC Funtions

    [PunRPC]

    public void GetPos(Vector3 pos)
    {
        rpcPos = pos;
    }
    [PunRPC]
    public void SetCharacter(int cid)
    {
        characters[cid].SetActive(true);
        ani = characters[cid].GetComponent<Animator>();
        characterCollider = characters[cid].GetComponent<Collider>();
    }
    [PunRPC]
    public void SendAnim(float animX, float animZ)
    {
        ani.SetFloat("MoveX", animX);
        ani.SetFloat("MoveZ", animZ);
    }
    #endregion
}