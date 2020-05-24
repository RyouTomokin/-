using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using C;
using Peixi;

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
            //初始化玩家姓名
            //int x = CilentManager.PlayerNum;
            //for (int i = 0; i < 3; i++)
            //{
            //    Debug.Log(PlayersInRoom[x]);

            //    GameManager.Instance.Player_Name_Text[i].GetComponent<Text>().text = OnlyName(PlayersInRoom[x]);
            //    x = (x + 1) % 3;
            //}
            //Debug.Log(CilentManager.PlayerNum);

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

        public static string OnlyName(string pn)
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
                    //Debug.Log("AD = " + AD.action_owner_nickname + "||" + CilentManager.PlayerName);
                    FindObjectOfType<GameManager>().ReceivedBrideMsg(AD.action_owner_nickname);
                }
            }
            else if (step == 2) //收到提案
            {
                StepTwoActionData AD = (StepTwoActionData)actionData;

                Debug.Log("收到的hc_id = " + AD.hand_card + " bc_id = " + AD.agreement_card);
                ProposalManager.GetPropFromNet(AD.agreement_id, AD.hand_card, AD.agreement_card, AD.owner_nickname);
                Debug.Log("收到" + AD.owner_nickname + "的提案");
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
            //FindObjectOfType<TextInputManager>().SendMsg(end_nickname+"OnActionEnd");
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
            Debug.Log("收到房主发的提案");
            GameManager.Instance.ADtoProp(stepTwoActionData);
            FindObjectOfType<VoteState>().StartVoteRound();
        }

        public void OnVoteGet(float poll, bool isExtra)
        {
            Debug.Log("房主收到投票反馈");
            HouseOwner.ReciveVote(poll);
        }

        public void OnVoteEnd(bool isAgree, float agree, float disagree)
        {
            Debug.Log("玩家收到投票结果");
            FindObjectOfType<GameManager>().VoteEnd(agree, disagree);
        }

        public void OnSynchronizeAssets(string owner_nickname, int glod, int chip)
        {
            Debug.Log("Everyone Update:" + owner_nickname);
            foreach (var pd in CilentManager.PDs)
            {
                if (pd.PlayerName == owner_nickname)
                {
                    pd.SetChip = chip;
                    pd.SetMoney = glod;
                    //FindObjectOfType<TextInputManager>().SendMsg("同步"+owner_nickname+"的信息");
                    FindObjectOfType<GameManager>().UpdateUIMsg();
                }
            }
        }
    }
}