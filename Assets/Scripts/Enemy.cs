using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;




public class Enemy : MonoBehaviourPunCallbacks//, IPunObservable
{
    public int maxHealth;
    public int curHealth;
    public int power;
    private Transform tr;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;
    PhotonView PV;
    public PlayerController playerController;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerController = GetComponent<PlayerController>();
        tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            {//위치전송값
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
                
            }
    }
    private void OnTriggerEnter(Collider other)
        {   
            if(other.tag == "Player")
            {   
                Debug.Log("HitTager"+power); 
                //curHealth -= power; 
                //Debug.Log("HIT!:"+curHealth);     
            }
        }
    

    public void AttackProcess()
    {
        
        Debug.Log("뙴!");
    }

    [PunRPC]
    public void takedamage(int damge)
    {
        curHealth -= damge;
        Debug.Log("HIT!:"+curHealth);   
    }

    private Vector3 currPos;
        private Quaternion currRot;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(tr.position);
                stream.SendNext(tr.rotation);  
            }
            else
            {
                currPos = (Vector3)stream.ReceiveNext();
                currRot = (Quaternion)stream.ReceiveNext();
                    

                //float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
                //rg.position += rg.velocity * lag;
            }
        }
}
