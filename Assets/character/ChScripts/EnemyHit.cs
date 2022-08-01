using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.AI;
public class EnemyHit : LivingEntity0, IPunObservable
{
    // 데미지 처리 및 추적루틴
    public LayerMask whatIsTarget; // 공격 대상 레이어
    private LivingEntity0 targetEntity; // 추적할 대상
    private LivingEntity0 livingEntity;
    private LivingEntity0 targetPlayer;
    private NavMeshAgent nav;
    private Animator anim;
    private Transform tr;
    public Image nowHpbar;
    public AudioClip hitSound;
    public AudioClip deadSound;
    private AudioSource AudioPlayer;
    public float Damage = 2;
    
    Vector3 enemyPosition;
    Vector3 hitposition;
   
    bool die=false;
    public GameObject hpbar;
    public GameObject[] Monsterarray;
    public int monsterNum;
    MonsterFollow monsterFollow;

    float Timer = 0f;



    void Start()
    {
       startingHealth = 300;//초기 체력
       monsterExp = 100;
       nav = GetComponent<NavMeshAgent>();
       anim = GetComponent<Animator>();
       AudioPlayer = GetComponent<AudioSource>();
        tr = GetComponent<Transform>();
        monsterFollow = GetComponent<MonsterFollow>();
        hpbar.SetActive(false);
        Monsterarray = Resources.LoadAll<GameObject>("Monsters/");
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        
        //StartCoroutine(UpdatePath());
        //StartCoroutine(FindPlayer());
    }
    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    // private bool hasTarget
    // {
    //     get
    //     {
    //         // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
    //         if (targetEntity != null && !targetEntity.dead)
    //         {
    //             return true;
    //         }

    //         // 그렇지 않다면 false
    //         return false;
    //     }
    // }
    // Update is called once per frame
    void Update()
    {
     
        MonsterRespawn();
       
    }
    void FixedUpdate()
    {
        if (dead && die == false)//!dead
        {
            enemyPosition = tr.position;
            die = true;
            
            StartCoroutine(Death());
            Debug.Log("Enemy Down!!");   
        }

    }
    

    // private IEnumerator UpdatePath() 
    // {
    //     // 살아있는 동안 무한 루프
    //     while (true)//!dead
    //     {   
            

    //         if (hasTarget)
    //         {
    //             StopCoroutine(FindPlayer());
    //             // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
    //             nav.isStopped = false;
    //             nav.SetDestination(targetEntity.transform.position);
    //             Targetting();//범위안에 들어오면 공격시작
    //             if(Vector3.Distance(targetEntity.transform.position, tr.position) > 20f)
    //             {
    //                 nav.isStopped = true;
    //                 anim.SetBool("Walk",false);
    //                 StartCoroutine(FindPlayer());
    //                 //Debug.Log("Stop following");
    //             }
                
    //         }
    //         else
    //         {
    //             // 추적 대상 없음 : AI 이동 중지
    //             nav.isStopped = true;

    //         }
            
    //         // 0.25초 주기로 처리 반복
    //         yield return new WaitForSeconds(0.5f); 
    //     }
    // }
    // private IEnumerator FindPlayer()
    // {
    //     while(true){
    //         // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
    //         // 단, targetLayers에 해당하는 레이어를 가진 콜라이더만 가져오도록 필터링
    //         Collider[] colliders =
    //             Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

    //         // 모든 콜라이더들을 순회하면서, 살아있는 플레이어를 찾기
    //         for (int i = 0; i < colliders.Length; i++)
    //         {
    //             // 콜라이더로부터 LivingEntity 컴포넌트 가져오기
    //             livingEntity = colliders[i].GetComponent<LivingEntity0>();

    //             // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
    //             if (livingEntity != null && !livingEntity.dead)
    //             {
    //                 // 추적 대상을 해당 LivingEntity로 설정
    //                 targetEntity = livingEntity;
    //                 //추적시작
    //                 nav.isStopped = false;
    //                 if (isattack==false)
    //                 {
    //                     anim.SetBool("Walk",true);
    //                 }
                    
    //                 //Debug.Log("Found");
    //                 // for문 루프 즉시 정지
    //                 break;
    //             }     
    //         }
            

    //         yield return new WaitForSeconds(0.5f);
    //     }
        
