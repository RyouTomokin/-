using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Peixi
{
    public class ProposalStateEvent : RoundState
    {
        PlayableDirector director;
        /// <summary>
        /// 提交提案
        /// </summary>
        public Action<Bill> submitBill;

        private void Start()
        {
            director = GetComponent<PlayableDirector>();
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }
        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            print("开始提案阶段");
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束提案阶段，等待其他玩家");
            director.Play();
            //StartCoroutine(RoundInterval());
        }
        public void DeleteBill(int m_card,string m_playerName)
        {
            Bill bill = new Bill();
            bill.card1 = m_card;
            bill.action = "delete";
            bill.name = m_playerName;
            if (submitBill != null)
            {
                submitBill.Invoke(bill);
            }
            else
            {
                throw new Exception("submitBill没有订阅者");
            }
        }
        public void AddBill(int m_card,string m_playerName)
        {
            Bill bill = new Bill();
            bill.card1 = m_card;
            bill.action = "add";
            bill.name = m_playerName;
            if (submitBill!=null)
            {
                submitBill.Invoke(bill);
            }
            else
            {
                throw new Exception("submitBill没有订阅者");
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m_card1">被替换的卡牌</param>
        /// <param name="m_card2">需要添加的卡牌</param>
        /// <param name="m_playerName">操作者名字</param>
        public void ReplaceBill(int m_card1,int m_card2,string m_playerName)
        {
            Bill bill = new Bill();
            bill.card1 = m_card1;
            bill.card2 = m_card2;
            bill.action = "replace";
            bill.name = m_playerName;
            if (submitBill != null)
            {
                submitBill.Invoke(bill);
            }
            else
            {
                throw new Exception("submitBill没有订阅者");
            }
        }
        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            FindObjectOfType<NegociateState>().RoundStartInvoke();
        }

    }
}
