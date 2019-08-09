using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class Uihandler : MonoBehaviourPunCallbacks
{
    public InputField CreateRoomTF;
    public InputField JoinRoomTF;

    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomTF.text, null);
    }

    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateRoomTF.text, new RoomOptions {MaxPlayers =4}, null);
    }

   
    public override void OnJoinedRoom()
    {
        print("Room Joined Scuess");
        PhotonNetwork.LoadLevel(1);
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("RoomFailed"+returnCode+"Message"+message);
    }

   

}
