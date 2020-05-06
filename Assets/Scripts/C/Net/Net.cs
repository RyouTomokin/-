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
        public static string Bool2String(bool b)
        {
            if (b)
                return "true";
            else
                return "false";
        }
        public static bool String2Bool(string s)
        {
            if (s == "true")
                return true;
            if (s == "false")
                return false;
            return false;
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
        public static void JoinRoom(string room)
        {
            netManager.CreateOrJoinRoom(room);
        }
        #endregion

        #region 玩家互动代码
        public static void SendChat(string chat_ss)
        {
            netManager.SendResponse(NetManager.MessageType.聊天, "", chat_ss);
        }
        #endregion

        #region 游戏进行中网络代码

        //发牌
        public static void DealCards()
        {
            netManager.SendResponse(NetManager.MessageType.发牌,"" ,"");
        }

        //协议初始化
        public static void InitAgreement(int agreement_1,int agreement_2)
        {
            netManager.SendResponse(NetManager.MessageType.协议初始, "", agreement_1.ToString() + "|" + agreement_2);
        }

        //初始化完成 客户端调用
        public static void InitDone()
        {
            netManager.SendResponse(NetManager.MessageType.初始化完成, "", "");
        }

        //开始某一阶段 由房主通知其余玩家
        public static void StartStep(int step_num)
        {
            netManager.SendResponse(NetManager.MessageType.阶段开始消息, "", step_num.ToString());
        }

        //发送 主机通知客户端 开始进行turn 第几阶段
        public static void SendTurnStartMessage(int step_num, string tar_nickname)
        {
            netManager.SendResponse(NetManager.MessageType.轮次, tar_nickname, step_num.ToString() + "@start");
        }


        //发送玩家行为
        public static void SendAction(ActionData actionData)
        {
            netManager.SendResponse(NetManager.MessageType.行为, "", JsonConvert.SerializeObject(actionData));
        }

        //发送玩家行为回复 tar_nickname回复目标昵称 0拒绝 1同意
        public static void SendActionAns(string tar_nickname,int isAgree)
        {
            netManager.SendResponse(NetManager.MessageType.行为回复, tar_nickname,isAgree.ToString() );
        }

        //action完毕
        public static void SendActionEndMessage()
        {
            netManager.SendResponse(NetManager.MessageType.行为结束, "", "end");
        }

        //主机通知客户端 开始进行投票  stepTwoActionData 提案数据  isExtra 0表示不是额外一票 1表示是额外一票
        public static void StartVote(StepTwoActionData stepTwoActionData,int isExtra)
        {
            netManager.SendResponse(NetManager.MessageType.开始投票, "",isExtra.ToString() + "@" + JsonConvert.SerializeObject(stepTwoActionData));
        }

        //客户端返回 投票结果     isExtra 0表示不是额外一票 1表示是额外一票
        public static void ClientReturnVoteAns(float poll,int isExtra)
        {                               
            netManager.SendResponse(NetManager.MessageType.投票结果, "", poll.ToString() + "@" + isExtra.ToString());
        }

        //房主调用  投票结束消息 每个提案投票结束后调用
        public static void EndVote(bool isAgree,float agree,float disagree)
        {
            netManager.SendResponse(NetManager.MessageType.投票结束, "", Bool2String(isAgree) + "@" + agree.ToString() + "@" + disagree.ToString());
            Net.GetInterface().OnVoteEnd(isAgree, agree, disagree);
        }

        //同步自身资产 gold 钱袋数 chip 筹码数
        public static void SynchronizeAssets(int gold,int chip)
        {
            netManager.SendResponse(NetManager.MessageType.资产同步, "", gold.ToString() + "@" + chip.ToString());
        }

        

        #endregion


    }
}
