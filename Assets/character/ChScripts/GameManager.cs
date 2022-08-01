using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Auth;


//파이어베이스로 전송할 변수만 넣으세요.. 
public class GameManager : MonoBehaviour
{ 

    public static GameManager instance = null;

    private void Awake()
    { 
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때 
        { 
            instance = this; //내자신을 instance로 넣어줍니다. 
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지 
        } 
        else 
        { 
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미 
            Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제 
        } 
        
    }
//GameManager.instance.(변수 혹은 함수명)
   
    public string userName;
    public float userExp;
    public FirebaseUser user;
    //public float[] userPosition = new float[3];
    public float userPositionX;
    public float userPositionY;
    public float userPositionZ;
    public int userLevel;
    
    
    
////////////////////////////////////////////////////////////////
    

}
