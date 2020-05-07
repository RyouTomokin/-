using System;
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

    PrepareStateEvent prepare;
    ProposalStateEvent proposal;
    NegociateState negociate;
    VoteState vote;
    AccountState account;
    private void OnEnable()
    {
        control = (ControlConsole)target;
        TestVote();
        TestUpdatePlayerData();
        prepare = FindObjectOfType<PrepareStateEvent>();
        proposal = FindObjectOfType<ProposalStateEvent>();
        negociate = FindObjectOfType<NegociateState>();
        vote = FindObjectOfType<VoteState>();
        account = FindObjectOfType<AccountState>();
    }
    public override void OnInspectorGUI()
    {
        showPrepareStateButton = EditorGUILayout.Foldout(showPrepareStateButton, "准备阶段", true);
        if (showPrepareStateButton)
        {
            if (GUILayout.Button("开始准备阶段"))
            {
                prepare.RoundStartInvoke();
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
            if (GUILayout.Button("结束准备阶段"))
            {
                prepare.RoundEndInvoke();
            }
        }

       showProposalStateButton = EditorGUILayout.Foldout(showProposalStateButton, "提案阶段", true);
        if (showProposalStateButton)
        {
            if (GUILayout.Button("开始提案阶段"))
            {
                proposal.RoundStartInvoke();
            }
            if (GUILayout.Button("结束提案阶段"))
            {
                proposal.RoundEndInvoke();
            }
        }

        showNegociateStateButton = EditorGUILayout.Foldout(showNegociateStateButton, "协商阶段", true);
        if (showNegociateStateButton)
        {
            if (GUILayout.Button("开始协商阶段"))
            {
               negociate.StartRound(bills);
            }
            if (GUILayout.Button("结束协商阶段"))
            {
                negociate.RoundEndInvoke();
            }
        }

        showVoteStateButton = EditorGUILayout.Foldout(showVoteStateButton, "投票阶段", true);
        if (showVoteStateButton)
        {
            if (GUILayout.Button("开始投票阶段"))
            {
                TestVote();
                vote.RoundStartInvoke(bills);
            }
            if (GUILayout.Button("显示投票结果"))
            {
                result.negativeVote = 2;
                result.positiveVote = 2.5f;
                FindObjectOfType<VoteState>().InvokeShowVoteResult(result);
            }
            if (GUILayout.Button("结束投票阶段"))
            {
                vote.RoundEndInvoke();
            }
        }

        showAccountStateButton = EditorGUILayout.Foldout(showAccountStateButton, "结算阶段", true);
        if (showAccountStateButton)
        {
            if (GUILayout.Button("开始结算阶段"))
            {
                account.StartRound(TestScore());
            }
            if (GUILayout.Button("结束结算阶段"))
            {
                account.RoundEndInvoke();
            }
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
        player1.card1 = 1;
        Bill player2 = new Bill();
        player2.name = "Player2";
        player2.action = "Add";
        player2.card1 = 2;
        Bill player3 = new Bill();
        player3.name = "Player3";
        player3.action = "Replace";
        player3.card1 = 3;
        player3.card2 = 4;
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

    Score[] TestScore()
    {
        Score[] m_score = new Score[3];
        for (int i = 0; i < m_score.Length; i++)
        {
            m_score[i].chipGain = 1;
            m_score[i].GcoinGain = 2;
            m_score[i].name = "player";
            m_score[i].score = 15;
        }
        return m_score;
    }

}



