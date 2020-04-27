using UnityEngine;
using UnityEditor;

namespace Tomokin
{
    /// <summary>
    /// 存储每个玩家的数据
    /// </summary>
    public class PlayerGameData
    {
        private ActionData AD;
        private int Money;
        private int Chip;
        private int Vote;
        private int ExVote;
        public string PlayerName;
        public int Number;
        public bool IsHouseOwner;

        public PlayerGameData(int M, int C, int V)
        {
            Money = M;
            Chip = C;
            Vote = V;
            AD = new ActionData();
        }

        public PlayerGameData(string pn, int num, bool isowner)
        {
            AD = new ActionData();
            Money = 5;Chip = 2;
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
                    TanChuang("钱袋");
            }
        }
        public int GetChip
        {
            get { return Chip; }
            set
            {
                if (Chip + value >= 0)
                    Chip += value;
                else
                    TanChuang("筹码");

            }
        }
        public int GetVote
        {
            get { return Vote; }
            set
            {
                if (Vote + value >= 0)
                    Vote += value;
                else
                    TanChuang("票");

            }
        }
        public int GetExVote
        {
            get { return ExVote; }
            set
            {
                if (ExVote + value >= 0)
                    ExVote += value;
                else
                    TanChuang("票");

            }
        }

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
        private static int HuiHe;
        private static int JieDuan;

        public static void Init()
        {
            HuiHe = 1;
            JieDuan = 1;
            //开放前两个槽关闭后两个
            //实例化玩家
        }

        private static void AddHuiHe()
        {
            HuiHe++;
            if (HuiHe == 4)
            {
                //锁定前两个协议书槽
                //开放后两个协议书槽
            }
            if (HuiHe == 7)
            {
                //结束游戏
            }
        }
        public static void AddJieDuan()
        {
            JieDuan++;
            if (JieDuan > 5)
            {
                JieDuan = 1;
                AddHuiHe();
                //判断每个玩家的一些状态，比如是否被拉拢，额外票等
            } 
        }
        //返回一个时间倒计时
        public static int TimerController()
        {
            return 0;
        }
    }
}