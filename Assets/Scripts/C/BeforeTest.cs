using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace C
{
    public class BeforeTest : MonoBehaviour, NetInterface
    {

        private void Awake()
        {
            Net.InitNetInterface(this);
        }


        public void OnLogin()
        {

        }

        public void OnLoginFail()
        {

        }

        public void OnMyPlayerJoinFail()
        {

        }

        public void OnMyPlayerJoinSuccess(List<string> player_nicknames)
        {

        }

        public void OnPlayerJoin(string nickname)
        {

        }

        public void OnPlayerLeave(string nickname)
        {

        }

        public void OnRoomListGet(List<string> rooms)
        {

        }

        public void OnEveryReady(bool isHouseOwner)
        {
            if (isHouseOwner)
            {
                //发牌
               // Net.DealCards("player1",new List<int>());
            }
            else
            {

            }
        }

        public void OnCardGet(List<int> cards)
        {
            //得到卡牌
        }

        public void OnTurnEnter(int step_num)
        {
           
        }


        //接收到其他玩家操作
        public void OnOtherPlayerActionGet(ActionData actionData)
        {

        }

        void NetInterface.OnLogin()
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnLoginFail()
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnRoomListGet(List<string> rooms)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnMyPlayerJoinSuccess(List<string> player_nicknames)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnMyPlayerJoinFail()
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnPlayerJoin(string nickname)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnPlayerLeave(string nickname)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnEveryReady(bool isHouseOwner)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnCardGet()
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnTurnEnter(int step_num)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnAgreementInit(int agreement_1, int agreement_2)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnVote(StepTwoActionData stepTwoActionData, bool isExtra)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnVoteGet(float poll, bool isExtra)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnOtherPlayerActionGet(ActionData actionData)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnChatMessageGet(string chat_message)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnInitDone(string done_nickname)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnStepStart(int step_num)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnOtherPlayerActionAns(string nick_name,bool b)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnActionEnd(string end_nickname)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnVoteEnd(bool isAgree, float agree, float disagree)
        {
            throw new System.NotImplementedException();
        }

        void NetInterface.OnSynchronizeAssets(string owner_nickname, int glod, int chip)
        {
            throw new System.NotImplementedException();
        }
    }
}