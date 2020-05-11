using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
//using WebSocketSharp;

namespace Peixi
{
    public class NegociateState : RoundState
    {
        [SerializeField]
        protected GameObject ticketFrame;
        [SerializeField]
        GameObject[] playerBills;
        [SerializeField]
        GameObject exTicketFrame;
        [SerializeField]
        GameObject Plane;
        /// <summary>
        /// 同意购买额外一票
        /// </summary>
        public event Action<bool> onAgreeBuyTicket;
        private void Start()
        {
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
            Utility.AcitveAllChildren(transform, false);
        }

        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            print("开始协商阶段");
            ticketFrame.SetActive(true);
            exTicketFrame.SetActive(true);
            Plane.SetActive(true);
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束协商阶段，等待其他玩家");
            ticketFrame.SetActive(false);
            foreach (GameObject item in playerBills)
            {
                item.SetActive(false);
            }
            //StartCoroutine(RoundInterval());
        }
        /// <summary>
        /// 服务器开始提案阶段
        /// </summary>
        /// <param name="m_playerBills"></param>
        public void StartRound(List<Bill> m_playerBills)
        {
            exTicketFrame.SetActive(true);
            Plane.SetActive(true);

            for (int i = 0; i < playerBills.Length; i++)
            {
                playerBills[i].SetActive(true);
                var player = playerBills[i].transform.Find("player").GetComponent<Text>();
                var content = playerBills[i].transform.Find("content").GetComponent<Text>();

                player.text = m_playerBills[i].name + "希望";
                if (m_playerBills[i].action == "Add")
                {
                    content.text = "增加卡牌" + m_playerBills[i].card1;
                }
                else if (m_playerBills[i].action == "Delete")
                {
                    content.text = "删除卡牌" + m_playerBills[i].card1;
                }
                else if (m_playerBills[i].action == "Replace")
                {
                    content.text = "以" + m_playerBills[i].card2 + "取代" + m_playerBills[i].card1;
                }
                else
                {
                    throw new Exception("提案的行动填写错误");
                }

            }
        }
        public void BuyAddtionalTicket(bool buyAdditonalTicket)
        {
            if (onAgreeBuyTicket != null)
            {
                onAgreeBuyTicket.Invoke(buyAdditonalTicket);
            }
            else
            {
                throw new Exception("onAgreeBuyTicket没有订阅者");
            }
        }
        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            //FindObjectOfType<VoteState>().RoundStartInvoke();
        }
    }
}

