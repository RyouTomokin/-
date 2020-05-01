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
        /// <summary>
        /// 外部人员管理，唤醒阶段的事件
        /// </summary>
        protected Dictionary<string, Action> registeredEvents = new Dictionary<string, Action>();
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="m_action"></param>
        protected void RegisterEvent(string name,Action m_action)
        {
            if (registeredEvents.ContainsKey(name))
            {
                throw new Exception("事件" + name + "已经被注册");
            }
            else
            {
                registeredEvents.Add(name, m_action);
            }
        }
        public void InvokeEvent(string eventName)
        {
            if (registeredEvents.ContainsKey(eventName))
            {
                registeredEvents[eventName].Invoke();
            }
            else
            {
                throw new Exception("事件" + eventName + "没有被注册或者事件名称错误");
            }
        }
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

