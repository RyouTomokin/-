using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Tomokin
{
    /// <summary>
    /// 存储每个玩家的数据
    /// </summary>
    public class PlayerGameData
    {
        public ActionData AD;
        private int Money;
        private int Chip;
        public bool ExVote;
        public string PlayerName;
        public int Number;
        public bool IsHouseOwner;
        public PlayerGameData Bebribed;

        public PlayerGameData(int M, int C)
        {
            Money = M;
            Chip = C;
            AD = new ActionData();
        }

        public PlayerGameData(string pn, int num, bool isowner)
        {
            AD = new ActionData();
            Money = 5; Chip = 2; ExVote = false;
            PlayerName = pn;
            Number = num;
            IsHouseOwner = isowner;
        }

        public int GetMoney
        {
            get { return Money; }
            set
            {
                if (Money + value >= 0)
                    Money += value;
                else
                    TanChuang("钱袋不足");
            }
        }
        public bool IsMoneyEnough(int m)
        {
            if (Money - m >= 0) return true;
            else return false;
        }
        public int GetChip
        {
            get { return Chip; }
            set
            {
                if (Chip + value >= 0)
                    Chip += value;
                else
                    TanChuang("筹码不足");

            }
        }
        public bool IsChipEnough(int c)
        {
            if (Chip - c >= 0) return true;
            else return false;
        }

        //投票，判断被贿赂情况

        public double GetScore
        {
            get { return Money + (Chip * 1.5); }
        }

        public void TanChuang(string s)
        {
        }

        //如果是房主的话，控制时间

    }
    public static class HouseOwner
    {
        private static int Turns;
        private static int Stages;
        private static List<Proposal> PropOfTurn = new List<Proposal>();

        public static void Init()
        {
            Turns = 1;
            Stages = 1;
            //开放前两个槽关闭后两个
            //实例化玩家
        }

        public static Proposal GetPropInNet
        {
            set { PropOfTurn.Add(value); }
        }

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
        }
        //返回一个时间倒计时
        public static int TimerController()
        {
            return 0;
        }
    }
}