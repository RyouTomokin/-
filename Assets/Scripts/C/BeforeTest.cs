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
        public void OnOtherPlayerActionGet()
        {

        }
    }
}