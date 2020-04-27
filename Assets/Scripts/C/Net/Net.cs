using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace C
{
    public static class Net
    {
        private static NetManager netManager;

        private static NetInterface netInterface;

        #region 初始化
        //NetManager初始化
        public static void InitNetManager(NetManager netManager)
        {
            Net.netManager = netManager;
        }

        //NetInterface初始化
        public static void InitNetInterface(NetInterface netInterface)
        {
            Net.netInterface = netInterface;
        }
        #endregion

        #region 不用管
        public static NetInterface GetInterface()
        {
            return netInterface;
        }
        #endregion

        #region 游戏开始前网络代码
        //登录
        public static void Login(string nickname)
        {
            netManager.Login(nickname); 
        }
        //得到当前房间列表
        public static void GetCurrentRoomList(string filter_str)
        {
            netManager.FreshRoomList(filter_str);
        }
        //进入房间
        public static void JoinRoom(string room)
        {
            netManager.CreateOrJoinRoom(room);
        }
        #endregion

        #region 游戏进行中网络代码

        //发牌
        public static void DealCards(string tar_player_nickname,List<int> cards)
        {
            netManager.SendResponse(NetManager.MessageType.发牌,tar_player_nickname ,JsonConvert.SerializeObject(cards));
        }

        //发送玩家行为
        public static void SendAction()
        {

        }

        #endregion

    }
}
