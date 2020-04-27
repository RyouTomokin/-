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
        void OnCardGet(List<int> cards);

        //该当前玩家进行操作 step_num当前阶段 [1，2，3，4，5]
        void OnTurnEnter(int step_num);

        //接收到其他玩家操作
        void OnOtherPlayerActionGet();
    }

}
