 using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using TMPro;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviourPunCallbacks, IPunObservable
    {
        public TMP_Text nickName;
        public Camera firstPersonCamera;
        private float turnspeed = 2f;
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform tr;
        private Vector3 m_Move;
        private Vector3 V3;
        private Vector3 forward;
        private Vector3 right;
        private bool m_crouch;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private Animator anim;
        private float curTime;
        public float coolTime = 0.3f;
        private void Start()
        {
            
            nickName.text = photonView.Owner.NickName;
            tr = GetComponent<Transform>();
            anim = GetComponent<Animator>();
            m_Character = GetComponent<ThirdPersonCharacter>();
            if (photonView.IsMine)
            {
                firstPersonCamera.enabled = true;
            }
            else firstPersonCamera.enabled = false;
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                if (!m_Jump)
                {
                    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                }
            }
            V3 = new Vector3 (0,Input.GetAxis("Mouse X"),0);
            transform.Rotate (V3*turnspeed);
            forward = transform.forward;
            right = transform.right;

            
            if (Input.GetKey(KeyCode.Q) || Input.GetMouseButtonDown(0)) //마우스 왼쪽 버튼 입력을 받으면 GetMouseButtonDown(0)
            { 
                anim.SetTrigger("Punching"); 
                Debug.Log("mouseclick");
            }
        
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!photonView.IsMine)
            {//위치전송값
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
                return;
            }
            
                // read inputs
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");
                if(v<0) v=0;
                bool crouch = Input.GetKey(KeyCode.C);

                m_Move = (v*tr.forward + h*tr.right);
                
                if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
                
                

                // pass all parameters to the character control script
                m_Character.Move(m_Move, crouch, m_Jump);
                m_Jump = false;
                
            
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
}
