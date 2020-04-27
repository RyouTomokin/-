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

        public GameObject OtherPlayerMsg;   //其他玩家的信息
        public GameObject Player;           //本玩家
        public GameObject PlayerR;          //红色玩家
        public GameObject PlayerB;          //蓝色玩家
        private ActionData A_Data;          //本玩家的行为信息
        public GameObject MessageLabel;     //本玩家的信息栏
        public static string PlayerName;
        public HandCardsManager HCM;

        public List<CardData> CardsInLibarary;                      //牌库中的卡牌
        private List<GameObject> UIs = new List<GameObject>();      //协议书和手牌的UI
        public List<GameObject> CardsInBook;                        //协议书UI
        private GameObject[] LockBook = new GameObject[2];
        public List<GameObject> CardsInHand;                        //手牌UI
        private bool isNotNull = false;                             //标记，是否点击空白
        public CardMsg[] CM;
        private List<CardData> UnableCards = null;
        //public Text OutPut;

        void ShowPlayerMsg()
        {
            Text txt = MessageLabel.GetComponentInChildren<Text>();
            txt.text = string.Format("钱袋 = {0}" +
                "\n筹码 = {1}" +
                "\n选票 = {2}",
                A_Data.P_Data.GetMoney, A_Data.P_Data.GetChip, A_Data.P_Data.GetVote);
            if (A_Data.GetBeBribed != null)
            {
                txt.text += string.Format("\n被{0}贿赂", A_Data.GetBeBribed.name);
            }
        }

        /// <summary>
        /// 贿赂（按钮）
        /// </summary>
        public void Bribed()
        {
            GameObject obj = EnemyEvent.SelectedObj;
            if (A_Data.P_Data.GetMoney > 1)
            {
                A_Data.P_Data.GetMoney = -2;
                obj.GetComponent<ActionData>().GetBeBribed = Player;
                obj.GetComponent<ActionData>().P_Data.GetMoney = 2;
            }

            obj.GetComponent<EnemyEvent>().ShowMsg();
        }
        /// <summary>
        /// 投票（按钮）
        /// </summary>
        public void Vote()
        {
            GameObject obj = EnemyEvent.SelectedObj;
            if (A_Data.P_Data.GetVote > 0)
            {
                A_Data.P_Data.GetVote = -1;
                obj.GetComponent<ActionData>().VoteNumofProposal++;
            }
            else if (A_Data.P_Data.GetExVote > 0)
            {
                A_Data.P_Data.GetExVote = -1;
                obj.GetComponent<ActionData>().VoteNumofProposal += 1.5f;
            }
        }

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
        public void RefreshLib(HandCardsManager hm)
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

            RefreshLib(hm);

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
                CardData nCard = null;
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


        //public void Agree()
        //{
        //    string str = ProposalManager.PrintProp();
        //    ProposalManager.AgreeProp();
        //    OutPut.text += str;
        //}

        public void guiLing()
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


        //只计算自己的分数
        public void SettleCore()
        {
            CardData cd;
            foreach (var cib in CardsInBook)
            {
                GameObject CIB = cib.transform.parent.gameObject;
                if (CIB.activeSelf)
                {
                    cd = CIB.GetComponent<CardMsg>().card;
                    A_Data.P_Data.GetMoney = cd.Get_G;
                    PlayerR.GetComponent<ActionData>().P_Data.GetMoney = cd.Get_R;
                    PlayerB.GetComponent<ActionData>().P_Data.GetMoney = cd.Get_B;
                    //性格的加成
                }
            }
            EnemyEvent.SelectedObj.gameObject.GetComponent<EnemyEvent>().ShowMsg();
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
            FindObjectOfType<PrepareStateEvent>().onRollCard += Roll;
            //LockBook = new GameObject[2]{ CardsInBook[4], CardsInBook[5]};

            //UI初始化
            //OtherPlayerMsg.SetActive(false);//用于SimpleScene
            UIs.AddRange(CardsInBook);
            UIs.AddRange(CardsInHand);
            foreach (GameObject item in UIs)
            {
                item.SetActive(false);
                //item.transform.parent.gameObject.SetActive(false);
            }
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