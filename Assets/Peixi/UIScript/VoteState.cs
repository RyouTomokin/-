using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tomokin;

namespace Peixi
{
    /// <summary>
    /// 记录提案
    /// </summary>
    public struct Bill
    {
        public string name;
        public string action;
        public int card1;
        public int card2;
        public Bill(string m_name,string m_action,int m_card1,int m_card2)
        {
            name = m_name;
            action = m_action;
            card1 = m_card1;
            card2 = m_card2;
        }
    }
    /// <summary>
    /// 记录投票结果
    /// </summary>
    public struct Vote
    {
        public float negativeVote;
        public float positiveVote;
    }
    /// <summary>
    /// 服务器使用StartRound(List<Bill> m_playerBill)开启投票回合
    /// 服务器使用InvokeShowVoteResult(Vote m_vote)显示投票结果
    /// </summary>
    public class VoteState : RoundState
    {
        [SerializeField]
        GameObject voteFrame;
        [SerializeField]
        GameObject useExTicketFrame;
        [SerializeField]
        GameObject voteResultFrame;
        
        /// <summary>
        /// 投票开始
        /// </summary>
        public event Action onVoteRoundStart;
        /// <summary>
        /// 投票结束
        /// </summary>
        public event Action onVoteRoundEnd;
        /// <summary>
        /// 服务器接收投票结果
        /// </summary>
        public event Action<bool> onVoteSent;
        /// <summary>
        /// 服务器接收使用额外一票事件
        /// </summary>
        public event Action<float> onUseTicket;
        /// <summary>
        /// 服务器展示投票结果
        /// </summary>
        public event Action<Vote> onShowVoteResult;
        int voteRound = 1;
        List<Bill> playerBills = new List<Bill>();
        public List<Bill> PlayerBills
        {
            get { return playerBills; }
        }
        private void Start()
        {
            //gameObject.SetActive(false);

            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }
        protected override void OnRoundStart()
        {
            //base.OnRoundStart();
            //print("开始投票阶段");
            //StartVoteRound();
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束投票阶段，等待其他玩家");
            voteFrame.SetActive(false);
            voteResultFrame.SetActive(false);
            useExTicketFrame.SetActive(false);
        }
        /// <summary>
        /// 服务器开始投票回合
        /// </summary>
        /// <param name="m_playerBills">所有玩家的提案</param>
        public void RoundStartInvoke(List<Bill> m_playerBills)
        {
            playerBills = m_playerBills;
            StartVoteRound();
        }
        public void StartVoteRound()
        {
            voteFrame.SetActive(true);
            voteFrame.transform.Find("cancelButton").gameObject.SetActive(true);
            print("开始第" + voteRound + "轮投票");
            if (onVoteRoundStart != null)
            {
                onVoteRoundStart.Invoke();
            }
            else
            {
                Debug.LogWarning("onVoteRoundStart is empty");
            }

            Proposal prop = CilentManager.PropNeedVote;
            Debug.Log("len="+ CilentManager.playerdata.Bebribed.Count);
            foreach (var item in CilentManager.playerdata.Bebribed)
            {
                Debug.Log("name=" + prop.Player.PlayerName + " Cilent=" + CilentManager.PlayerName);
                if (item.PlayerName == prop.Player.PlayerName)
                {
                    voteFrame.transform.Find("cancelButton").gameObject.SetActive(false);
                }
            }
        }
        /// <summary>
        /// 结束投票回合
        /// </summary>
        public void EndVoteRound()
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
        public void InvokeVoteSent(bool m_vote)
        {
            //print("投下赞成票");
            if (onVoteSent != null)
            {
                onVoteSent.Invoke(m_vote);  //投普通票
                var pd = CilentManager.playerdata;
                if (pd.ExVote)
                {
                    useExTicketFrame.SetActive(true);
                }
                else
                {
                    onUseTicket.Invoke(0);  //没有额外票
                    Debug.Log("没有额外一票");
                }
                
            }      
        }
        public void InvokeUseTicket(bool m_useTicket)
        {
            float v;
            if (m_useTicket) v = 1.5f;
            else v = -1.5f;
            //print("使用额外一票");
            if (onUseTicket != null)
            {
                if (CilentManager.playerdata.ExVote)
                {
                    onUseTicket.Invoke(v);
                    CilentManager.playerdata.ExVote = false;
                }
                else
                {
                    Debug.Log("没有额外一票");
                    onUseTicket.Invoke(0);
                } 
            }
        }

        public void InvokeUnuseTicket()
        {
            Debug.Log("不使用额外一票");
            onUseTicket.Invoke(0);
        }
        /// <summary>
        /// 服务器显示投票结果
        /// </summary>
        /// <param name="m_vote"></param>
        public void InvokeShowVoteResult(Vote m_vote)
        {
            voteResultFrame.SetActive(true);
            if (onShowVoteResult != null)
            {
                onShowVoteResult.Invoke(m_vote);
            }
        }
        public void HideVoteResult()
        {
            voteResultFrame.SetActive(false);
        }
        public void InvokeVoteRoundEnd()
        {
            if (onVoteRoundEnd != null)
            {
                onVoteRoundEnd.Invoke();
            }
            else
            {
                Debug.LogWarning("onNotUseTicket is empty");
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
    }
}

