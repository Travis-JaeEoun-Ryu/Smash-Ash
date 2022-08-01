using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Sphere : MonoBehaviourPun
{
    private Transform tr;
    private float Damage = 20f;
    float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        StartCoroutine(delete());
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.forward*speed*Time.deltaTime);
    }

    IEnumerator delete()
    {
        yield return new WaitForSeconds(1.8f);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {   
        if(other.tag == "Player")
        {   
            Debug.Log("Playerhit");
  
            // 상대방으로부터 LivingEntity 타입을 가져오기 시 도

            LivingEntity0 target = other.GetComponent<LivingEntity0>();
            
            // 상대방으로 부터 LivingEntity 오브젝트를 가져오는데 성공했다면
            if(target != null)
            {
                Debug.Log("Get target");
                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;
                target.OnDamage(Damage, hitPoint, hitNormal);
                PhotonNetwork.Destroy(gameObject);    
                
            }                  
        }
    }
}
