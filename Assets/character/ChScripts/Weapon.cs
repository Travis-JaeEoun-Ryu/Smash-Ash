using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Weapon : MonoBehaviour
{
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    private Animator anim;
    PhotonView PV;
    PlayerController playerController;
    
    private void Start()
    {
        meleeArea = GetComponentInChildren<BoxCollider>();
        trailEffect = GetComponentInChildren<TrailRenderer>();
        playerController = GetComponent<PlayerController>();
        meleeArea.enabled = false;
        trailEffect.enabled = false;
    }
    private void Update()
    {
        
        

    }

    void WeaponActive()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;
    }

    void WeaponInactive()
    {
        meleeArea.enabled = false;
        trailEffect.enabled = false;
    }
    
    // void AttackTrue()
    // {
    //     // if(i==0)
    //     // {
    //     //     playerController.isAttack = true;
    //     //     i++;
    //     // }
    //     // else if (i==1)
    //     // {
    //     //     playerController.isAttack = false;
    //     //     i--;
    //     // }
    //     playerController.isAttack = true;
    // }

    // void AttackFalse()
    // {
    //     playerController.isAttack = false;
    // }
   
    
}
