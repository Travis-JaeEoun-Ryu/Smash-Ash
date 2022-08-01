 using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using TMPro;
using UnityEngine.UI;



    //[RequireComponent(typeof (CharacterEngine))]
public class EnemyController : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform tr;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;
    PhotonView PV;
    
    private PlayerController playerController;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerController = GetComponent<PlayerController>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {
    }
    //부딪치면 밀려나지 않게
    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {//위치전송값
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);      
        }
        FreezeVelocity();
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
            }
        }
    }


