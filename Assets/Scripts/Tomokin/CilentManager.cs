using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using C;

namespace Tomokin
{
    //本玩家的基本信息
    public static class CilentManager
    {
        public static string PlayerName;
        public static string PlayerID;
        public static int PlayerNum;
        public static PlayerGameData[] PDs = new PlayerGameData[3];
        public static PlayerGameData playerdata;
    }

    public static class HouseOwner
    {
        private static int Turns;
        private static int Stages;
        private static List<Proposal> PropOfTurn = new List<Proposal>();
        private static int Ready;
        private static int StageDone;
        private static float vote;
        private static float agree;
        private static float disagree;

        public static void Init()
        {
            Turns = 0;
            Stages = 1;
            Net.StartStep(Stages);
            //开放前两个槽关闭后两个
            //实例化玩家
        }

        public static void ReadyInit() { Ready = 0; }
        public static void ReadyAdd()
        {
            Ready++;
            if (Ready == 2)
            {
                Init();
                Turns = 1;
            }
        }

        public static void StageAdd()
        {
            StageDone++;
            Debug.Log("StageDone = " + StageDone);
            if (StageDone == 3)
            {
                AddJieDuan();
                vote = 0;
                disagree = 0;
                agree = 0;
            }
        }

        public static void ReciveVote(float poll)
        {
            if (poll > 0) agree += poll;
            else if (poll < 0) disagree -= poll;
            else Debug.LogError("poll==0，有问题");
            vote += poll;
        }

        //投票结果上传到服务器
        public static float GetVote { get => vote; }
        public static float GetAgree { get => agree; }
        public static float GetDisagree { get => disagree; }

        public static Proposal GetPropInNet
        {
            set { PropOfTurn.Add(value); }
        }
        //协议书排序 
        public static List<Proposal> PropSort(List<Proposal> props)
        {
            Proposal temp;
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 3; j++)
                {
                    if (props[i].Player.GetScore > props[j].Player.GetScore)
                    {
                        temp = props[i];
                        props[i] = props[j];
                        props[j] = temp;
                    }
                }
            }
            return props;
        }

        public static int[] AgreementInit()
        {
            int[] c_id = new int[2];
            int max = GameManager.Instance.CardsInLibarary.Count - 1;
            c_id[0] = Random.Range(0, max);
            c_id[1] = Random.Range(0, max);

            return c_id;
        }

        private static void AddTurns()
        {
            Turns++;
            if (Turns == 4)
            {
                //锁定前两个协议书槽
                //开放后两个协议书槽
            }
            if (Turns == 7)
            {
                //结束游戏
            }
            //回合结束时，清除上回合的提案
            //回合结束时，标记上回合未使用拉拢
            //清除额外票
        }
        public static void AddJieDuan()
        {
            Stages++;
            if (Stages > 5)
            {
                Stages = 1;
                AddTurns();
            }
            StageDone = 0;           
            //单机测试
            //GameManager.Instance.StartStage();
            Net.StartStep(Stages);
        }
        //返回一个时间倒计时
        public static int TimerController()
        {
            return 0;
        }
    }
}