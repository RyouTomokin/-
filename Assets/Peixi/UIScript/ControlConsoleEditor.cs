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
    private void OnEnable()
    {
        control = (ControlConsole)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("开始准备阶段"))
        {
            FindObjectOfType<PrepareStateEvent>().RoundStartInvoke();
        }
        if (GUILayout.Button("在线玩家向发送悄悄话"))
        {
            PrepareStateEvent prepare = FindObjectOfType<PrepareStateEvent>();
            prepare.OnBribeMessageReceived();
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
            FindObjectOfType<VoteState>().RoundStartInvoke();
        }
        if (GUILayout.Button("显示投票结果"))
        {
            FindObjectOfType<VoteState>().InvokeVoteResultReceived();
        }
    }
}
