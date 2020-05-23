using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public struct Score
    {
        public string name;
        public int GcoinGain;
        public int chipGain;
        public float score;
        public Score(string m_name,int m_coin,int m_chip,float m_score)
        {
            name = m_name;
            GcoinGain = m_coin;
            chipGain = m_chip;
            score = m_score;
        }
    }
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
            accountFrame.SetActive(true);
            // print("开始结算阶段");
            if (onRoundStart != null)
            {
                onRoundStart.Invoke(m_score);
            }
            else
            {
                throw new Exception("onRoundStart没有订阅者");
            }
            
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
            //print("结束结算阶段，等待其他玩家");
            accountFrame.SetActive(false);
        }
        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            FindObjectOfType<PrepareStateEvent>().RoundStartInvoke();
        }
        public void RoundStartInvoke(Score[] m_score)
        {
            accountFrame.SetActive(true);
            onRoundStart.Invoke(m_score);
        }
    }
}

