﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler,IEndDragHandler
{
    public static Inventory instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public int gold = 0;
    public Text Gold;

    public List<InventorySlot> slotlist = new List<InventorySlot>();

    public Text MaxCount;
    int count = 0;


    void Start()
    {
        gameObject.SetActive(false);
    }


    //드래그시 받아오는 이미지를 저장할 이미지 오브젝트
    public Image DragImage;

    //드래그 활성화, 비활성화를 인식해줄 변수
    int DragActive = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        //인벤토리 슬롯마다 저장되어 있는 rect 값을 불러와 클릭지점이 rect의 위치와 동일하면 반응
        Vector3 MsPos = eventData.position;

        for (int i = 0; i <slotlist.Count; i++)
        {
            if (slotlist[i].rc.x <= MsPos.x && MsPos.x <= (slotlist[i].rc.x + slotlist[i].rc.width)
                && slotlist[i].rc.y >= MsPos.y && MsPos.y >= (slotlist[i].rc.y - slotlist[i].rc.height)
                && slotlist[i].Item.sprite != null) 
            {
                DragActive = i;
            }
        }
        if (DragActive != -1)
        {
            DragImage.gameObject.SetActive(true);
            DragImage.sprite = slotlist[DragActive].Item.sprite;
            DragImage.transform.position = MsPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector3 MsPos = eventData.position;

        //마우스버튼 업한 슬롯 칸의 정보를 불러온다.
        InventorySlot slot = slotlist.Find(o => (o.rc.x <= MsPos.x && MsPos.x <= (o.rc.x + o.rc.width)
            && o.rc.y >= MsPos.y && MsPos.y >= (o.rc.y - o.rc.height)));

        if(slot != null)
        {
            if(slot == slotlist[DragActive])
            {
                DragActive = -1;
                DragImage.sprite = null;
                DragImage.gameObject.SetActive(false);
            }
        }
        else if(slot == null)
        {
            
            DragActive = -1;
            DragImage.sprite = null;
            DragImage.gameObject.SetActive(false);
            
        }

    }
    

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 MsPos = eventData.position;
        DragImage.transform.position = MsPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 MsPos = eventData.position;
        for (int i = 0; i < slotlist.Count; i++)
        {
            if (slotlist[i].rc.x <= MsPos.x && MsPos.x <= (slotlist[i].rc.x + slotlist[i].rc.width)
                && slotlist[i].rc.y >= MsPos.y && MsPos.y >= (slotlist[i].rc.y - slotlist[i].rc.height))
            {
                if (slotlist[i].Item.sprite != null)
                {
                    Sprite iSpr;
                    iSpr = slotlist[i].Item.sprite;
                    int iCount;
                    iCount = slotlist[i].ItemCount;
                    itemInfo iInfo = slotlist[i].ItemInfo;

                    Sprite dSpr;
                    dSpr = slotlist[DragActive].Item.sprite;
                    int dCount;
                    dCount = slotlist[DragActive].ItemCount;
                    itemInfo dInfo = slotlist[DragActive].ItemInfo;

                    slotlist[i].Item.sprite = dSpr;
                    slotlist[i].ItemCount = dCount;
                    slotlist[i].ItemInfo = dInfo;

                    slotlist[DragActive].Item.sprite = iSpr;
                    slotlist[DragActive].ItemCount = iCount;
                    slotlist[DragActive].ItemInfo = iInfo;



                    DragImage.sprite = null;
                    DragImage.gameObject.SetActive(false);
                    DragActive = -1;

                }
                else if (slotlist[i].Item.sprite == null)
                {
                    slotlist[i].Item.gameObject.SetActive(true);

                    slotlist[i].Item.sprite = slotlist[DragActive].Item.sprite;
                    slotlist[i].ItemCount = slotlist[DragActive].ItemCount;
                    slotlist[i].ItemInfo = slotlist[DragActive].ItemInfo;

                    slotlist[DragActive].Item.sprite = null;
                    slotlist[DragActive].ItemCount = 0;

                    DragImage.sprite = null;
                    DragImage.gameObject.SetActive(false);
                    DragActive = -1;
                }
            }
        }
        
    }





    public void PickItem(itemInfo info)
    {
        InventorySlot pick = slotlist.Find(o => ( o.Item.sprite != null &&  o.Item.sprite.name.Equals(info.ItemIndex)));

        if (pick == null)
        {
            InventorySlot NewPick = slotlist.Find(o => (o.Item.sprite == null));

            NewPick.ItemInfo = info;
            NewPick.Item.sprite = ResourceManager.instance.GetIcon(info.ItemIndex);
            NewPick.ItemCount += 1;
            count += 1;
            
                
        }
        else if (pick != null)
        {
            pick.ItemCount += 1;
        }
    }

    void Update()
    {
        MaxCount.text = count.ToString() + "/42";
        Gold.text = gold.ToString() + "G";
    }
}
