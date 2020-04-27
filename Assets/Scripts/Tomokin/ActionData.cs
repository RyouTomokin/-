using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Tomokin
{
    /// <summary>
    /// 每个玩家的行为信息
    /// </summary>
    public class ActionData
    {
        private GameObject Bebribed;
        public List<CardData> Cards;
        public float VoteNumofProposal = 0;

        /// <summary>
        /// 被贿赂
        /// </summary>
        public GameObject GetBeBribed
        {
            set { Bebribed = value; }
            get { return Bebribed; }
        }

    }
}