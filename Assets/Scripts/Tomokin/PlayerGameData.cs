using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using C;

namespace Tomokin
{
    /// <summary>
    /// 存储每个玩家的数据
    /// </summary>
    public class PlayerGameData
    {
        private int Money;
        private int Chip;
        public bool ExVote;
        public string PlayerName;
        public int Number;
        public bool IsHouseOwner;
        public List<PlayerGameData> Bebribed = new List<PlayerGameData>();

        public PlayerGameData(int M, int C)
        {
            Money = M;
            Chip = C;
        }

        public PlayerGameData(string pn, int num, bool isowner)
        {
            Money = 5; Chip = 2; ExVote = false;
            PlayerName = pn;
            Number = num;
            IsHouseOwner = isowner;
        }

        public int SetMoney { set => Money = value; }

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

        public int SetChip { set => Chip = value; }
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
        
        public float GetScore
        {
            get { return Money + (Chip * 1.5); }
        }

        public void TanChuang(string s)
        {
        }

        //如果是房主的话，控制时间

    }
    
}