using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Peixi
{
    public class NegociateState : RoundState
    {
        [SerializeField]
        protected GameObject ticketFrame;

        /// <summary>
        /// 同意购买额外一票
        /// </summary>
        public event Action<bool> onAgreeBuyTicket;
        private void Start()
        {
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }

        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            print("开始协商阶段");
            ticketFrame.SetActive(true);
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束协商阶段，等待其他玩家");
            ticketFrame.SetActive(false);
            StartCoroutine(RoundInterval());
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
            FindObjectOfType<VoteState>().RoundStartInvoke();
        }
    }
}

