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
            协议初始,
            开始投票,
            投票结果,
            行为,
            行为回复,
            行为结束,
            轮次,
            初始化完成,
            阶段开始消息,
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
                string s = PhotonNetwork.LocalPlayer.NickName.Split('|')[0] + "：" + data;
                PhotonNetwork.RaiseEvent(b, s, null, SendOptions.SendReliable);
                Net.GetInterface().OnChatMessageGet(s);
                return;
            }
              
            if (messageType == MessageType.发牌)
            {
                b = 2;
                PhotonNetwork.RaiseEvent(b, PhotonNetwork.LocalPlayer.NickName, null, SendOptions.SendReliable);
                return;
            }
            if (messageType == MessageType.行为)
            {
                b = 3;
                PhotonNetwork.RaiseEvent(b, PhotonNetwork.LocalPlayer.NickName + "@" + data, null, SendOptions.SendReliable);
                return;
            }

            if(messageType == MessageType.轮次)
            {
                b = 4;
                //tar_player_nickname + "@"  + step_num + "@" + status
                PhotonNetwork.RaiseEvent(b,tar_player_nickname + "@" + data, null, SendOptions.SendReliable);
                return;
            }

            if(messageType == MessageType.协议初始)
            {
                b = 5;
                PhotonNetwork.RaiseEvent(b, data, null, SendOptions.SendReliable);
                //直接调用回调
                string[] ss = ((string)data).Split('|');
                Net.GetInterface().OnAgreementInit(int.Parse(ss[0]), int.Parse(ss[1]));
                return;
            }
            if(messageType == MessageType.开始投票)
            {
                b = 6;
                PhotonNetwork.RaiseEvent(b, data, null, SendOptions.SendReliable);
                return;
            }
            if (messageType == MessageType.投票结果)
            {
                b = 7;
                PhotonNetwork.RaiseEvent(b, data, null, SendOptions.SendReliable);
                return;
            }
            if(messageType == MessageType.初始化完成)
            {
                if (IsHouseOwner()) return;
                b = 8;
                PhotonNetwork.RaiseEvent(b, PhotonNetwork.LocalPlayer.NickName, null, SendOptions.SendReliable);
                return;
            }
            if(messageType == MessageType.阶段开始消息)
            {
                if (!IsHouseOwner()) return;
                b = 9;
                PhotonNetwork.RaiseEvent(b, data, null, SendOptions.SendReliable);
                Net.GetInterface().OnStepStart(int.Parse(data));
                return;
            }
            if(messageType == MessageType.行为回复)
            {
                b = 10;
                PhotonNetwork.RaiseEvent(b, tar_player_nickname + "@" + data, null, SendOptions.SendReliable);
                return;
            }
            if(messageType == MessageType.行为结束)
            {
                if (IsHouseOwner())
                {
                    Net.GetInterface().OnActionEnd(PhotonNetwork.LocalPlayer.NickName);
                    return;
                }
                b = 11;
                PhotonNetwork.RaiseEvent(b, PhotonNetwork.LocalPlayer.NickName, null, SendOptions.SendReliable);
                return;
            }
           
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
                    Net.GetInterface().OnChatMessageGet((string)chat_s);
                }

            }
            if (photonEvent.Code == 2)
            {
                //发牌数据
                object chat_s;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收到卡牌数据：" + chat_s);
                    Net.GetInterface().OnCardGet();
                }
            }
             if(photonEvent.Code == 3)
            {
                //行为数据
                object chat_s;
                if(photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收到行为数据：" + chat_s);
                    string[] ss = ((string)chat_s).Split('@');
                    //if(ss[0] == PhotonNetwork.LocalPlayer.NickName)
                    Net.GetInterface().OnOtherPlayerActionGet((ActionData)JsonConvert.DeserializeObject(ss[1]));
                }
            }
            if(photonEvent.Code == 4)
            {
                object chat_s;
                if(photonEvent.Parameters.TryGetValue(245,out chat_s))
                {
                    Debug.Log("接收到轮次数据：" + chat_s);
                    string[] ss = ((string)chat_s).Split('@');
                    if (ss[0] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        if (ss[2] == "start")
                            Net.GetInterface().OnTurnEnter(int.Parse(ss[1]));
                    }
                       
                }
            }
            if(photonEvent.Code == 5)
            {
                object chat_s;
                if (IsHouseOwner()) return;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收协议初始消息：" + chat_s);
                    if (!IsHouseOwner())
                    {
                        string[] ss = ((string)chat_s).Split('|');
                        Net.GetInterface().OnAgreementInit(int.Parse(ss[0]), int.Parse(ss[1]));
                    }
                }
            }
            if(photonEvent.Code == 6)
            {
                object chat_s;
                if (IsHouseOwner()) return;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                   
                    if (!IsHouseOwner())
                    {
                        Debug.Log("接收投票开始消息：" + (string)chat_s);
                        string[] ss = ((string)chat_s).Split('@');
                        if(int.Parse(ss[0]) == 0)
                        {
                            //非额外一票
                            Net.GetInterface().OnVote((StepTwoActionData)JsonConvert.DeserializeObject((string)chat_s),false);
                        }
                        else
                        {
                            //额外一票
                            Net.GetInterface().OnVote((StepTwoActionData)JsonConvert.DeserializeObject((string)chat_s), true);
                        }
                       // Net.GetInterface().OnVote((StepTwoActionData)JsonConvert.DeserializeObject((string)chat_s));
                    }
                }
            }
            if(photonEvent.Code == 7)
            {
                object chat_s;
                //收到投票结果
                if (IsHouseOwner())
                {
       
                    if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                    {
                        Debug.Log("接收投票结果：" + (string)chat_s);
                        string[] ss = ((string)chat_s).Split('@');
                        if(int.Parse(ss[1]) == 0)
                        {
                            //不是额外一票
                            Net.GetInterface().OnVoteGet(float.Parse(ss[0]),false);
                        }
                        else
                        {
                            //额外一票
                            Net.GetInterface().OnVoteGet(float.Parse(ss[0]), true);
                        }
                    
                    }
                }
            }
            if (photonEvent.Code == 8)
            {
                if (!IsHouseOwner()) return;
                object chat_s;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收初始化完成消息：" + (string)chat_s);
                    Net.GetInterface().OnInitDone((string)chat_s);
                }
            }
            if(photonEvent.Code == 9)
            {
                object chat_s;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收阶段开始消息：" + (string)chat_s);
                    Net.GetInterface().OnStepStart(int.Parse((string)chat_s));
                }
            }
            if(photonEvent.Code == 10)
            {
                object chat_s;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收行为回复消息：" + (string)chat_s);
                    string[] ss = ((string)chat_s).Split('@');
                    if(ss[0] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        if (int.Parse(ss[1]) == 0)
                            Net.GetInterface().OnOtherPlayerActionAns(false);
                        else
                            Net.GetInterface().OnOtherPlayerActionAns(true);
                    }
                    
                }
            }
            if (photonEvent.Code == 10)
            {
                object chat_s;
                if (!IsHouseOwner()) return;
                if (photonEvent.Parameters.TryGetValue(245, out chat_s))
                {
                    Debug.Log("接收行为完成消息：" + (string)chat_s);
                    Net.GetInterface().OnActionEnd((string)chat_s);

                }
            }
            //foreach (var item in photonEvent.Parameters)
            //{
            //    Debug.Log(item.Key + " " + item.Value);
            //}
        }


        //判断是否是房主
        private bool IsHouseOwner()
        {
            Room room = PhotonNetwork.CurrentRoom;
            return room.masterClientId == PhotonNetwork.LocalPlayer.ActorNumber ? true : false;
        }
    }


}
