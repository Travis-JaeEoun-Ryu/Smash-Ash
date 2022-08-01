using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class PhotonInit : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1.0";
    //public string userId = "Wans";
    public TMP_InputField txtUserID;
    public byte maxPlayer = 20;
    public GameObject ui;
    public GameObject loginimage;
    float posX;
    float posY;
    float posZ;
    // Start is called before the first frame update
    


    //몬스터 종류들을 저장해놓을 배열
    public GameObject[] Monsterarray;

    


    void Awake()
    {



    }

    void Start()
    {
        //firebaseManager = GetComponent<FirebaseManager>();
        posX = GameManager.instance.userPositionX;
        posY = GameManager.instance.userPositionY;
        posZ = GameManager.instance.userPositionZ;
        Monsterarray = Resources.LoadAll<GameObject>("Monsters/");
        Login();
    }
    public void Login()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = GameManager.instance.userName;//txtUserID.text;
        txtUserID.text = "";
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.ConnectUsingSettings();


    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect To Master");
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed Join Room !!!");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayer });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room !!!");
        Cursor.visible = false;//마우스 커서 안보이게
        Cursor.lockState = CursorLockMode.Locked;//마우스 커서를 고정
        ui.SetActive(false);//로그인 오브젝트 제거
        loginimage.SetActive(false);
        //PhotonNetwork.Instantiate("Player",new Vector3(0, 3.0f, 0), Quaternion.identity);
        CreatePlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnEnemy();
        }

    }

    void CreatePlayer()//
    {
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);

        int number = Random.Range(1, 2);
        switch (number)
        {
            case 1:
                PhotonNetwork.Instantiate("Player", new Vector3(posX,posY,posZ), Quaternion.identity);
                break;
            case 2:
                PhotonNetwork.Instantiate("ybot", points[idx].position, Quaternion.identity);
                break;
        }

    }
    void SpawnEnemy()
    {
        
        for (int i = 0; i < 5; i++)
        {
            float angle = UnityEngine.Random.Range(-180f, 180f);
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

            PhotonNetwork.Instantiate("Monsters/" + Monsterarray[i].gameObject.name,
            new Vector3(Random.Range(0, 20f), 0.1f, Random.Range(-20f, 20f)), rotation);
        }
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

