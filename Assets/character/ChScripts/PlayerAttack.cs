using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class PlayerAttack : LivingEntity0
{
    public float Damage ;//기본데미지
    public AudioClip hitSound;
    public Image Hpbar;
    public Image Xpbar;
    public Text Exper;
    private AudioSource AudioPlayer;
    public GameObject hitEffect;
    private Vector3 v3;
    private int level;
    private bool Dead=false;
    private Animator anim;
    private Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        startingHealth = 200;//초기 체력
        Exp = GameManager.instance.userExp;
        level = GameManager.instance.userLevel;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
        Damage = GameManager.instance.userLevel*1f + 15;
        }
        Exper.text = "Lv." + level.ToString();
        //Debug.Log(Exp);
        GameManager.instance.userExp = Exp;
        GameManager.instance.userLevel = level;
        if(Exp>=1000) 
        {
            Exp = 0;
            level += 1;
        }
        Xpbar.fillAmount = Exp/1000;
        Hpbar.fillAmount = (health/startingHealth); 
        if(dead1 == true && Dead == false)
        {
            anim.SetTrigger("newDead");
            Debug.Log("player_Dead");

            Dead = true;
            StartCoroutine(Death());
        }
        
    }
    void FixedUpdate()
    {
        
    }

    IEnumerator Death()
    {

        yield return new WaitForSeconds(2f);
        dead = false;
        dead1 = false;
        Dead = false;
        health = startingHealth;
        
        anim.SetTrigger("Alive");
        yield return new WaitForSeconds(0.5f);
        tr.position = new Vector3(0,0,0);
        Debug.Log("코루틱 시작");


    }
    

    // 데미지를 입었을때 실행할 처리
    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        // 아직 사망하지 않은 경우에만 피격 효과 재생
        if (dead)//!dead
        {
            
        }

        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
        
        //피통처리
        //Debug.Log("Player Hit!,HP:"+ (health/startingHealth).ToString ("N3")+"DG:"+ damage);
        Hpbar.fillAmount = (health/startingHealth);    
    }

    
    //검이 상대에게 닿으면 데미지 처리
    private void OnTriggerEnter(Collider other)
    {   
        if(other.tag == "Enemy")
        {   
            
            // 상대방으로부터 LivingEntity 타입을 가져오기 시도
            LivingEntity0 target = other.GetComponent<LivingEntity0>();
            // 상대방으로 부터 LivingEntity 오브젝트를 가져오는데 성공했다면
            if(target != null)
            {
                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;
                target.OnDamage(Damage, hitPoint, hitNormal);
                
               
                Get_exp(target);
                RandomScale();
                Instantiate(hitEffect, hitPoint+new Vector3(-0.12f,1f,0)+v3 ,Quaternion.LookRotation(hitNormal)); 
                
            }
            
                     
        }
    }
    
    
    public void Get_exp(LivingEntity0 target)
    {
        if(target.dead == true)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Exp += target.monsterExp;
                photonView.RPC("GetExp", RpcTarget.Others, target.monsterExp);
            }
        }
    }
    [PunRPC]
    public void GetExp(float exp) {
        Exp += exp; 
        
    }

    void RandomScale()
    {
        int number = Random.Range(-3, 5);
        float scale = (0.1f * number); 
        v3 = new Vector3(0,scale,scale);
    }

    public void Heal()
    {
        health += 20f;
        if (health >= startingHealth)
        {
            health = startingHealth;
        }
        Hpbar.fillAmount = (health/startingHealth);
    }

    ///////////////////////////////////////////////////////////////////////////////
    
    
}
