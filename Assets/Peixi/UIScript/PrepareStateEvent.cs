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
        public event Action rejectBribe;
        /// <summary>
        /// 同意接受贿赂
        /// </summary>
        public event Action approveBribe;
        /// <summary>
        /// 收到线上玩家的贿赂请求消息
        /// </summary>
        public event Action<string> bribeMessageReceived;
        /// <summary>
        /// 向其他玩家发送贿赂请求
        /// </summary>
        public event Action<int> bribeMessageSent;
        /// <summary>
        /// 收到贿赂请求处理结果
        /// </summary>
        public event Action<string,bool> bribeRequestResultReceived;
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
        public void OnBribeButtonPressed()
        {
            int coin = PlayerInformation.instance.Chip;
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
        public void OnDealPlayerButtonPressed(int playerNum)
        {
            inquireBribeFrame.SetActive(true);
            dealOnlinePlayer1Button.SetActive(false);
            dealOnlinePlayer2Button.SetActive(false);
            bribeMessageSent.Invoke(playerNum);
        }
        public void OnConfirmBribeButtonPressed()
        {
            print("confirm bribe button press");
            inquireBribeFrame.SetActive(false);
            bribeButton.SetActive(true);
            //调用贿赂的底层逻辑（需要知道被贿赂的对象）
        }
        //public void OnCancelBribeButtonPressed()
        //{
        //    print("cancel bribe button press");
        //    inquireBribeFrame.SetActive(false);
        //    bribeButton.SetActive(true);
        //}
        //public void ShutDownBribeWarnFrame()
        //{
        //    bribeWarnFrame.SetActive(false);
        //}
        /// <summary>
        /// 服务器向本地玩家发送BribeMessage
        /// </summary>
        /// <param name="name">发送者的名字</param>
        public void OnBribeMessageReceived(string name)
        {
            if (bribeMessageReceived != null)
            {
                bribeMessageReceived.Invoke(name);
            }
            //print("收到其他玩家的悄悄话");
        }
        /// <summary>
        /// 服务器向本地玩家发送BribeMessage
        /// </summary>
        /// <param name="playerNumber">发送者的编号</param>
        public void OnBribeMessageReceived(int playerNumber)
        {
            if (bribeMessageReceived != null)
            {
                if (playerNumber == 1)
                {
                   //需要一个字典来进行名字和编号的转换
                }
            }
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
        /// <summary>
        /// 服务器回答贿赂请求结果
        /// </summary>
        /// <param name="m_name">受贿人的名字</param>
        /// <param name="m_result">受贿结果</param>
        public void InvokeBribeRequestResultReceived(string m_name,bool m_result)
        {
            if (bribeRequestResultReceived != null)
            {
                bribeRequestResultReceived.Invoke(m_name, m_result);
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
        }
        #endregion

        #region//RollCard
        /// <summary>
        /// 询问是否Rollcard
        /// </summary>
        public void OnRollCardButtonPressed()
        {
            //print("press roll card button");
            PlayerInformation playerInformation = FindObjectOfType<PlayerInformation>();
            //print(playerInformation.Chip);
            if (playerInformation.Chip >= 2)
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

        public void ShutDownWarnFrame()
        {
            rollCardWarnFrame.SetActive(false);
        }
        #endregion

        public void StartRound()
        {
 
        }

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