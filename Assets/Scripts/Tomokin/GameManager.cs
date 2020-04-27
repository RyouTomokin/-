using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Peixi;

namespace Tomokin
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public static int Turns;
        public static int Stages;
        public static string PlayerName;                            //本玩家姓名
        public HandCardsManager HCM;                                //本玩家的手牌管理器
        public static PlayerGameData PD;

        public List<CardData> CardsInLibarary;                      //牌库中的卡牌
        private List<GameObject> UIs = new List<GameObject>();      //协议书和手牌的UI（按钮）
        public List<GameObject> CardsInBook;                        //协议书UI（按钮）
        private GameObject[] LockBook = new GameObject[2];          //被锁定的协议书槽
        public List<GameObject> CardsInHand;                        //手牌UI（按钮）
        private bool isNotNull = false;                             //标记，是否点击空白
        public CardMsg[] CM;                                        //所有的卡牌信息类
        public Text OutPut;                                         //打印提案显示

        /// <summary>
        /// 显示更换或删除卡牌的UI
        /// </summary>
        /// <param name="obj">协议书的菜单栏</param>
        public void Show_Replace_Remove(GameObject obj)
        {
            if (obj == LockBook[0] || obj == LockBook[1]) return;
            Show_UI(CardsInBook, obj);
        }
        /// <summary>
        /// 显示添加卡牌的UI
        /// </summary>
        /// <param name="obj">卡牌的菜单栏</param>
        public void Show_Add(GameObject obj)
        {
            Show_UI(CardsInHand, obj);
        }
        /// 显示UI的实际操作
        private void Show_UI(List<GameObject> items, GameObject obj)
        {
            foreach (var item in items)
            {
                if (obj == item)
                {
                    isNotNull = true;
                    item.SetActive(!item.activeSelf);
                    continue;
                }
                item.SetActive(false);
            }
        }

        #region 拉拢模块



        #endregion

        #region 换牌模块
        /// <summary>
        /// 随机获得一个卡牌的属性
        /// </summary>
        /// <param name="v">卡牌类型</param>
        /// <returns>卡牌属性类</returns>
        public CardData RollCard(int v)
        {
            CardData card;
            int c_id;
            for (int i = 0; i < 100; i++)
            {
                c_id = Random.Range(0, CardsInLibarary.Count - 1);
                card = CardsInLibarary[c_id];
                if (card.GetPreference == v && card.Get_IsInLib)
                    return card;
            }
            return null;
        }
        /// <summary>
        /// 更新指定卡牌
        /// </summary>
        /// <param name="C_obj">卡牌对象</param>
        /// <param name="C_msg">卡牌信息</param>
        public void RefreshCard(GameObject C_obj, CardData C_msg)
        {
            C_obj.SetActive(true);
            C_obj.transform.SetAsLastSibling();

            C_obj.GetComponent<CardMsg>().card = C_msg;
            C_obj.GetComponent<Image>().sprite = C_msg.icon;
        }
        /// <summary>
        /// 将手牌和协议书设置为不可获取
        /// </summary>
        /// <param name="hm"></param>
        public void RefreshLib()
        {
            foreach (var cil in CardsInLibarary)
            {
                cil.Get_IsInLib = true;
            }
            foreach (var cm in CM)
            {
                if (cm.GetComponent<CardMsg>().card != null)
                    cm.GetComponent<CardMsg>().card.Get_IsInLib = false;
            }
        }
        /// <summary>
        /// 根据策略换牌
        /// </summary>
        /// <param name="hm"></param>
        public void RollCards(HandCardsManager hm)
        {
            Debug.Log("换牌1");
            int value = hm.drop.value;
            int[] ploy = new int[] { -1, -1, -1, -1, -1 };

            RefreshLib();

            //选择策略
            switch (value)
            {
                case 0:
                    ploy = HandCardsManager.Ploy.PJ;
                    break;
                case 1:
                    ploy = HandCardsManager.Ploy.JH;
                    break;
                case 2:
                    ploy = HandCardsManager.Ploy.WH;
                    break;
            }
            Debug.Log("换牌");
            //为卡牌对象获取卡牌信息
            for (int i = 0; i < hm.Cards.Count; i++)
            {
                GameObject h = hm.Cards[i];
                CardData nCard;
                if (ploy[i] != -1)
                    nCard = RollCard(ploy[i]);
                else
                {
                    Debug.Log("策略加载失败");
                    continue;
                }
                if (nCard == null)
                {
                    Debug.Log("未找到合适的卡牌 :" + ploy[i] + "\n");
                    continue;
                }
                nCard.Get_IsInLib = false;
                RefreshCard(h, nCard);
            }
        }

        public void Roll()
        {
            Debug.Log("Rol Card");
            RollCards(HCM);
        }
        #endregion

        /// <summary>
        /// 打印提案
        /// </summary>
        public void Agree()
        {
            string str = ProposalManager.PrintProp();
            ProposalManager.AgreeProp();
            if (OutPut != null)
                OutPut.text += str;
        }

        public void GuiLing()
        {
            foreach (CardMsg cm in CM)
            {
                cm.card = null;
                cm.gameObject.SetActive(false);
            }
            foreach (CardData cil in CardsInLibarary)
            {
                cil.Get_IsInLib = true;
            }
        }

        //统计票数
        public void CountVotes()
        {
            //向服务器提交数据统一统计
            //或者直接在客户端统计
        }

        // Use this for initialization
        void Start()
        {
            Instance = this;
            //订阅
            FindObjectOfType<PrepareStateEvent>().onRollCard += Roll;
            //锁定协议书槽
            if (CardsInBook.Count > 5)
                LockBook = new GameObject[2] { CardsInBook[4], CardsInBook[5] };

            //UI初始化
            UIs.AddRange(CardsInBook);
            UIs.AddRange(CardsInHand);
            foreach (GameObject item in UIs)
            {
                item.SetActive(false);
                //item.transform.parent.gameObject.SetActive(false);
            }
            //卡牌初始化
            int order = 0;
            foreach (CardData cil in CardsInLibarary)
            {
                cil.Get_IsInLib = true;
                cil.Get_Order = order;
                order++;
            }
            Debug.Log("order = " + order);
        }

        // Update is called once per frame
        void Update()
        {
            //鼠标点击空白的地方取消所有菜单的显示
            if (Input.GetMouseButtonUp(0))
            {
                if (isNotNull)
                    isNotNull = false;
                else
                {
                    foreach (GameObject item in UIs)
                    {
                        item.SetActive(false);
                    }
                }
            }
            //ShowPlayerMsg();//用于SimpleScene
        }
    }
}