using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform closePotTrans = null;
    [SerializeField] Animator ani = null;
    [SerializeField] Rigidbody rb = null;
    [SerializeField] Collider characterCollider = null;
    [SerializeField] CookUI cookUI = null;
    [SerializeField] SkillCoolDown skillCoolDown;
    [SerializeField] Transform camLookAt = null;
    [SerializeField] PlayerIK playerIK;
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
    [SerializeField] float stunTime = 5f;
    [SerializeField] int currentConnections;
    [SerializeField] TextMesh floatingId;
    [SerializeField] SpriteRenderer nameSlot;
    [SerializeField] float R, G, B;
    float stunRemainTime = -1;
   
    

    public List<GameObject> characters;
    bool jump;
    bool sprint;
    public bool isSprinting;
    bool isStun;
    public int _characterId;

    bool isSkillOk = true;
    bool isGameBegin = false;

    public bool inCookerArea;
    bool isOpenCooker;
  
    FoodTeam teamValue;

    Vector3 nameSlotPos;

    float angle = 0;
    Color rayColor;

    Vector3 angles = Vector3.zero;

    Vector3 rpcPos = Vector3.zero;
    Vector3 lastPos = Vector3.zero;


    Vector2 lastAnim = Vector2.zero;




    private void Awake()
    {

    }


    private void Start()
    {




        Cursor.lockState = CursorLockMode.Locked;
        if (photonView.IsMine)
        {

            CrossHair.instance.gameObject.transform.position = this.gameObject.transform.forward;

            skillCoolDown = GameObject.FindObjectOfType<SkillCoolDown>();
            cookUI = CookUI.instance;
            cookUI.gameObject.SetActive(false);
            _characterId = SaveManager.instance.nowData.characterID;
            if (_characterId <= 4)
            {
                teamValue = FoodTeam.GOOD;
                R = 0.3764706f;
                G = 0.5686275f;
                B = 0.4470588f;
                nameSlotPos = new Vector3(0, 5.3f, 0);
                
            }
            else if (_characterId >= 5)
            {
                teamValue = FoodTeam.BAD;
                R = 0.9137255f;
                G = 0.282353f;
                B = 0.2745098f;
                nameSlotPos = new Vector3(0, 4.5f, 0);
            }

            photonView.RPC("SetCharacter", RpcTarget.All, _characterId);//SaveManager.instance.nowData.characterID
            photonView.RPC("SetPlayerName", RpcTarget.All, SaveManager.instance.nowData.playerName, R, G, B, nameSlotPos);
            
            _characterId = SaveManager.instance.nowData.characterID;
            Vcam = FindObjectOfType<CinemachineVirtualCamera>();
            playerIK = this.gameObject.GetComponentInChildren<PlayerIK>();
            camLookAt = this.gameObject.transform.Find("camLookAt");


            if (SaveManager.instance.nowData.characterID <= 4)// if team == green
            {
                camLookAt.transform.localPosition = new Vector3(0, 3.5f, 0);
            }
            else if (SaveManager.instance.nowData.characterID >= 5)// if team == red 
            {
                camLookAt.transform.localPosition = new Vector3(0, 3, 0);
            }


        }
        else if (!photonView.IsMine)
        {
            rb.isKinematic = true;
        }

    }

    private void ConnectionCheck()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (GameObject.FindWithTag("Player"))
            {
                currentConnections++;
            }
        }
        if (currentConnections == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            photonView.RPC("StartGameCountDown", RpcTarget.All);
        }
    }


    private IEnumerator GameBeginCountdown()
    {
        GameBeginCountDown.instance.countDownText.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time < startTime + 5)
        {
            GameBeginCountDown.instance.countDownText.SetText(((startTime + 5) - Time.time).ToString("0"));
            yield return null;
        }
        GameBeginCountDown.instance.countDownText.gameObject.SetActive(false);
        isGameBegin = true;
    }

    //PlayerInput;
    private void Update()
    {
        if (!isGameBegin && this.gameObject != null)
        {
            ConnectionCheck();
        }
        if (photonView.IsMine && isGameBegin)
        {
            InputMagnitude(isStun);
            Jump();
            Sprint();

            OpenCooker();



        }


    }




    void InputMagnitude(bool _stun)
    {
        if (!_stun)
        {
            playerMoveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (!isOpenCooker)
            {
                yaw = -Input.GetAxis("Mouse Y");
                pitch = Input.GetAxis("Mouse X");
            }

            jump = Input.GetKeyDown(KeyCode.Space) ? true : false;
            sprint = Input.GetKeyDown(KeyCode.LeftShift) ? true : false;
        }



    }

    private void OpenCooker()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCookerArea)
        {
            yaw = 0f;
            pitch = 0f;

            cookUI.gameObject.SetActive(!cookUI.gameObject.activeSelf);
            if (cookUI.gameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;

                isOpenCooker = true;
                cookUI._isCookerOpen = true;
                //CurrentStatus.instance.gameObject.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                isOpenCooker = false;
                cookUI._isCookerOpen = false;
                //CurrentStatus.instance.gameObject.SetActive(true);

            }


        }



    }
    private void CloseCooker()
    {
        cookUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isOpenCooker = false;
        cookUI._isCookerOpen = false;
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

        // 消極的同步座標 如果本尊移動位置就同步發送給鏡像
        float d = Vector3.Distance(rb.position, lastPos);
        if (d > distanceToSync)
        {
            photonView.RPC("GetPos", RpcTarget.Others, rb.position);
        }





    }
    /// <summary>
    ///  Jump
    /// </summary>
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


    /// <summary>
    /// Sprint
    /// </summary>
    private void Sprint()
    {
        if (sprint && isSkillOk && !isSprinting && moveDir != Vector3.zero)
        {

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
            playerIK.isIKActive = false;
            velocity = sprintForce;


            yield return null;
            isSprinting = false;
            playerIK.isIKActive = true;

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
        float 長度 = Vector2.Distance(lastAnim, new Vector2(playerMoveInput.x, playerMoveInput.z));
        if (長度 > 0.05f)
        {
            lastAnim = new Vector2(playerMoveInput.x, playerMoveInput.z);
            // 同步
            photonView.RPC("SendAnim", RpcTarget.Others, lastAnim.x, lastAnim.y);
        }
        if (isStun && stunRemainTime == stunTime)
        {
            SendStunAnim();

        }
        else if (!isStun && stunRemainTime == 0)
        {
            stunRemainTime = -1;
            SendStunAnim();

        }

    }
    private void SendStunAnim()
    {
        photonView.RPC("StunAnim", RpcTarget.Others, isStun);

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
        if (isStun)
        {
            ani.SetBool("Stun", true);
        }
        else
        {
            ani.SetBool("Stun", false);
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

        Quaternion camDir = Quaternion.Euler(0, camLookAt.transform.rotation.eulerAngles.y, 0);
        //playerPos.rotation = Quaternion.Lerp(playerPos.rotation, camDir, Time.fixedDeltaTime * 10f);

        //rb.MoveRotation(Quaternion.Lerp(rb.rotation, camDir, Time.fixedDeltaTime * 2f));

        rb.MoveRotation(camDir);
        camLookAt.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

    }


    #region CheckPot
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.tag == "Pot" && other != null && other.gameObject.GetComponent<Cooker>().cookerTeam.ToString() == teamValue.ToString())
            {

                //CurrentStatus.instance.gameObject.SetActive(true);
                //CurrentStatus.instance.statusText.text = "按 -E- 打開鍋子";
                closePotTrans = other.transform;

                cookUI.SetCookerUIBillboard(closePotTrans.position);

                inCookerArea = true;



            }
           
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.tag == "Pot")
            {

                CloseCooker();
                closePotTrans = null;
                inCookerArea = false;
                //CurrentStatus.instance.gameObject.SetActive(false);



            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        
    }
    #endregion

    #region ApplyStun
    private void OnCollisionEnter(Collision collision)
    {
        if (isSprinting && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("ApplyStun", RpcTarget.All);
        }



    }
    private IEnumerator StunCountDown()
    {
        isStun = true;
        yaw = 0f;
        pitch = 0f;
        float startTime = Time.time;
        playerMoveInput = Vector2.zero;


        while (Time.time < startTime + stunTime)
        {
            stunRemainTime = (startTime + stunTime) - Time.time;

            yield return null;


        }
        stunRemainTime = 0;
        isStun = false;







    }


    #endregion



    #region RPC Funtions

    [PunRPC]
    public void StartGameCountDown()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(GameBeginCountdown());
        }
    }

    [PunRPC]
    public void StunAnim(bool _stun)
    {
        ani.SetBool("Stun", _stun);
    }
    [PunRPC]
    public void ApplyStun()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(StunCountDown());

        }


    }


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
    public void SetPlayerName(string playerName, float r, float g, float b, Vector3 namePos)
    {
        nameSlot.transform.localPosition = namePos;
        floatingId.text = playerName;
        nameSlot.color = new Color(r, g, b, 1);
      

    }
    [PunRPC]
    public void SendAnim(float animX, float animZ)
    {
        ani.SetFloat("MoveX", animX);
        ani.SetFloat("MoveZ", animZ);
    }
    #endregion
}
