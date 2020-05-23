using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Tomokin;
//using WebSocketSharp;

namespace Peixi
{
    public class NegociateState : RoundState
    {
        [SerializeField]
        protected GameObject ticketFrame;
        [SerializeField]
        GameObject[] playerBills;
        private List<GameObject> content_txt = new List<GameObject>();
        private List<GameObject> player_txt = new List<GameObject>();
        private List<GameObject> showcard_plane = new List<GameObject>();
        [SerializeField]
        GameObject exTicketFrame;
        [SerializeField]
        GameObject Plane;
        [SerializeField]
        GameObject HideBtn;
        [SerializeField]
        GameObject ShowBtn;
        /// <summary>
        /// 同意购买额外一票
        /// </summary>
        public event Action<bool> onAgreeBuyTicket;
        private void Start()
        {
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
            Utility.AcitveAllChildren(transform, false);

            foreach (var bill in playerBills)
            {
                content_txt.Add(bill.transform.Find("content").gameObject);
                player_txt.Add(bill.transform.Find("player").gameObject);
                showcard_plane.Add(bill.transform.Find("ShowPicture").gameObject);
            }
        }

        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            ticketFrame.SetActive(true);
            exTicketFrame.SetActive(true);
            HideBtn.SetActive(true);
            Plane.SetActive(true);
            ShowCardMsg();
        }

        private void ShowCardMsg()
        {
            List<Proposal> props = ProposalManager.PropsofthisTurn;
            int n = props.Count;
            if (n > 3) Debug.LogError("提案数量太多");
            Debug.Log("n = " + n);
            for (int i = 0; i < n; i++)
            {
                GameObject hc = showcard_plane[i].transform.Find("HandCard").gameObject;
                GameObject bc = showcard_plane[i].transform.Find("BookCard").gameObject;
                if (props[i].HandCard != null)
                {
                    hc.GetComponent<CardMsg>().card = props[i].HandCard;
                    hc.SetActive(true);
                    GameManager.InputCardMsg(hc);
                }
                else
                {
                    hc.SetActive(false);
                    content_txt[i].GetComponent<Text>().text = "删除此卡牌";
                }

                if (props[i].BookCard != null)
                {
                    bc.GetComponent<CardMsg>().card = props[i].BookCard;
                    bc.SetActive(true);
                    GameManager.InputCardMsg(bc);
                }
                else
                {
                    bc.SetActive(false);
                    content_txt[i].GetComponent<Text>().text = "添加此卡牌";
                }

                if (props[i].BookCard != null && props[i].HandCard != null)
                    content_txt[i].GetComponent<Text>().text = "用左边的卡牌替换右边的卡牌";

                string name = TomokinNet.OnlyName(props[i].Player.PlayerName);
                player_txt[i].GetComponent<Text>().text = name + "希望";
            }


        }

        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束协商阶段，等待其他玩家");
            ShowBtn.SetActive(false);
            ticketFrame.SetActive(false);
            //foreach (GameObject item in playerBills)
            //{
            //    item.SetActive(false);
            //}
            //StartCoroutine(RoundInterval());
        }
        /// <summary>
        /// 服务器开始提案阶段
        /// </summary>
        /// <param name="m_playerBills"></param>
        public void StartRound(List<Bill> m_playerBills)
        {
            exTicketFrame.SetActive(true);
            HideBtn.SetActive(true);
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

