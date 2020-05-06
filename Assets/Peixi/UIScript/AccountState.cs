using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    /// <summary>
    /// 开始AccouState时，需要从服务器传入一个Score结构作为显示结果
    /// </summary>
    public class AccountState : RoundState
    {
        [SerializeField]
        protected GameObject accountFrame;
        public event Action<Score[]> onRoundStart;
        /// <summary>
        /// 外部开启AccouState
        /// </summary>
        /// <param name="m_score"></param>
        public void StartRound(Score[] m_score)
        {
            onRoundStart.Invoke(m_score);
        }
        private void Start()
        {
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }
        protected override void OnRoundStart()
        {
            print("开始结算阶段");
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束结算阶段，等待其他玩家");
            StartCoroutine(RoundInterval());
            accountFrame.SetActive(false);
        }
        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            FindObjectOfType<PrepareStateEvent>().RoundStartInvoke();
        }
        public void RoundStartInvoke(Score[] m_score)
        {
            onRoundStart.Invoke(m_score);
        }
    }
}

