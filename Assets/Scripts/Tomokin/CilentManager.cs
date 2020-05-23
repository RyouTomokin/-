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
        public static Proposal PropNeedVote;
    }

    public static class HouseOwner
    {
        private static int Turns;
        private static int Stages;
        private static List<Proposal> PropOfTurn = new List<Proposal>();
        private static int Ready;
        private static int StageDone;
        private static int VoteOver;
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
                StageDone = 0;
                AddJieDuan();
                vote = 0;
                disagree = 0;
                agree = 0;
            }
        }

        public static void InitVote()
        { 
            vote = 0;
            VoteOver = 0;
            disagree = 0;
            agree = 0;
        }
        /// <summary>
        /// 收到投票
        /// </summary>
        /// <param name="poll">票数</param>
        public static void ReciveVote(float poll)
        {
            Debug.Log("Get投票" + poll + "  VoteOver=" + VoteOver);
            if (poll > 0) agree += poll;
            else if (poll < 0) disagree -= poll;
            else Debug.LogError("poll==0，有问题");

            if (Mathf.Abs(poll) == 0.5f)
            {
                agree++;
                disagree++;
            } 
            vote += poll;
            Debug.Log("agree = " + agree + " disagree = " + disagree);
            VoteOver++;
            
            if (VoteOver >= 3)
            {
                GameManager.Instance.SendVoteResult();
            }
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
            int n = props.Count;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (props[i].Player == null || props[j].Player == null) continue;
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
            Debug.Log("Turns = "+ Turns);
            if (Turns == 3)
            {
                Debug.Log("游戏结束");
            }

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
            
            //单机测试
            //GameManager.Instance.StartStage();
            Debug.Log("stages = " + Stages);
            Net.StartStep(Stages);
        }
        //返回一个时间倒计时
        public static int TimerController()
        {
            return 0;
        }
    }
}