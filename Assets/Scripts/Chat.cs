using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Chat : MonoBehaviourPunCallbacks
{
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput; 
    public PhotonView PV;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ChatInput.Select();
        }
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("FFFFFFFFFFFFFFFFFFFFFFF");
        RoomRenewal();
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
        //ChatText[0].text = PhotonNetwork.LocalPlayer.NickName + "접속";
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        if (PhotonNetwork.IsMasterClient)
        PV.RPC("ChatRPC", RpcTarget.All,"<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        //ChatText[0].text = "<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>";
    }  

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        if (PhotonNetwork.IsMasterClient)
        PV.RPC("ChatRPC", RpcTarget.All,"<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
        //ChatText[0].text = "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>";
    }
    void RoomRenewal()
    {
        ListText.text = PhotonNetwork.NickName;//" ";
        // for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        //     ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : "\n");
        RoomInfoText.text = "현재"+ PhotonNetwork.CurrentRoom.PlayerCount + "명 / 최대" + PhotonNetwork.CurrentRoom.MaxPlayers + "명";
    }

    ////////////////////////////////////////////////////////
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    
}