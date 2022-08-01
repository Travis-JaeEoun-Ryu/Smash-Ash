using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject Ui;


    public Inventory inven;


    bool resurrection = false;
    bool appear = false;

    //restart 창 자식오브젝트를 저장할 변수들
    Image back;
    Text gameover;
    Image town;
    Text town_t;
    Image menu;
    Text menu_t;

    bool resurPassive = false;
    float resurrectionCount = 0;

    //퀘스트창을 관리해줄 오브젝트
    public GameObject QuestWindow;

    

    void Awake()
    {
        
    }

    void Start()
    {
        if (inven.gameObject.activeSelf == false)
        {
            inven.gameObject.SetActive(true);
        }
    }


    //인벤토리 꺼주는 버튼
    public void OninventoryOut()
    {
        inven.gameObject.SetActive(false);
    }

    //Tab 누르면 인벤토리를 호출하는 함수
    public void InventoryKeycode()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inven.gameObject.activeSelf == false)
            {
                inven.gameObject.SetActive(true);
            }
            else
            {
                inven.gameObject.SetActive(false);
            }
        }
    }

    //화면상의 인벤토리를 불러올수있는 버튼
    public void OninventoryButton()
    {
        if (inven.gameObject.activeSelf == false)
        {
            inven.gameObject.SetActive(true);
        }
        else
        {
            inven.gameObject.SetActive(false);
        }
    }



  

    //ESC를 누르면 UI가 꺼짐
    public void canvascontrol()
    {
        if (Ui.gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Ui.gameObject.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Ui.gameObject.SetActive(true);
            }
        }
    }

    //V를 누르면 퀘스트창을 OnOff 할 수 있음
    public void QuestWindowOnOff()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (QuestWindow.gameObject.activeSelf == false)
            {
                QuestWindow.gameObject.SetActive(true);
            }
            else if(QuestWindow.gameObject.activeSelf == true)
            {
                QuestWindow.gameObject.SetActive(false);
            }
        }
    }

    //클릭시 퀘스트창 Off
    public void QuestWindowXbutton()
    {
        QuestWindow.gameObject.SetActive(false);
    }

    
    void Update()
    {

        //if (resurPassive == false)
        //{
        //    resurrectionCount += 1f * Time.deltaTime;
        //    if (resurrectionCount >= 1.5f)
        //    {
        //        resurPassive = true;
        //        Character.instance.CanMove = true;
        //        resurrectionCount = 0f;
        //    }
        //}

        canvascontrol();
        InventoryKeycode();
        QuestWindowOnOff();


    }
}
