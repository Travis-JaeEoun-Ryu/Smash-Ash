 using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;
using Photon.Realtime;
using UnityStandardAssets.Utility;
using TMPro;
using UnityEngine.UI;
using System.Collections;


    //[RequireComponent(typeof (CharacterEngine))]
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        public TMP_Text nickName;
        public Camera firstPersonCamera;
        public Camera secondCamera;
        public GameObject canvas;
        public GameObject ExitButton;
        public Image[] img_Skill;
        public GameObject NameCanvas;
        
        private float turnspeed = 2f;
        public bool attacked = false;
        private bool esc = true;
        private Transform tr;
        private Rigidbody rg;
        private Vector3 m_Move;
        private Vector3 V3;
        private Vector3 forward;
        private Vector3 right;
        private bool[] skillActive = new bool[8];
        
        private Animator anim;
        private float curTime;
        EnemyController enemy;
        CapsuleCollider capsuleCollider;
        public AudioListener audioListener;
        //public float m_DoubleClickSecond = 0.25f;
        //private bool m_IsOneClick = false;
        //private double m_Timer = 0;
        private int bCount = 0;
        public bool isAttack = false;

        Weapon weapon;
        PlayerAttack playerAttack;
    //쿨타임

    public static PlayerController instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
        {
            //Ray ray = Camera.main.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0));
            nickName.text = photonView.Owner.NickName;
            tr = GetComponent<Transform>();
            rg = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            weapon = GetComponent<Weapon>();
            enemy = GetComponent<EnemyController>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            playerAttack = GetComponent<PlayerAttack>();

            for(int i=0; i<skillActive.Length; i++)
            {skillActive[i] = false;}    

            if (photonView.IsMine)
            {
                firstPersonCamera.enabled = true;
                secondCamera.enabled = false;
                canvas.SetActive(true);
                audioListener.enabled = true;
                NameCanvas.SetActive(false);
            }
            else 
            {
                firstPersonCamera.enabled = false;
                secondCamera.enabled = false;
                canvas.SetActive(false);
                audioListener.enabled = false;
                //CamPivot.SetActive(false);
            }
            
            
        }

        private void Update()
        {
            
            

            if (photonView.IsMine)
            {
            V3 = new Vector3 (0,Input.GetAxis("Mouse X"),0);
            transform.Rotate (V3*turnspeed);
            forward = transform.forward;
            right = transform.right;
            
            GameManager.instance.userPositionX = tr.position.x;
            GameManager.instance.userPositionY = tr.position.y;
            GameManager.instance.userPositionZ = tr.position.z;
            
            
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)//애니메이션 재생이 끝나면
            {
                if (Input.GetMouseButtonDown(0) )//&& isAttack == false
                {
                    
                    switch(bCount)
                    {
                        case 0:
                            //isAttack = true;
                            photonView.RPC("Slash1",RpcTarget.All);
                            bCount++;
                            Debug.Log("S1");
                            break;

                        case 1:
                            //isAttack = true;
                            photonView.RPC("Slash2",RpcTarget.All);
                            bCount--;
                            Debug.Log("S2");
                            break;

                    }
                }
               

                if(Input.GetKeyDown(KeyCode.E) && skillActive[1] == false)//num1
                {
                    photonView.RPC("JumpSlash",RpcTarget.All);
                    StartCoroutine(CoolTime (2f,1));
                    playerAttack.Heal();
                    //playerAttack.Damage *= 2f;
                }
                
                if(Input.GetKeyDown(KeyCode.R)&& skillActive[2] == false)
                {
                    photonView.RPC("SlashCombo",RpcTarget.All);
                    StartCoroutine(CoolTime (4f,2));
   
                }

                if(Input.GetKeyDown(KeyCode.F)&& skillActive[4] == false)
                {
                    photonView.RPC("Attack360",RpcTarget.All);
                    StartCoroutine(CoolTime (4f,4));    
                }

                
                if(Input.GetKeyDown(KeyCode.Alpha2)&& skillActive[5] == false)
                {
                    photonView.RPC("Spell",RpcTarget.All);
                    StartCoroutine(CoolTime (10f,5));        
                }
                if(Input.GetKeyDown(KeyCode.Alpha3)&& skillActive[6] == false)
                {
                    photonView.RPC("Spell1",RpcTarget.All);
                    StartCoroutine(CoolTime (15f,6));        
                }
                
                if(Input.GetKeyDown(KeyCode.G)&& skillActive[7] == false)
                {
                    photonView.RPC("Test",RpcTarget.All);  
                    StartCoroutine(CoolTime (10f,7));      
                }
            }
            
            
            
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (esc==true)
                {
                    Cursor.visible = true;//마우스 커서 보이게
                    Cursor.lockState = CursorLockMode.None;//마우스 커서 고정풀기
                    ExitButton.SetActive(true);
                    esc = false;
                }
                else
                {
                    Cursor.visible = false;//마우스 커서 안보이게
                    Cursor.lockState = CursorLockMode.Locked;//마우스 커서를 고정
                    ExitButton.SetActive(false);
                    esc = true;
                }
                
            }

            if(Input.GetKeyDown(KeyCode.T)&& skillActive[3] == false)
            {
                tr.Translate(Vector3.forward*5.0f);
                StartCoroutine(CoolTime (4f,3));               
            }
            if(Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("WalkForward",false);
                anim.SetBool("RunForward",false);       
            }


            }
            //////////////////////////////////////////
            
            

            
        
        }
        void FreezeVelocity()
        {
            rg.velocity = Vector3.zero;
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!photonView.IsMine)
            {//위치전송값
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
                tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
                
            }
            if (photonView.IsMine)
            {
                // read inputs
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                if(v<0) v=0;
                //FreezeVelocity();

            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("WalkForward",true);
                
                if(v<0) v=0;
                //tr.Translate(Vector3.forward*v*speed*Time.deltaTime); 
            }else anim.SetBool("WalkForward",false);

            if(Input.GetKey(KeyCode.Q) && skillActive[0] == false )//num0
                {
                    Debug.Log("Q");
                    photonView.RPC("Roll",RpcTarget.All);
                    StartCoroutine(CoolTime (2f,0));    
                }
            

            if (Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("WalkForward",false);
                anim.SetBool("RunForward",true);
                if(v<0) v=0;
                
                
            }
            else anim.SetBool("RunForward",false);



            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("LeftWalk",true);
                //tr.Translate(Vector3.right*h*speed*0.6f*Time.deltaTime);
                
            }else anim.SetBool("LeftWalk",false);


            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("RightWalk",true);
                //tr.Translate(Vector3.left*-h*speed*0.6f*Time.deltaTime);      
            }else anim.SetBool("RightWalk",false);
              
            
            if (Input.GetKey(KeyCode.S))
            {
                 anim.SetBool("WalkBackward",true);
            }else anim.SetBool("WalkBackward",false);

            if (Input.GetKey(KeyCode.V))
            {
                CameraBack();
            }else CameraFront();
            
                
           }   
    
        }

        IEnumerator CoolTime(float cool,int skill_num)
        {
            float cool_tmp = cool;
            skillActive[skill_num] = true;
            while (cool > 0f)
            {
                cool -= Time.deltaTime;
                img_Skill[skill_num].fillAmount = (cool/cool_tmp);
                yield return new WaitForFixedUpdate(); 
            }
            skillActive[skill_num] = false;
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
        [PunRPC]
        public void Slash1()
        {
            anim.SetTrigger("Slash1");
            
        }
        [PunRPC]
        public void Slash2()
        {
            anim.SetTrigger("Slash2");
            
        }
        [PunRPC]
        public void Attack360()
        {
            anim.SetTrigger("Attack360");
            StartCoroutine(attack360());
        }
        private IEnumerator attack360()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            yield return new WaitForSeconds(0.3f);
            for(int i = 0 ; i<20; i++)
            {
                tr.Translate(Vector3.forward*0.15f);
                yield return new WaitForSeconds(0.02f);
            }
            
        }

        [PunRPC]
        public void Roll()
        {
            anim.SetTrigger("Roll");
        }
        [PunRPC]
        public void SlashCombo()
        {
            anim.SetTrigger("SlashCombo");
            //weapon.Slash3();
        }
        [PunRPC]
        public void JumpSlash()
        {
            anim.SetTrigger("JumpSlash");
            //weapon.Slash2();
        }
        [PunRPC]
        public void Spell()
        {
            anim.SetTrigger("Spell");
        }
        [PunRPC]
        public void Spell1()
        {
            anim.SetTrigger("Spell1");
        }
        [PunRPC]
        public void Test()
        {
            anim.SetTrigger("Test");
        }

        private void CameraBack()
        {
            firstPersonCamera.enabled = false;
            secondCamera.enabled = true;
        }

        private void CameraFront()
        {
            firstPersonCamera.enabled = true;
            secondCamera.enabled = false;
        }

        void ColliderRe()
        {
            capsuleCollider.height = 1.8f;
            capsuleCollider.center = new Vector3(0,0.9f,0);
        }
        void ColliderRoll()
        {
            capsuleCollider.height = 0.9f;
            capsuleCollider.center = new Vector3(0 ,0.45f, 0);
        }


        void AttackFalse()
        {
            isAttack = false;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }

