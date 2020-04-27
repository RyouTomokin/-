using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Newtonsoft.Json;

namespace C
{
    public class NetManager : MonoBehaviourPunCallbacks,IOnEventCallback
    {

        public enum MessageType
        {
            聊天,//对应序号1
            发牌,
            行为,
        }

       // public ChatManager chatManager;

        private RoomOptions roomOptions;

        private TypedLobby typedLobby;

        private void Awake()
        {
            Net.InitNetManager(this);
        }

        private void Start()
        {
            roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 3;
            typedLobby = TypedLobby.Default;
            //PhotonNetwork.ConnectUsingSettings();
        }

        //登录
        public void Login(string nickname)
        {
            PhotonNetwork.NickName = nickname;
           // PhotonNetwork.ConnectToRegion()
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateOrJoinRoom(string room_name)
        {
            PhotonNetwork.JoinOrCreateRoom(room_name, roomOptions, typedLobby);
        }

        //成功加入房间
        public override void OnJoinedRoom()
        {
            // chatManager.CreateOrJoinRoomCallback(true,PhotonNetwork.CurrentRoom.PlayerCount);
            //构造当前房间中玩家昵称数据
            List<string> player_nicknames = new List<string>();
            foreach (var item in PhotonNetwork.PlayerListOthers)
            {
                player_nicknames.Add(item.NickName);
            }
            Net.GetInterface().OnMyPlayerJoinSuccess(player_nicknames);
            //检测房间是否满员
            Room room = PhotonNetwork.CurrentRoom;
            if (room.PlayerCount == room.MaxPlayers)
            {
                Net.GetInterface().OnEveryReady(room.masterClientId == PhotonNetwork.LocalPlayer.ActorNumber ? true : false);
            }
        }
        //失败
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Net.GetInterface().OnMyPlayerJoinFail();
          //  chatManager.CreateOrJoinRoomCallback(false,0);
        }
        //失败
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
           // chatManager.CreateOrJoinRoomCallback(false,0);
        }
        
        public override void OnConnectedToMaster()
        {
            Debug.Log("登录成功");
            Debug.Log("用户名称：" + PhotonNetwork.LocalPlayer.NickName);
           // chatManager.LoginCallback(PhotonNetwork.LocalPlayer.NickName);
            //加入默认大厅
            PhotonNetwork.JoinLobby();
    
            Net.GetInterface().OnLogin();
        }

        public void FreshRoomList(string filter_str)
        {
            PhotonNetwork.GetCustomRoomList(typedLobby, filter_str);
        }

     
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("匹配到的房间数量：" + roomList.Count);
            List<string> rooms = new List<string>();
            foreach (RoomInfo item in roomList)
            {
                rooms.Add(item.Name);
            }
            Net.GetInterface().OnRoomListGet(rooms);
           // chatManager.FreshRoomListCallback(rooms);
        }

        public void SendResponse(MessageType messageType,string tar_player_nickname,string data)
        {
            byte b = 0;
            if(messageType == MessageType.聊天)
            {
                b = 1;
                PhotonNetwork.RaiseEvent(b, PhotonNetwork.LocalPlayer.NickName + "@" + data, null, SendOptions.SendReliable);
                return;
            }
              
            if (messageType == MessageType.发牌)
            {
                b = 2;
                PhotonNetwork.RaiseEvent(b, PhotonNetwork.LocalPlayer.NickName + "@" + tar_player_nickname + "@" + data, null, SendOptions.SendReliable);
                return;
            }
                
            if (messageType == MessageType.行为)
                b = 3;
           
        }




        //当玩家进入房间
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Net.GetInterface().OnPlayerJoin(newPlayer.NickName);
            //检测房间是否满员
            Room room = PhotonNetwork.CurrentRoom;
            if (room.PlayerCount == room.MaxPlayers)
            {
                Net.GetInterface().OnEveryReady(room.masterClientId == PhotonNetwork.LocalPlayer.ActorNumber ? true : false);
            }
           // chatManager.ShowPlayerEnter(newPlayer.NickName.Split('|')[0],PhotonNetwork.CurrentRoom.PlayerCount);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            
            Net.GetInterface().OnPlayerLeave(otherPlayer.NickName);
           // chatManager.ShowPlayerLeft(otherPlayer.NickName.Split('|')[0], PhotonNetwork.CurrentRoom.PlayerCount);
        }

        

        //接收事件
        public void OnEvent(EventData photonEvent)
        {
            Debug.Log("===============消息===============");
            Debug.Log(photonEvent.Code);
            if(photonEvent.Code == 1)
            {
                //聊天数据
                object chat_s;
                if(photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                  //  chatManager.ShowChatMessage((string)chat_s);
                }

            }
            if (photonEvent.Code == 2)
            {
                //发牌数据
                object chat_s;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收到卡牌数据：" + chat_s);
                    string[] ss = ((string)chat_s).Split('@');
                    if(ss[1] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        Net.GetInterface().OnCardGet((List<int>)JsonConvert.DeserializeObject(ss[2]));
                    }
                }
            }
            //foreach (var item in photonEvent.Parameters)
            //{
            //    Debug.Log(item.Key + " " + item.Value);
            //}
        }
    }
}