    // }

    // void Targetting()
    // {//플레이어가 범위안에 들어오면 
    //     float targetRadius = 1f;
    //     float targetRange = 1f;

    //     RaycastHit[] rayHits = 
    //         Physics.SphereCastAll(transform.position, targetRadius, Vector3.forward, targetRange, whatIsTarget);
        
    //     if(rayHits.Length >0 && !isattack)
    //     {
            
    //         StartCoroutine(Attack());
    //     }
    // }

    // IEnumerator Attack()
    // {
    //     isattack = true;
    //     //nav.speed = 0;
    //     anim.SetBool("Walk",false);
    //     anim.SetBool("Attack",true);
    //     //anim.SetTrigger("WalkAttack");
        
    //     yield return new WaitForSeconds(0.3f);
    //     //attckArea.enabled = true;

    //     yield return new WaitForSeconds(2f);
    //     //attckArea.enabled = false;
    //     isattack = false;
    //     //nav.speed = 3;
    //     anim.SetBool("Attack",false);
    //     anim.SetBool("Walk",true);
    //     yield return new WaitForSeconds(2f);
    // }

    void TargetOut()
    {
        
    }
    
    // 데미지를 입었을때 실행할 처리
    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        // 아직 사망하지 않은 경우에만 피격 효과 재생
        
        if (!dead)//죽지않았을 경우
        {
            //StartCoroutine(FindPlayer());
            AudioPlayer.PlayOneShot(hitSound); 
            // LivingEntity의 OnDamage()를 실행하여 데미지 적용
            base.OnDamage(damage, hitPoint, hitNormal);  

        }
        if(dead)
        {
            
        }
        //if(health<0) health = startingHealth;//테스트용 무한체력
        Debug.Log("Enemy Hit!,HP:"+ (health).ToString ("N3")+"DG:"+ damage);
        nowHpbar.fillAmount = health/startingHealth;
        hpbar.SetActive(true);  
    }


    IEnumerator Death()
        {//Death Motioon Active
            nav.isStopped = true;
            monsterFollow.StopAttack();
            nav.speed = 0;
            AudioPlayer.PlayOneShot(deadSound);
            anim.SetTrigger("Death");
            yield return new WaitForSeconds(0.02f);
            monsterExp = 0;
            //targetEntity.GetEXP(EXP);
            yield return new WaitForSeconds(1f);
            Quaternion itemRotation = Quaternion.Euler(-90f, 0f, 0f);
        ItemManager.instance.MakeItem("#0001", this.transform.position);
        ItemManager.instance.DropItem("#0001", "#0006", "#0006", "#0011", "#0011", this.transform.position);
        ItemManager.instance.DropItem("#0001", "#0011", "#0011", "#0006", "#0006", this.transform.position);

        yield return new WaitForSeconds(0.2f);
        }
    void MonsterRespawn()
    {
        if(die == true)
        {
            Timer += Time.deltaTime;
            //Debug.Log("Time:"+Timer);
            if(Timer >= 5f){

                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(gameObject);//파괴
                   
                    float angle = UnityEngine.Random.Range(-180f, 180f);
                    Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                    
                    PhotonNetwork.Instantiate("Monsters/" + Monsterarray[monsterNum].gameObject.name,
                    new Vector3(Random.Range(0, 20f), 0.1f, Random.Range(-20f, 20f)), rotation);
                }

            }
            else 
            {
                nav.isStopped = true;
                hpbar.SetActive(false);
            }
        }
    }
   
    
    //공격이 플레이어에게 닿으면 데미지 처리
    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {   
        if(other.tag == "Player")
        {   
            Debug.Log("Playerhit");
  
            // 상대방으로부터 LivingEntity 타입을 가져오기 시 도

            LivingEntity0 target = other.GetComponent<LivingEntity0>();
            targetPlayer = target;
            // 상대방으로 부터 LivingEntity 오브젝트를 가져오는데 성공했다면
            if(target != null)
            {
                Debug.Log("Get target");
                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;
                target.OnDamage(Damage, hitPoint, hitNormal);    
                
            }                  
        }
    }


    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(nowHpbar.fillAmount);              
        }
        else
        {
           nowHpbar.fillAmount = (float)stream.ReceiveNext();     
        }
    }
   
}

