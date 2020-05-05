﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Peixi;

[CustomEditor(typeof(ControlConsole))]
public class ControlConsoleEditor : Editor
{
    bool showPrepareStateButton;
    bool showProposalStateButton;
    bool showNegociateStateButton;
    bool showVoteStateButton;
    bool showAccountStateButton;
    ControlConsole control;
    bool test;
    List<Bill> bills = new List<Bill>();
    List<PlayerData> datas = new List<PlayerData>();
    Vote result;

    public Func<string> testFunc = new Func<string>(ReceiveMessage);
    private void OnEnable()
    {
        control = (ControlConsole)target;
        TestVote();
        TestUpdatePlayerData();
    }
    public override void OnInspectorGUI()
    {
        showPrepareStateButton = EditorGUILayout.Foldout(showPrepareStateButton, "准备阶段", true);
        if (showPrepareStateButton)
        {
            if (GUILayout.Button("开始准备阶段"))
            {
                FindObjectOfType<PrepareStateEvent>().RoundStartInvoke();
            }
            if (GUILayout.Button("从服务器更新玩家信息"))
            {
                FindObjectOfType<PlayerInformation>().UpdatePlayerData(datas);
            }
            if (GUILayout.Button("在线玩家1发送悄悄话"))
            {
                PrepareStateEvent prepare = FindObjectOfType<PrepareStateEvent>();
                prepare.OnBribeMessageReceived("player1");
            }
            if (GUILayout.Button("收到在线玩家接受贿赂消息"))
            {
                FindObjectOfType<PrepareStateEvent>().InvokeBribeRequestResultReceived("player1", true);
            }
            if (GUILayout.Button("收到在线玩家拒绝贿赂消息"))
            {

            }
            if (GUILayout.Button("开始提案阶段"))
            {
                FindObjectOfType<ProposalStateEvent>().RoundStartInvoke();
            }
        }

       showProposalStateButton = EditorGUILayout.Foldout(showProposalStateButton, "提案阶段", true);
        if (EditorGUILayout.Foldout(showProposalStateButton, "提案阶段", true))
        {
            if (GUILayout.Button("开始提案阶段"))
            {
                FindObjectOfType<ProposalStateEvent>().RoundStartInvoke();
            }
        }

        showNegociateStateButton = EditorGUILayout.Foldout(showNegociateStateButton, "协商阶段", true);
        if (showNegociateStateButton)
        {
            if (GUILayout.Button("开始协商阶段"))
            {
                FindObjectOfType<NegociateState>().RoundStartInvoke();
            }
        }

        showVoteStateButton = EditorGUILayout.Foldout(showVoteStateButton, "投票阶段", true);
        if (showVoteStateButton)
        {
            if (GUILayout.Button("开始投票阶段"))
            {
                TestVote();
                FindObjectOfType<VoteState>().RoundStartInvoke(bills);
            }
            if (GUILayout.Button("显示投票结果"))
            {
                result.negativeVote = 2;
                result.positiveVote = 2.5f;
                FindObjectOfType<VoteState>().InvokeShowVoteResult(result);
            }
        }

        showAccountStateButton = EditorGUILayout.Foldout(showAccountStateButton, "结算阶段", true);
        if (showAccountStateButton)
        {
            if (GUILayout.Button("开始结算阶段"))
            {
               
            }
        }
        if (GUILayout.Button("测试Func"))
        {
            FindObjectOfType<Test>().bribe.Invoke();
        }
    }
    static string ReceiveMessage()
    {
        return "SS";
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
    void TestUpdatePlayerData()
    {
        PlayerData player1 = new PlayerData();
        player1.name = "player1";
        player1.chip = 5;
        player1.coin = 5;

        PlayerData player2 = new PlayerData();
        player2.name = "player2";
        player2.chip = 5;
        player2.coin = 5;

        PlayerData player3 = new PlayerData();
        player3.name = "player3";
        player3.chip = 0;
        player3.coin = 0;

        datas.Add(player3);
        datas.Add(player2);
        datas.Add(player1);
    }
}



