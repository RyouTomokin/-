using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Peixi;
using C;

namespace Tomokin
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public static int Turns;
        public static int Stages = 1;

        public HandCardsManager HCM;                                //本玩家的手牌管理器

        public List<CardData> CardsInLibarary;                      //牌库中的卡牌
        private List<GameObject> UIs = new List<GameObject>();      //协议书和手牌的UI（按钮）
        public List<GameObject> CardsInBook;                        //协议书UI（按钮）
        private GameObject[] LockBook = new GameObject[2];          //被锁定的协议书槽
        public List<GameObject> CardsInHand;                        //手牌UI（按钮）
        public Dropdown dropInRoom;                                 //在房间时的Dropdown
        private bool isNotNull = false;                             //标记，是否点击空白
        public CardMsg[] CM;                                        //所有的卡牌信息类
        public Text OutPut;                                         //打印提案显示

        private PrepareStateEvent prepare;
        public Transform[] BillsCardUI;
        public Transform[] BillsText;
        public Transform CilentPlayer;
        public Transform Player1, Player2;

        public void StartStage()
        {
            //设定倒计时
            //开启这回合的UI
            if (Stages == 3)
            {
                //提案排序
                ProposalManager.PropsofthisTurn = HouseOwner.PropSort(ProposalManager.PropsofthisTurn);
                //展示这些提案
                FindObjectOfType<NegociateState>().RoundStartInvoke();
                //InputProposol();
            }
            if (Stages == 4)
            {
                List<Bill> BillList = new List<Bill>();
                List<Proposal> props = ProposalManager.PropsofthisTurn;
                string tip;
                foreach (var prop in props)
                {
                    tip = ProposalManager.GetPropType(prop);
                    BillList.Add(new Bill(prop.Player.PlayerName, tip,
                        prop.HandCard.Get_Order, prop.BookCard.Get_Order));
                }
                FindObjectOfType<VoteState>().RoundStartInvoke(BillList);
                if (CilentManager.playerdata.IsHouseOwner)
                {
                    SendPropForVote(0);
                }
            }
            if (Stages == 5)
            {
                Score[] scores = new Score[3];
                int i = 0;
                foreach (var pd in CilentManager.PDs)
                {
                    Score score = new Score(pd.PlayerName, pd.GetMoney, pd.GetChip, pd.GetScore);
                    scores[i] = score;
                    i++;
                }
                FindObjectOfType<AccountState>().RoundStartInvoke(scores);
            }
        }
        //倒计时结束或者操作结束时调用
        public void EndStage()
        {
            //单机测试
            //HouseOwner.AddJieDuan();
            Net.SendActionEndMessage();
        }

        #region 提案(部分)
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

        //向服务器提交自己的议案
        public void SendMyProposal(Proposal myProp)
        {
            string tip = "";    //添加替换删除
            StepTwoActionData AD = new StepTwoActionData(CilentManager.PlayerName + CilentManager.PlayerID,
                2, tip, myProp.HandCard.Get_Order, myProp.InBook, myProp.BookCard.Get_Order,
                CilentManager.PlayerName);
            Net.SendAction(AD);
        }
        /// <summary>
        /// 展示提案
        /// </summary>
        public void InputProposol()
        {
            int i = 0;
            int type = 0;
            foreach (var prop in ProposalManager.PropsofthisTurn)
            {
                CardMsg HC = BillsCardUI[i].Find("HandCard").GetComponent<CardMsg>();
                CardMsg BC = BillsCardUI[i].Find("BookCard").GetComponent<CardMsg>();
                if (prop.HandCard)
                {
                    HC.card = prop.HandCard;
                    HC.gameObject.SetActive(true);
                    InputCardMsg(HC.gameObject);
                    type++;
                }
                else
                {
                    HC.card = null;
                    HC.gameObject.SetActive(false);
                }
                if (prop.BookCard)
                {
                    BC.card = prop.BookCard;
                    BC.gameObject.SetActive(true);
                    InputCardMsg(BC.gameObject);
                    type += 2;
                }
                else
                {
                    BC.card = null;
                    BC.gameObject.SetActive(false);
                }
                if (type == 1)
                {
                    BillsText[i].Find("player").GetComponent<Text>().text =
                        prop.Player.PlayerName + "希望";
                    BillsText[i].Find("content").GetComponent<Text>().text =
                        "添加此卡牌";
                }
                else if (type == 2)
                {
                    BillsText[i].Find("player").GetComponent<Text>().text =
                        prop.Player.PlayerName + "希望删除此卡牌";
                    BillsText[i].Find("content").GetComponent<Text>().text =
                        "希望删除此卡牌";
                }
                else if (type == 3)
                {
                    BillsText[i].Find("player").GetComponent<Text>().text =
                        prop.Player.PlayerName + "希望";
                    BillsText[i].Find("content").GetComponent<Text>().text =
                        "替换此卡牌到该卡牌的曾在的协议书上";
                }
                type = 0;
                i++;
            }
        }
        #endregion
        /// <summary>
        /// 为卡牌填写属性信息
        /// </summary>
        /// <param name="C_obj">卡牌对象</param>
        public void InputCardMsg(GameObject C_obj)
        {
            int i = 0;
            foreach (var text in C_obj.GetComponentsInChildren<Text>())
            {
                int v = C_obj.GetComponent<CardMsg>().card.GetByNum(i);
                if (v > 0) text.text = "+" + v;
                else text.text = v.ToString();
                i++;
            }
        }

        #region 拉拢模块

        //贿赂失败
        public void BriebFail(string player_n)
        {
            prepare.InvokeBribeRequestResultReceived(player_n, false);
        }

        //成功贿赂其他玩家（在之前要判断钱袋）
        public void BribeSuccess(PlayerGameData Bebribe_Player)
        {
            Bebribe_Player.Bebribed.Add(CilentManager.playerdata);
            Bebribe_Player.GetMoney = 2;
            CilentManager.playerdata.GetMoney = -2;
        }
        //通过姓名来检索玩家(回调)
        public void BribeSuccess(string player_n)
        {
            prepare.InvokeBribeRequestResultReceived(player_n, true);
            foreach (var pd in CilentManager.PDs)
            {
                if (player_n == pd.PlayerName)
                {
                    BribeSuccess(pd);
                    return;
                }
            }
            Debug.LogError("检索玩家姓名失败");
        }
        public void NoteBribe(string name)
        {
            Net.SendActionAns(name, 0);
        }

        //被其他玩家贿赂成功(监听贿赂同意的按钮)
        public void BeBribe(PlayerGameData Player)
        {
            CilentManager.playerdata.Bebribed.Add(Player);
            CilentManager.playerdata.GetMoney = 2;
            Player.GetMoney = -2;
            Net.SendActionAns(Player.PlayerName, 1);
        }
        /// <summary>
        /// 被其他玩家贿赂成功(监听贿赂同意的按钮)
        /// </summary>
        /// <param name="name">行贿人的名字</param>
        public void BeBribe(string name)
        {
            foreach (var pd in CilentManager.PDs)
            {
                if (pd.PlayerName == name)
                {
                    BeBribe(pd);
                }
            }
        }
        /// <summary>
        /// 发送贿赂请求
        /// </summary>
        /// <param name="num">被贿赂者的序号</param>
        public void SendBrideMsg(int num)
        {
            num = (CilentManager.PlayerNum + num) % 3;
            Debug.Log(num);
            if (!(num < CilentManager.PDs.Length)) return;
            //输出打印
            string msg = CilentManager.PlayerName + CilentManager.PlayerID +
                "给" + CilentManager.PDs[num].PlayerName + "发送贿赂请求";
            Debug.Log(msg);
            FindObjectOfType<TextInputManager>().SendMsg(msg);

            //发送贿赂请求（NET）
            ActionData ad = new StepOneActionData(CilentManager.PlayerName + CilentManager.PlayerID,
                1, "贿赂", CilentManager.PDs[num].PlayerName);
            Net.SendAction(ad);
        }
        /// <summary>
        /// 收到贿赂请求弹出窗口
        /// </summary>
        /// <param name="name">贿赂者的名字+ID</param>
        public void ReceivedBrideMsg(string name)
        {
            Debug.Log("弹出窗口");
            int i = 0;
            foreach (var pd in CilentManager.PDs)
            {
                if (pd.PlayerName == name)
                {
                    prepare.OnBribeMessageReceived(i);
                    return;
                }
                i++;
            }
            Debug.Log("未找到name=" + name + "的玩家");
        }

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
            //C_obj.GetComponent<Image>().sprite = C_msg.icon;

            InputCardMsg(C_obj);
            //int i = 0;
            //foreach (var text in C_obj.GetComponentsInChildren<Text>())
            //{
            //    int v = C_obj.GetComponent<CardMsg>().card.GetByNum(i);
            //    if (v > 0) text.text = "+" + v;
            //    else text.text = v.ToString();
            //    i++;
            //}

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
            UpdateUIMsg();
            SendPlayerMsg();
            RollCards(HCM);
        }
        #endregion


        /// <summary>
        /// 向服务器发送需要投票的提案,上一个投票结束也会调用(最好是房主来操作)
        /// </summary>
        public void SendPropForVote(int n)
        {
            Proposal prop = ProposalManager.PropsofthisTurn[n];
            string tip = "";    //添加替换删除
            tip = ProposalManager.GetPropType(prop);
            //一个提案
            StepTwoActionData AD = new StepTwoActionData(CilentManager.PlayerName + CilentManager.PlayerID,
                2, tip,
                prop.HandCard.Get_Order,
                prop.InBook,
                prop.BookCard.Get_Order,
                prop.Player.PlayerName);
            ProposalManager.PropsofthisTurn.RemoveAt(n);
            Net.StartVote(AD, 0);
        }
        //将客户端玩家信息上传服务器
        public void SendPlayerMsg()
        {
            Net.SynchronizeAssets(CilentManager.playerdata.GetMoney,
                CilentManager.playerdata.GetChip);
        }

        //本地玩家的UI信息更新
        public void UpdateUIMsg()
        {
            CilentPlayer.Find("name").GetComponent<Text>().text = CilentManager.PlayerName;
            CilentPlayer.Find("chip").GetComponent<Text>().text =
                string.Format("{0}", CilentManager.playerdata.GetChip);
            CilentPlayer.Find("gcoin").GetComponent<Text>().text =
                string.Format("{0}", CilentManager.playerdata.GetMoney);

            int n = CilentManager.PlayerNum;
            Player1.Find("name").GetComponent<Text>().text =
                CilentManager.PDs[(n + 1) % 3].PlayerName;
            Player1.Find("chip").GetComponent<Text>().text =
                string.Format("{0}", CilentManager.PDs[(n + 1) % 3].GetChip);

            Player2.Find("name").GetComponent<Text>().text =
                CilentManager.PDs[(n + 2) % 3].PlayerName;
            Player2.Find("chip").GetComponent<Text>().text =
                string.Format("{0}", CilentManager.PDs[(n + 2) % 3].GetChip);

        }

        //投票(上传服务器)
        public void SendVote(float poll)
        {
            Net.ClientReturnVoteAns(poll, 0);
        }
        //投票(转换票数)
        public void SendVote(bool agree)
        {
            if (agree) SendVote(1);
            else SendVote(-1);
        }
        //投额外一票(转换票数)
        public void SendExvote(bool agree)
        {
            if (CilentManager.playerdata.ExVote)
            {
                if (agree) SendVote(1.5f);
                else SendVote(-1.5f);
                CilentManager.playerdata.ExVote = false;
            }
        }
        /// <summary>
        /// 上传投票结果到服务器
        /// </summary>
        public void SendVoteResult()
        {
            float v, a, d;
            bool vb;
            v = HouseOwner.GetVote;
            if (v > 0) vb = true;
            else vb = false;
            a = HouseOwner.GetAgree;
            d = HouseOwner.GetDisagree;
            Net.EndVote(vb, a, d);
        }

        //房主统计票数

        //提案通过，客户端更新协议书(和自己的手牌)



        //开始某阶段


        //购票减筹码（在之前要判断筹码）(监听)
        public void BuyExVote(bool b)
        {
            if (b)
            {
                if (CilentManager.playerdata.GetChip >= 2)
                {
                    CilentManager.playerdata.GetChip = -2;
                    CilentManager.playerdata.ExVote = true;
                    //上传玩家信息
                    UpdateUIMsg();
                    SendPlayerMsg();
                }
            }
        }

        public void VoteEnd(float agree, float disagree)
        {
            //显示投票结果
            Vote v = new Vote();
            v.positiveVote = agree;
            v.negativeVote = disagree;
            FindObjectOfType<VoteState>().InvokeShowVoteResult(v);
            //(房主)如果还有提案没投票，开始新一轮投票
            if (CilentManager.playerdata.IsHouseOwner)
            {
                FindObjectOfType<VoteState>().StartVoteRound();
                if (ProposalManager.PropsofthisTurn.Count == 0)
                    SendPropForVote(0);
            }
        }

        // 打印提案
        public void Agree()
        {
            string str = ProposalManager.PrintProp();
            ProposalManager.AgreeProp();
            if (OutPut != null)
                OutPut.text += str;
        }

        // Use this for initialization
        void OnEnable()
        {
            Instance = this;
            prepare = FindObjectOfType<PrepareStateEvent>();
            //订阅
            //准备阶段
            prepare.onRollCard += Roll;   //监听换牌按钮
            //prepare.bribeMessageSent += SendBrideMsg; //监听发送贿赂请求按钮
            prepare.approveBribe += BeBribe;    //接收贿赂
            prepare.rejectBribe += NoteBribe;   //拒绝贿赂

            //谈判阶段
            FindObjectOfType<NegociateState>().onAgreeBuyTicket += BuyExVote;

            //投票阶段
            FindObjectOfType<VoteState>().onVoteSent += SendVote;
            FindObjectOfType<VoteState>().onUseTicket += SendExvote;

            //阶段结束
            prepare.onRoundEnded += EndStage;
            FindObjectOfType<ProposalStateEvent>().onRoundEnded += EndStage;
            FindObjectOfType<NegociateState>().onRoundEnded += EndStage;
            FindObjectOfType<VoteState>().onRoundEnded += EndStage;
            FindObjectOfType<AccountState>().onRoundEnded += EndStage;

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