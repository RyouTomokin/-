using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 网络类回调接口
/// </summary>
namespace C
{
    public interface NetInterface
    {

        //登录回调 
        void OnLogin();

        //登录失败
        void OnLoginFail();

        //获取到房间列表 rooms为房间列表名称
        void OnRoomListGet(List<string> rooms);

        //当前玩家加入房间成功 players已在房间中的全部玩家
        void OnMyPlayerJoinSuccess(List<string> player_nicknames);

        //当前玩家加入房间失败
        void OnMyPlayerJoinFail();

        //当玩家加入房间
        void OnPlayerJoin(string nickname);

        //当玩家离开房间
        void OnPlayerLeave(string nickname);

        //玩家全部准备完毕后调用 isHouseOwner是否是房主
        void OnEveryReady(bool isHouseOwner);

        //接收到卡牌时调用
        void OnCardGet();

        //该当前玩家进行操作 step_num当前阶段 [1，2，3，4，5]
        void OnTurnEnter(int step_num);

        //协议初始化
        void OnAgreementInit(int agreement_1, int agreement_2);

        //初始化完成回调 只有房主会收到该回调
        void OnInitDone(string done_nickname);

        //阶段开始回调 客户端收到阶段开始消息
        void OnStepStart(int step_num);

        //开始投票 isExtra 是否是额外一票投票
        void OnVote(StepTwoActionData stepTwoActionData,bool isExtra);

        //收到投票 只有房主能得到回调
        void OnVoteGet(float poll,bool isExtra);

        //投票结束 回调
        void OnVoteEnd(bool isAgree, float agree, float disagree);

        //接收到其他玩家操作
        void OnOtherPlayerActionGet(ActionData actionData);

        //接收到其他玩家请求回复 b是否同意
        void OnOtherPlayerActionAns(string tar_nickname,bool b);

        //行动结束
        void OnActionEnd(string end_nickname);

        //资产同步消息回调
        void OnSynchronizeAssets(string owner_nickname, int glod, int chip);

        //聊天消息回调
        void OnChatMessageGet(string chat_message);
    }

}
