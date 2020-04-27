using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Peixi
{
    public class PrepareStateEvent : RoundState
    {
        public PlayableDirector director;
        public PlayableAsset[] timeLines;
        public GameObject inquireBribeFrame;
        public GameObject dealOnlinePlayer1Button;
        public GameObject dealOnlinePlayer2Button;
        public GameObject bribeButton;

        delegate void EmptyEngine();
        EmptyEngine engine;
        public GameObject inquireRollCardFrame;
        public GameObject rollCardButton;
        public GameObject confirmRollCardButton;
        public GameObject cancelRollCardButton;
        public GameObject bribeMessageFrame;

        /// <summary>
        /// 玩家Roll牌
        /// </summary>
        public event Action onRollCard;
        /// <summary>
        /// 拒绝接受贿赂
        /// </summary>
        public event Action rejectBribe;
        /// <summary>
        /// 同意接受贿赂
        /// </summary>
        public event Action approveBribe;
        /// <summary>
        /// 收到线上玩家的贿赂请求消息
        /// </summary>
        public event Action bribeMessageReceived;
        /// <summary>
        /// 向其他玩家发送贿赂请求
        /// </summary>
        public event Action<string> bribeMessageSent;
        /// <summary>
        /// 收到贿赂请求处理结果
        /// </summary>
        public event Action<bool> bribeRequestResultReceived;
        // Start is called before the first frame update
        void Start()
        {
            director = GetComponent<PlayableDirector>();
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }

        // Update is called once per frame
        void Update()
        {
            if (engine != null)
            {
                engine();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                OnTimeOut();
            }
        }

        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            print("开始准备阶段");
            director.playableAsset = timeLines[0];
            director.Play();
        }

        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束准备阶段，等待其他玩家");
            director.playableAsset = timeLines[0];
            director.time = director.duration;
            engine += PlayStateEndAnim;
            StartCoroutine(RoundInterval());
        }

        #region//Bribe
        public void OnBribeButtonPressed()
        {
            print("点击了贿赂按钮");
            //if (director.state == PlayState.Paused)
            //{
            //    director.playableAsset = timeLines[1];
            //    director.Play();
            //}
            dealOnlinePlayer1Button.SetActive(true);
            bribeButton.SetActive(false);
            
        }

        public void OnDealPlayer1ButtonPressed()
        {
            inquireBribeFrame.SetActive(true);
            dealOnlinePlayer1Button.SetActive(false);
        }

        public void OnDealPlayer2ButtonPressed()
        {

        }

        public void OnConfirmBribeButtonPressed()
        {
            print("confirm bribe button press");
            inquireBribeFrame.SetActive(false);
            bribeButton.SetActive(true);
        }

        public void OnCancelBribeButtonPressed()
        {
            print("cancel bribe button press");
            inquireBribeFrame.SetActive(false);
            bribeButton.SetActive(true);
        }

        /// <summary>
        /// 外界向本地玩家发送BribeMessage时使用此方法
        /// </summary>
        public void OnBribeMessageReceived()
        {
            if (bribeMessageReceived != null)
            {
                bribeMessageReceived.Invoke();
            }
            //print("收到其他玩家的悄悄话");
        }

        public void InvokeApproveBribe()
        {
            if (approveBribe != null)
            {
                approveBribe.Invoke();
            }
        }

        public void InvokeRejectBribe()
        {
            if (rejectBribe != null)
            {
                rejectBribe.Invoke();
            }
        }

        public void InvokeBribeRequestResultReceived(bool m_result)
        {
            bribeRequestResultReceived.Invoke(m_result);
        }
        #endregion

        #region//RollCard
        /// <summary>
        /// 询问是否Rollcard
        /// </summary>
        public void OnRollCardButtonPressed()
        {
            print("press roll card button");
            rollCardButton.SetActive(false);
            inquireRollCardFrame.SetActive(true);
        }
        /// <summary>
        /// 确认Roll牌
        /// </summary>
        public void OnConfirmRollCardButtonPressed()
        {
            print("confirm roll card");
            rollCardButton.SetActive(true);
            inquireRollCardFrame.SetActive(false);
            director.playableAsset = timeLines[2];
            director.Play();

            if (onRollCard != null)
            {
                onRollCard.Invoke();
            }
        }

        public void OnCancelRollCardButtonPressed()
        {
            print("cancel roll card");
            rollCardButton.SetActive(true);
            inquireRollCardFrame.SetActive(false);
        }
        #endregion

        /// <summary>
        /// 时间到或者按下结束按钮
        /// </summary>
        public void OnTimeOut()
        {
            print("结束准备阶段，等待其他玩家");
        }

        void PlayStateEndAnim()
        {
            director.time -= Time.deltaTime;
            director.Evaluate();
            if (director.time <= 0)
            {
                director.time = 0;
                director.Pause();
                engine -= PlayStateEndAnim;
            }
        }

        #region//Test code
        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            FindObjectOfType<ProposalStateEvent>().RoundStartInvoke();
        }
        #endregion
    }
}