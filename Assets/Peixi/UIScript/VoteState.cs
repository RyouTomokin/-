using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public struct Bill
    {
        public string name;
        public string action;
        public string card1;
        public string card2;
    }
    public class VoteState : RoundState
    {
        [SerializeField]
        protected GameObject voteFrame;
        public event Action<List<Bill>> onVoteRoundStart;
        public event Action onVoteRoundEnd;
        /// <summary>
        /// 投下赞成票事件
        /// </summary>
        public event Action onAgreeVoteSent;
        /// <summary>
        /// 投下反对票事件
        /// </summary>
        public event Action onDisagreeVoteSent;
        /// <summary>
        /// 使用额外一票事件
        /// </summary>
        public event Action onUseTicket;
        /// <summary>
        /// 从服务器接收投票结果
        /// </summary>
        public event Action onVoteResultReceived;
        int voteRound = 1;
        public List<Bill> bills = new List<Bill>();
        private void Start()
        {
            //onVoteRoundEnd += EndVoteRound;
            //onVoteRoundStart += StartVoteRound;
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
            //test code
            TestVote();
            //RoundStartInvoke();
        }
        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            print("开始投票阶段");
            StartVoteRound();
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束投票阶段，等待其他玩家");
            onVoteRoundEnd.Invoke();
            StartCoroutine(RoundInterval());
        }
        void StartVoteRound()
        {
            voteFrame.SetActive(true);
            print("开始第" + voteRound + "轮投票");
            if (onVoteRoundStart != null)
            {
                onVoteRoundStart.Invoke(bills);
            }
            else
            {
                Debug.LogWarning("onVoteRoundStart is empty");
            }
        }
        void EndVoteRound()
        {
            voteFrame.SetActive(false);
            voteRound += 1;
            if (voteRound > 3)
            {
                OnRoundEnd();
            }
            else
            {
                StartCoroutine(VoteInterval());
            }
        }
        public void InvokeAgreeProposal()
        {
            print("投下赞成票");
            if (onAgreeVoteSent != null)
            {
                onAgreeVoteSent.Invoke();
            }      
        }
        public void InvokeDisagreeProposal()
        {
            print("投下反对票");
            if (onDisagreeVoteSent != null)
            {
                onDisagreeVoteSent.Invoke();
            }
        }
        public void InvokeUseTicket()
        {
            print("使用额外一票");
            if (onUseTicket != null)
            {
                onUseTicket.Invoke();
            }
        }
        public void InvokeVoteResultReceived()
        {
            if (onVoteResultReceived != null)
            {
                onVoteResultReceived.Invoke();
            }
        }
        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            FindObjectOfType<AccountState>().RoundStartInvoke();
        }
        IEnumerator VoteInterval()
        {
            yield return new WaitForSeconds(1.5f);
            StartVoteRound();
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
}

