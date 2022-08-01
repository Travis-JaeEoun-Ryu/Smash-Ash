using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class MouseRotation : MonoBehaviourPunCallbacks, IPunObservable
{
    //public Transform target;   
    private Vector3 V3;
    float rotationX;
    float rotationY;
    private Transform tr;
     
    float turnspeed = 1f;
        
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }
        
        
            // Update is called once per frame
    void Update () 
    {     
        // rotationX = Input.GetAxis("Mouse Y");
        // rotationX = Mathf.Clamp(rotationX, -20.0f, 60.0f);
        // V3 = new Vector3 (rotationX, 0, 0);
        // transform.Rotate(-V3*turnspeed);
        
        if(photonView.IsMine){
        rotationY -= Input.GetAxis("Mouse Y") * turnspeed;    // +=로 하면 마우스 반전 상하로 바뀌게됨
        rotationY = Mathf.Clamp(rotationY, -20.0f, 60.0f);       //상하 범위 제한 시키기, 왜냐하면 위로 향하는데 360도로 돌기때문.
 
        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        }
        
            
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {//위치전송값
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
                
        }
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
