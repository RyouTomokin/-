using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using C;

namespace Tomokin
{
    public class TomokinNet : MonoBehaviour, NetInterface
    {
        public LoginManager loginManager;
        public RoomManager roomManager;
        public static List<string> PlayersInRoom;   //Name+ID

        private void Awake()
        {
            Net.InitNetInterface(this);
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
            int x = CilentManager.PlayerNum;
            for (int i = 0; i < 3; i++)
            {
                Debug.Log(PlayersInRoom[x]);

                GameManager.Instance.Player_Name_Text[i].GetComponent<Text>().text = OnlyName(PlayersInRoom[x]);
                x = (x + 1) % 3;
            }
            Debug.Log(CilentManager.PlayerNum);

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
                PlayersInRoom.Add(pn);
            }
            roomManager.JoinRoom();
            PlayersInRoom.Add(CilentManager.PlayerName + CilentManager.PlayerID);
        }

        public void OnPlayerJoin(string nickname)
        {
            Debug.Log(nickname + "进入房间");

            roomManager.JoinRoom(OnlyName(nickname));
            PlayersInRoom.Add(nickname);
            //添加到文本框
        }

        public void OnPlayerLeave(string nickname)
        {
            Debug.Log(nickname + "退出了房间");
            roomManager.LeaveRoom(OnlyName(nickname));
            PlayersInRoom.Remove(nickname);
            //添加到文本框
            FindObjectOfType<TextInputManager>().SendMsg(OnlyName(nickname) + "退出了房间");
            //游戏暂停或停止
        }

        public void OnRoomListGet(List<string> rooms)
        {
            loginManager.RoomListGet(rooms);
        }


        public void OnAgreementInit(int agreement_1, int agreement_2)
        {
            FindObjectOfType<BookManager>().Add_Card_To_Book(0, agreement_1);
            FindObjectOfType<BookManager>().Add_Card_To_Book(1, agreement_2);
            HouseOwner.ReadyInit();
            Net.InitDone();
        }

        public void OnOtherPlayerActionAns(string tar_nickname, bool b)
        {
            FindObjectOfType<GameManager>().BribeSuccess(tar_nickname);
        }

        public void OnOtherPlayerActionGet(C.ActionData actionData)
        {
            Debug.Log("接收到" + actionData.action_owner_nickname + "的请求");
            int step = actionData.step_num;

            if (step == 1)  //收到贿赂请求
            {
                StepOneActionData AD = (StepOneActionData)actionData;
                Debug.Log(AD.tar_player_nickname);
                Debug.Log(CilentManager.PlayerName + CilentManager.PlayerID);
                if (AD.tar_player_nickname == CilentManager.PlayerName+CilentManager.PlayerID)
                {
                    //弹出贿赂请求窗口
                    Debug.Log("AD = " + AD.action_owner_nickname + "||" + CilentManager.PlayerName);
                    FindObjectOfType<GameManager>().ReceivedBrideMsg(AD.action_owner_nickname);
                }
            }
            else if (step == 2) //收到提案
            {
                StepTwoActionData AD = (StepTwoActionData)actionData;
                ProposalManager.GetPropFromNet(0, AD.hand_card, AD.agreement_id, AD.owner_nickname);
            }
            else if (step == 3)
            {
                StepThreeActionData AD = (StepThreeActionData)actionData;
            }
            else if (step == 4)
            {
                StepForthActionData AD = (StepForthActionData)actionData;
            }
            else
            {
                Debug.LogError("step is not true");
            }
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
            FindObjectOfType<GameManager>().StartStage();
        }
        public void OnActionEnd(string end_nickname)
        {
            Debug.Log(end_nickname + "回合结束");
            FindObjectOfType<TextInputManager>().SendMsg("OnActionEnd");
            //发送给房主
            HouseOwner.StageAdd();
        }

        public void OnCardGet()
        {
            throw new System.NotImplementedException();
        }

        public void OnTurnEnter(int step_num)
        {
            throw new System.NotImplementedException();
        }

        public void OnVote(StepTwoActionData stepTwoActionData, bool isExtra)
        {
            if (!isExtra)
            {
                //所以玩家得到此提案，并对它投票
            }
        }

        public void OnVoteGet(float poll, bool isExtra)
        {
            if (isExtra) HouseOwner.ReciveVote(poll);
        }

        public void OnVoteEnd(bool isAgree, float agree, float disagree)
        {
            FindObjectOfType<GameManager>().VoteEnd(agree, disagree);
        }

        public void OnSynchronizeAssets(string owner_nickname, int glod, int chip)
        {
            foreach (var pd in CilentManager.PDs)
            {
                if (pd.PlayerName == owner_nickname)
                {
                    pd.SetChip = chip;
                    pd.SetMoney = glod;
                    FindObjectOfType<GameManager>().UpdateUI();
                }
            }
        }
    }
}