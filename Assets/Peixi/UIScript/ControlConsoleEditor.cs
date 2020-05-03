using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Peixi;

[CustomEditor(typeof(ControlConsole))]
public class ControlConsoleEditor : Editor
{
    ControlConsole control;
    bool test;
    List<Bill> bills = new List<Bill>();
    Vote result;
    private void OnEnable()
    {
        control = (ControlConsole)target;
        TestVote();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("开始准备阶段"))
        {
            FindObjectOfType<PrepareStateEvent>().RoundStartInvoke();
        }
        if (GUILayout.Button("在线玩家1发送悄悄话"))
        {
            PrepareStateEvent prepare = FindObjectOfType<PrepareStateEvent>();
            prepare.OnBribeMessageReceived("player1");
        }
        if (GUILayout.Button("收到在线玩家接受贿赂消息"))
        {
            FindObjectOfType<PrepareStateEvent>().InvokeBribeRequestResultReceived(true);
        }
        if (GUILayout.Button("收到在线玩家拒绝贿赂消息"))
        {

        }
        if (GUILayout.Button("开始投票阶段"))
        {
            FindObjectOfType<VoteState>().StartRound(bills);
        }
        if (GUILayout.Button("显示投票结果"))
        {
            result.negativeVote = 2;
            result.positiveVote = 2.5f;
            FindObjectOfType<VoteState>().InvokeShowVoteResult(result);
        }
    }
    void TestVote()
    {
        Bill player1 = new Bill();
        player1.name = "Player1";
        player1.action = "Delete";
        player1.card1 = "Card001";
        Bill player2 = new Bill();
        player2.name = "Player2";
        player2.action = "Add";
        player2.card1 = "Card002";
        Bill player3 = new Bill();
        player3.name = "Player3";
        player3.action = "Replace";
        player3.card1 = "Card003";
        player3.card2 = "Card004";
        bills.Insert(0, player1);
        bills.Insert(1, player2);
        bills.Insert(2, player3);
    }
}



