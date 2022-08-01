using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterFollow : MonoBehaviourPun
{
    public LayerMask whatIsTarget; // 공격 대상 레이어
    private LivingEntity0 targetEntity; // 추적할 대상
    private LivingEntity0 livingEntity;
    private LivingEntity0 targetPlayer;
    private NavMeshAgent nav;
    private Animator anim;
    private Transform tr;
    private EnemyHit enemyHit;
    bool isattack = false;
    private int findrange;
    public bool dead;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        enemyHit = GetComponent<EnemyHit>();
        StartCoroutine(UpdatePath());
        StartCoroutine("FindPlayer");

        findrange = 20;
        dead = false;
    }
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead)
            {
                
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator UpdatePath() 
    {
        // 살아있는 동안 무한 루프
        while (true)//!dead
        {   
            

            if (hasTarget)
            {
                
                StopCoroutine("FindPlayer");
                 anim.SetBool("Walk",true);
                // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
                nav.isStopped = false;
                nav.SetDestination(targetEntity.transform.position);
                Targetting();//범위안에 들어오면 공격시작
                if(Vector3.Distance(targetEntity.transform.position, tr.position) > 20f)
                {
                    nav.isStopped = true;
                    anim.SetBool("Walk",false);
                    findrange = 20;
                    //StartCoroutine(FindPlayer());
                    //Debug.Log("Stop following");
                }
                
            }
            else
            {
                // 추적 대상 없음 : AI 이동 중지
                nav.isStopped = true;
                anim.SetBool("Walk",false);

            }
            
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.5f); 
        }
    }
    private IEnumerator FindPlayer()
    {
        while(true){
            // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
            // 단, targetLayers에 해당하는 레이어를 가진 콜라이더만 가져오도록 필터링
            for (int i = 0; i < findrange; i++)
            {
                Collider[] colliders =
                Physics.OverlapSphere(transform.position, (float)i, whatIsTarget);
                //yield return new WaitForSeconds(0.25f);
                // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
                for (int j = 0; j < colliders.Length; j++)
                {
                    // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
                    livingEntity = colliders[j].GetComponent<LivingEntity0>();

                    // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        findrange = 3;
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;
                        //추적시작
                        nav.isStopped = false;
                        if (isattack==false)
                        {
                            anim.SetBool("Walk",true);
                        }
                        
                        //Debug.Log("Found");
                        // for문 루프 즉시 정지
                        break;
                    }     
                }
                
            }
            

            
            

            yield return new WaitForSeconds(0.5f);
        }
        
    }

    void Targetting()
    {//플레이어가 범위안에 들어오면 
        if(enemyHit.monsterNum == 0)
        {
            float targetRadius = 10f;
            float targetRange = 10f;

            RaycastHit[] rayHits = 
            Physics.SphereCastAll(transform.position, targetRadius, Vector3.forward, targetRange, whatIsTarget);
            if(rayHits.Length >0 && !isattack)
            {
                
                StartCoroutine("Attack");
            }
        }
        else
        {
            float targetRadius = 1f;
            float targetRange = 1f;

            RaycastHit[] rayHits = 
            Physics.SphereCastAll(transform.position, targetRadius, Vector3.forward, targetRange, whatIsTarget);
            if(rayHits.Length >0 && !isattack && !dead)
            {
                
                StartCoroutine(Attack());
                
            }

        }
        
        
        
    }

    IEnumerator Attack()
    {
        if(enemyHit.monsterNum == 0)
        {
            isattack = true;
            nav.isStopped = true;
            transform.LookAt(targetEntity.transform.position);
            anim.SetBool("Walk",false);
            anim.SetBool("Attack",true);
            if (PhotonNetwork.IsMasterClient){
                PhotonNetwork.Instantiate("Sphere",tr.position + new Vector3(0,1f,0.5f), tr.rotation);
            }
            

            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds(2f);
            
            isattack = false;
            nav.isStopped = false;
            anim.SetBool("Attack",false);
            anim.SetBool("Walk",true);
            yield return new WaitForSeconds(2f);
        }
        else
        {
            isattack = true;
        
            anim.SetBool("Walk",false);
            anim.SetBool("Attack",true);
    
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds(2f);
            
            isattack = false;
            anim.SetBool("Attack",false);
            anim.SetBool("Walk",true);
            yield return new WaitForSeconds(2f);


        }
        
    }
    public void StopAttack()
    {
        StopCoroutine("Attack");
        anim.SetBool("Attack",false);
    }

    
}
