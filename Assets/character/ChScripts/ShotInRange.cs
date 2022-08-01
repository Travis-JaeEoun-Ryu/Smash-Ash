using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ShotInRange : MonoBehaviourPun
{
    public float damage; // 공격력
    private float fireDistance = 70f; // 사정거리
    
    public Transform CameraPoint;
    private Transform tr;
    public GameObject hitEffect;
    public GameObject hitEffect1;
    public LayerMask whatIsTarget; // 공격 대상 레이어
    LivingEntity0 target;
    PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    void Update()
    {
        if (photonView.IsMine)
        damage = GameManager.instance.userLevel*1.2f + 25;
    }
    public void Shot() {
        if (photonView.IsMine) 
        {
            tr = CameraPoint;
            // tr.position = CameraPoint.position;
            // tr.forward = CameraPoint.forward;
            photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient,0);
        }
        
    }
    public void Shot1() {
        if (photonView.IsMine)
        {
            tr = CameraPoint;
            photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient,1);
        }
    }

    private void Rayhit() 
    {


    }

    // 호스트에서 실행되는, 실제 발사 처리
    [PunRPC]
    private void ShotProcessOnServer(int num) {
        
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        Vector3 hitnormal = Vector3.zero;
        
        
        
        if (Physics.Raycast(CameraPoint.position,
            CameraPoint.forward, out hit, fireDistance))
        {
            hitPosition = hit.point;
            hitnormal = hit.normal;
            if(num==0)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    target = hit.collider.GetComponent<LivingEntity0>();
                
                    if (target != null)
                    {
                        
                        target.OnDamage(damage, hit.point, hit.normal);
                        playerAttack.Get_exp(target);
                    }
                }
            }
            else if(num==1)
            {
                Collider[] colliders =
                Physics.OverlapSphere(hit.point, 15f, whatIsTarget);
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity0 target = colliders[i].GetComponent<LivingEntity0>();
                    if (target != null)
                    {
                        target.OnDamage(damage, hit.point, hit.normal);
                        playerAttack.Get_exp(target);
                    }
                }
            }   
            
        }
        else
        {
            
            hitPosition = CameraPoint.position +
                          CameraPoint.forward * fireDistance;
        }
        if (num==0)
        photonView.RPC("ShotEffectProcessOnClients", RpcTarget.All, hitPosition, hitnormal);
        else if (num==1)
        photonView.RPC("ShotEffect", RpcTarget.All, hitPosition, hitnormal);
    }

    [PunRPC]
    private void ShotEffectProcessOnClients(Vector3 hitPosition, Vector3 hitnormal) {
        Instantiate(hitEffect, hitPosition ,Quaternion.LookRotation(hitnormal));
    }
   

    [PunRPC]
    private void ShotEffect(Vector3 hitPosition, Vector3 hitnormal) {
        Instantiate(hitEffect1, hitPosition ,Quaternion.LookRotation(hitnormal));
    }
}

