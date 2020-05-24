using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
using Tomokin;
using UnityEngine.UI;

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
        public GameObject bribeMessageFrame;
        public GameObject bribeWarnFrame;
        public GameObject bribeResultFrame;

        delegate void EmptyEngine();
        EmptyEngine engine;
        public GameObject inquireRollCardFrame;
        public GameObject rollCardButton;
        public GameObject confirmRollCardButton;
        public GameObject cancelRollCardButton;
        public GameObject rollCardWarnFrame;

        /// <summary>
        /// 玩家Roll牌
        /// </summary>
        public event Action onRollCard;
        /// <summary>
        /// 拒绝接受贿赂
        /// </summary>
        public event Action<string> rejectBribe;
        /// <summary>
        /// 同意接受贿赂
        /// </summary>
        public event Action<string> approveBribe;
        /// <summary>
        /// 收到线上玩家的贿赂请求消息
        /// </summary>
        public event Action<int> bribeMessageReceived;
        /// <summary>
        /// 向其他玩家发送贿赂请求
        /// 参数1：受贿人名字
        /// 参数2：行贿人名字
        /// </summary>
        public event Action<string,string> bribeMessageSent;
        /// <summary>
        /// 收到贿赂请求处理结果
        /// </summary>
        public event Action<string, bool> bribeRequestResultReceived;

        //PlayerInformation info ;
        // Start is called before the first frame update
        void Start()
        {
            director = GetComponent<PlayableDirector>();
            //info = FindObjectOfType<PlayerInformation>();
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

            bribeButton.SetActive(true);
            rollCardButton.SetActive(true);

            //print("开始准备阶段");
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
        /// <summary>
        /// 贿赂玩家(btn)
        /// </summary>
        public void OnBribeButtonPressed()
        {
            int coin = Tomokin.CilentManager.playerdata.GetMoney;
            if (coin >= 2)
            {
                //print("点击了贿赂按钮");
                //if (director.state == PlayState.Paused)
                //{
                //    director.playableAsset = timeLines[1];
                //    director.Play();
                //}
                dealOnlinePlayer1Button.SetActive(true);
                dealOnlinePlayer2Button.SetActive(true);
                bribeButton.SetActive(false);
            }
            else
            {
                bribeWarnFrame.SetActive(true);
            }
        }
        /// <summary>
        /// PY交易(btn)
        /// </summary>
        /// <param name="playerNum">玩家序号</param>
        public void OnDealPlayerButtonPressed(int playerNum)
        {
            //是否花费2GB贿赂的弹窗
            //inquireBribeFrame.SetActive(true);
            dealOnlinePlayer1Button.SetActive(false);
            dealOnlinePlayer2Button.SetActive(false);
            if (bribeMessageSent != null)
            {
                //bribeMessageSent.Invoke(playerNum);
            }
            else
            {
                Debug.LogWarning("bribeMessageSent没有订阅者");
            }
        }
        public void OnConfirmBribeButtonPressed()
        {
            print("confirm bribe button press");
            inquireBribeFrame.SetActive(false);
            bribeButton.SetActive(true);
            //调用贿赂的底层逻辑（需要知道被贿赂的对象）
        }
        /// <summary>
        /// 服务器向本地玩家发送BribeMessage
        /// </summary>
        /// <param name="n">发送者的序号</param>
        public void OnBribeMessageReceived(int n)
        {
            if (bribeMessageReceived != null)
            {
                bribeMessageReceived.Invoke(n);
            }
            //print("收到其他玩家的悄悄话");
        }
        public void InvokeApproveBribe(string name)
        {
            if (approveBribe != null)
            {
                approveBribe.Invoke(name);
            }
        }
        public void InvokeRejectBribe(string m_name)
        {
            if (rejectBribe != null)
            {
                rejectBribe.Invoke(m_name);
            }
        }
        /// <summary>
        /// 服务器回答贿赂请求结果
        /// </summary>
        /// <param name="m_name">受贿人的名字</param>
        /// <param name="m_result">受贿结果</param>
        public void InvokeBribeRequestResultReceived(string m_name, bool m_result)
        {
            bribeResultFrame.SetActive(true);
            var content = bribeResultFrame.GetComponentInChildren<Text>();
            if (m_result)
            {
                content.text = m_name + "同意了你的请求";
            }
            else
            {
                content.text = m_name + "拒绝了你的请求";
            }
        }
        #endregion

        #region//RollCard
        /// <summary>
        /// 询问是否Rollcard(btn)
        /// </summary>
        public void OnRollCardButtonPressed()
        {
            var chip = Tomokin.CilentManager.playerdata.GetChip;
            if (chip >= 2)
            {
                rollCardButton.SetActive(false);
                inquireRollCardFrame.SetActive(true);
            }
            else
            {
                rollCardWarnFrame.SetActive(true);
            }
        }
        /// <summary>
        /// 确认Roll牌(btn)
        /// </summary>
        public void OnConfirmRollCardButtonPressed()
        {
            print("confirm roll card");
            //rollCardButton.SetActive(true);
            inquireRollCardFrame.SetActive(false);
            director.playableAsset = timeLines[2];
            director.Play();

            if (onRollCard != null)
            {
                CilentManager.playerdata.GetChip = -2;
                onRollCard.Invoke();
            }
        }

        public void OnCancelRollCardButtonPressed()
        {
            print("cancel roll card");
            rollCardButton.SetActive(true);
            inquireRollCardFrame.SetActive(false);
        }

        public void ShutDownWarnFrame()
        {
            rollCardWarnFrame.SetActive(false);
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