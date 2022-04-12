using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float speed = 60f;
    private void Start()
    {
        Invoke("DetroySelf", 10f);
    }
    private void Update()
    {
        if (canMove)
            Move(Time.deltaTime);
    }
    int nowPoint = 0;
    void Move(float times)
    {
        // 計算這一幀移動距離
        float mspeed = times * speed;

        while (mspeed > 0f)
        {
            // 如果沒有下一個點就不動
            if (nowPoint < 0)
                return;
            // 先檢查離下一個點多遠
            float d = Vector3.Distance(this.transform.position, psss[nowPoint]);
            // 一幀移動的距離不可大於到下個點的距離 否則會移動過頭
            float dmove = mspeed;
            // 這一幀可以抵達這個點 所以下次執行要往下一個點移動
            bool needAdd = false;
            if (dmove > d)
            {
                dmove = d;
                needAdd = true;
            }
            this.transform.Translate((psss[nowPoint] - this.transform.position).normalized * dmove, Space.World);
            // 消耗移動距離
            mspeed -= dmove;
            if (needAdd)
                nowPoint--;
        }
    }

    private void DetroySelf()
    {

        Collider[] cols;
        cols = Physics.OverlapSphere(this.transform.position, 10f);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].transform.root.tag == "Player")
            {
                cols[i].transform.root.GetComponent<PlayerController>().ApplyStun();
            }
        }
        Instantiate(impactParticle, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
    List<Vector3> psss = new List<Vector3>();
    int shooterId = 0;
    bool canMove = false;
    public void LagMove(double lagTime, List<Vector3> poss, int shooterId)
    {
        this.shooterId = shooterId;
        this.psss = poss;
        nowPoint = psss.Count-1;
        //Debug.LogError(psss.Count);
        canMove = true;
        StartCoroutine(ILagMove(lagTime));
    }
    IEnumerator ILagMove(double lagTime)
    {
        yield return null;
        double time = lagTime / (double)10;
        for (int i = 0; i < 10; i++)
        {
            Move((float)time);
        }
    }
    [SerializeField] GameObject impactParticle = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetInstanceID() == shooterId)
            return;
        
        PlayerController pc = other.transform.root.GetComponent<PlayerController>();
        if (pc != null)
        {
            //TODO: fly away
        }

        DetroySelf();
    }

    
}
