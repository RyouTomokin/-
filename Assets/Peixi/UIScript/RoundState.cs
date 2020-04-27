using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public class RoundState : MonoBehaviour
    {
        public event Action onRoundStarted;
        public event Action onRoundEnded;
        protected virtual void OnRoundStart()
        {
        }
        protected virtual void OnRoundEnd()
        {
        }
        private void Start()
        {
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }
        /// <summary>
        /// 外部成员开启回合
        /// </summary>
        public void RoundStartInvoke()
        {
            onRoundStarted.Invoke();
        }
        /// <summary>
        /// 外部成员结束回合
        /// </summary>
        public void RoundEndInvoke()
        {
            onRoundEnded.Invoke();
        }
    }
}

