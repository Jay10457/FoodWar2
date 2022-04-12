using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0f;
    private void Start()
    {
        Invoke("DetroySelf", 10f);
    }

    private void DetroySelf()
    {
        Instantiate(impactParticle, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
    private void Update()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
    int shooterId = 0;
    public void LagMove(double lagTime, int shooterId)
    {
        this.shooterId = shooterId;
        StartCoroutine(ILagMove(lagTime));
    }
    IEnumerator ILagMove(double lagTime)
    {
        yield return null;
        
        for(int i = 0; i < 5; i++)
        {
            double time = lagTime / (double)5;
            this.transform.Translate(Vector3.forward * (float)((double)speed * time), Space.Self);
            yield return new WaitForEndOfFrame();
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
            pc.TakeGunDamage();
        }

        DetroySelf();
    }
}
