using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C;

namespace Tomokin
{
    public class TomokinNet :MonoBehaviour, NetInterface
    {
        public LoginManager loginManager;
        public RoomManager roomManager;
        public static List<string> PlayersInRoom;

        private void Awake()
        {
            Net.InitNetInterface(this);
        }
        public void OnCardGet(List<int> cards)
        {
        }

        public void OnEveryReady(bool isHouseOwner)
        {
            Debug.Log("全体就绪，准备进入游戏");
            if (PlayersInRoom.Count == 3)
                roomManager.GameStartInit(PlayersInRoom, isHouseOwner);
            else
            {
                Debug.Log("人数不对，进入游戏错误:" + PlayersInRoom.Count);
                
            }
            foreach (var item in PlayersInRoom)
            {
                Debug.Log(item);
            }

        }

        public void OnLogin()
        {
            Debug.Log("登录成功");
            loginManager.LoginSuccess();
        }

        public void OnLoginFail()
        {
            Debug.Log("登录失败");
        }

        public void OnMyPlayerJoinFail()
        {
            Debug.Log("未进入房间");
        }

        string OnlyName(string pn)
        {
            string n = "";
            foreach (var s in pn)
            {
                if (s != '|') n += s;
                else break;
            }
            return n;
        }

        public void OnMyPlayerJoinSuccess(List<string> player_nicknames)
        {
            Debug.Log("进入房间");
            loginManager.JoinSuccess();
            PlayersInRoom = new List<string>();
            foreach (string pn in player_nicknames)
            {
                roomManager.JoinRoom(OnlyName(pn));
            }
            roomManager.JoinRoom();
        }

        public void OnOtherPlayerActionGet()
        {
        }

        public void OnPlayerJoin(string nickname)
        {
            Debug.Log(nickname + "进入房间");
            
            roomManager.JoinRoom(OnlyName(nickname));
            //添加到文本框
        }

        public void OnPlayerLeave(string nickname)
        {
            Debug.Log(nickname + "退出了房间");
            roomManager.LeaveRoom(OnlyName(nickname));
            PlayersInRoom.Remove(nickname);
            //添加到文本框
        }

        public void OnRoomListGet(List<string> rooms)
        {
            loginManager.RoomListGet(rooms);
        }

        public void OnTurnEnter(int step_num)
        {
        }

        public void OnAgreementInit(int agreement_1, int agreement_2)
        {
            FindObjectOfType<BookManager>().Add_Card_To_Book(0, agreement_1);
            FindObjectOfType<BookManager>().Add_Card_To_Book(1, agreement_2);
            HouseOwner.ReadyInit();
            Net.InitDone();
        }

        public void OnVote(StepTwoActionData stepTwoActionData)
        {
            throw new System.NotImplementedException();
        }

        public void OnOtherPlayerActionGet(C.ActionData actionData)
        {
            throw new System.NotImplementedException();
        }

        public void OnCardGet()
        {
            throw new System.NotImplementedException();
        }

        public void OnVote(StepTwoActionData stepTwoActionData, bool isExtra)
        {
            throw new System.NotImplementedException();
        }

        public void OnVoteGet(float poll, bool isExtra)
        {
            throw new System.NotImplementedException();
        }

        public void OnChatMessageGet(string chat_message)
        {
            FindObjectOfType<TextInputManager>().RessiveMsg(chat_message);
        }

        public void OnInitDone(string done_nickname)
        {
            HouseOwner.ReadyAdd();
        }

        public void OnStepStart(int step_num)
        {
            GameManager.Stages = step_num;
        }

        public void OnOtherPlayerActionAns(bool b)
        {
            throw new System.NotImplementedException();
        }

        public void OnActionEnd(string end_nickname)
        {
            throw new System.NotImplementedException();
        }
    }
}