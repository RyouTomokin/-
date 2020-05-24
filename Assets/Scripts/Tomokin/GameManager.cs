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

        public Sprite[] TypeIcons;
        public static Sprite[] Icons;
        public Sprite[] TeemIcons;
        private PrepareStateEvent prepare;
        public Transform[] BillsCardUI;
        public Transform[] BillsText;
        public Transform CilentPlayer;
        public Transform Player1, Player2;

        private float vote;
        private string BribeTarget;

        public void StartStage()
        {
            //Debug.Log("三人准备好了，开始新的阶段");
            if(Stages == 1)
            {
                Debug.Log("开始准备阶段");

                //清零
                CilentManager.playerdata.Bebribed.Clear();
                CilentManager.playerdata.ExVote = false;

                PrepareStateEvent prepare = FindObjectOfType<PrepareStateEvent>();
                prepare.RoundStartInvoke();
            }
            if (Stages == 2)
            {
                Debug.Log("开始提交阶段");
            }
            if (Stages == 3)
            {
                Debug.Log("开始协商阶段");
                //提案排序
                ProposalManager.PropsofthisTurn = HouseOwner.PropSort(ProposalManager.PropsofthisTurn);
                //展示这些提案
                FindObjectOfType<NegociateState>().RoundStartInvoke();
                //InputProposol();
            }
            if (Stages == 4)
            {
                Debug.Log("开始投票阶段");
                List<Bill> BillList = new List<Bill>();
                List<Proposal> props = ProposalManager.PropsofthisTurn;
                string tip;
                foreach (var prop in props)
                {
                    tip = ProposalManager.GetPropType(prop);
                    int hc_id = -1, bc_id = -1;
                    if (prop.HandCard != null) hc_id = prop.HandCard.Get_Order;
                    if (prop.BookCard != null) bc_id = prop.BookCard.Get_Order;

                    BillList.Add(new Bill(prop.Player.PlayerName, tip, hc_id, bc_id));
                }
                //FindObjectOfType<VoteState>().RoundStartInvoke(BillList);
                if (CilentManager.playerdata.IsHouseOwner)
                {
                    HouseOwner.InitVote();  //初始化收到投票的数量
                    SendPropForVote(0);
                }
            }
            if (Stages == 5)
            {
                Debug.Log("结束阶段");
                //删除这个回合的提案
                ProposalManager.PropsofthisTurn.Clear();
                //更新玩家信息
                SendPlayerMsg();

                Invoke("", 0.5f);

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

        private void UpdatePlayerMsgFromBooks()
        {
            int[] NewChip = new int[3] { 0, 0, 0 };
            int value = HCM.drop.value;

            for (int i = 0; i < 3; i++)
            {
                PlayerGameData pd = CilentManager.PDs[i];
                foreach (var book in BookManager.Instance.BookCards)
                {
                    if (book.activeSelf)
                    {
                        NewChip[i] += book.GetComponent<CardMsg>().card.GetByNum(i);
                        //pd.GetMoney = book.GetComponent<CardMsg>().card.GetByNum(i);
                    }
                }
                pd.SetChip = pd.GetChip + NewChip[i];
            }
            PlayerGameData c_pd = CilentManager.playerdata;
            if (value == 0)
            {
                int max = Mathf.Max(NewChip);
                if (NewChip[CilentManager.PlayerNum] == max)
                {
                    //c_pd.SetChip = c_pd.GetChip + 1;
                    c_pd.GetMoney = 1;
                }
                else
                {
                    //c_pd.SetChip = c_pd.GetChip - 2;
                    c_pd.GetMoney = -2;
                }
            }
            else if (value == 1)
            {
                c_pd.GetMoney = NewChip[CilentManager.PlayerNum] / 2;
            }
            else if (value == 2)
            {
                int ww = 0, com = 0;
                foreach (var book in BookManager.Instance.BookCards)
                {
                    if (book.activeSelf)
                    {
                        CardData cd = book.GetComponent<CardMsg>().card;
                        if (cd.GetPreference == 0) ww++;
                        else if (cd.GetPreference == 2) com++;
                    }
                }
                c_pd.GetMoney = ww - com;
            }
        }

        //倒计时结束或者操作结束时调用
        public void EndStage()
        {
            //单机测试
            //HouseOwner.AddJieDuan();
            if (Stages == 4)
            {
                UpdatePlayerMsgFromBooks();
                UpdateUIMsg();
            }
            Debug.Log("阶段结束,上传服务器");
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
            int hc_id = -1;
            int bc_id = -1;
            if (myProp.HandCard != null)
                hc_id = myProp.HandCard.Get_Order;
            if (myProp.BookCard != null)
                bc_id = myProp.BookCard.Get_Order;
            //本地添加提案
            ProposalManager.GetPropFromNet(myProp.InBook, hc_id, bc_id,
                CilentManager.PlayerName + CilentManager.PlayerID);
            //上传服务器
            StepTwoActionData AD = new StepTwoActionData(CilentManager.PlayerName + CilentManager.PlayerID,
                2, tip, hc_id, myProp.InBook, bc_id, CilentManager.PlayerName + CilentManager.PlayerID);
            Stages = 3;
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
        public static void InputCardMsg(GameObject C_obj)
        {
            int i = 0;
            foreach (var text in C_obj.GetComponentsInChildren<Text>())
            {
                if (i >= 3)
                {
                    text.text = C_obj.GetComponent<CardMsg>().card.Get_CardName;
                    break;
                }
                int v = C_obj.GetComponent<CardMsg>().card.GetByNum(i);
                if (v > 0) text.text = "+" + v;
                else text.text = v.ToString();
                i++;
            }

            C_obj.transform.Find("CardAnim").Find("type").GetComponent<Image>().sprite =
                 Icons[C_obj.GetComponent<CardMsg>().card.GetPreference];
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
            //Bebribe_Player.GetMoney = 2;
            CilentManager.playerdata.GetMoney = -2;
            UpdateUIMsg();
        }
        //通过姓名来检索玩家(回调)
        public void BribeSuccess(string player_n)
        {
            if (player_n != CilentManager.PlayerName + CilentManager.PlayerID) return;

            prepare.InvokeBribeRequestResultReceived(BribeTarget, true);
            foreach (var pd in CilentManager.PDs)
            {
                string n = TomokinNet.OnlyName(pd.PlayerName);
                if (BribeTarget == n)
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
            //Player.GetMoney = -2;
            UpdateUIMsg();
            SendPlayerMsg();
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
            BribeTarget = TomokinNet.OnlyName(CilentManager.PDs[num].PlayerName);
            string msg = CilentManager.PlayerName + "给" +
                BribeTarget + "发送贿赂请求";
            Debug.Log(msg);
            //FindObjectOfType<TextInputManager>().SendMsg(msg);

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

        #region 投票模块

        public void ADtoProp(StepTwoActionData AD)
        {
            //Debug.Log("AD.name = " + AD.owner_nickname);
            foreach (var pd in CilentManager.PDs)
            {
                //Debug.Log("pd.name = " + pd.PlayerName);
                if (pd.PlayerName == AD.owner_nickname)
                {
                    CardData hc = null;
                    CardData bc = null;
                    if (AD.hand_card != -1) hc = CardsInLibarary[AD.hand_card];
                    if (AD.agreement_card != -1) bc = CardsInLibarary[AD.agreement_card];

                    if (hc == null) Debug.Log("hc is null");
                    if (bc == null) Debug.Log("bc is null");

                    Proposal prop = new Proposal(AD.agreement_id, hc, bc, pd);
                    CilentManager.PropNeedVote = prop;
                    break;
                }
            }
        }

        /// <summary>
        /// 向服务器发送需要投票的提案,上一个投票结束也会调用(最好是房主来操作)
        /// </summary>
        public void SendPropForVote(int n)
        {
            Proposal prop = ProposalManager.PropsofthisTurn[n];
            string tip = "";    //添加替换删除
            tip = ProposalManager.GetPropType(prop);
            //一个提案
            int hc_id = -1;
            int bc_id = -1;
            if (prop.HandCard != null) hc_id = prop.HandCard.Get_Order;
            if (prop.BookCard != null) bc_id = prop.BookCard.Get_Order;

            StepTwoActionData AD = new StepTwoActionData(CilentManager.PlayerName + CilentManager.PlayerID,
                2, tip, hc_id, prop.InBook, bc_id, prop.Player.PlayerName);
            ProposalManager.PropsofthisTurn.RemoveAt(n);
            Debug.Log("房主提交一个提案");
            Net.StartVote(AD, 0);
        }
        //投票(转换票数)
        public void SendVote(bool agree)
        {
            if (agree) vote = 1;
            else vote = -1;
        }
        //投额外一票并上传服务器(转换票数)
        public void SendExvote(float agree)
        {
            Debug.Log("额外一票为" + agree);
            vote += agree;
            Net.ClientReturnVoteAns(vote, 0);
        }
        /// <summary>
        /// 上传投票结果到服务器
        /// </summary>
        public void SendVoteResult()
        {
            Debug.Log("将投票结果上传服务器");
            float v, a, d;
            bool vb;
            v = HouseOwner.GetVote;
            if (v > 0) vb = true;
            else vb = false;
            a = HouseOwner.GetAgree;
            d = HouseOwner.GetDisagree;
            Debug.Log(a +" "+  d);
            Net.EndVote(vb, a, d);
        }
        /// <summary>
        /// 显示投票结果
        /// </summary>
        /// <param name="agree"></param>
        /// <param name="disagree"></param>
        public void VoteEnd(float agree, float disagree)
        {
            //显示投票结果
            Vote v = new Vote();
            v.positiveVote = agree;
            v.negativeVote = disagree;
            Debug.Log("显示投票结果");
            if (agree >= disagree) ProposalManager.AgreeProp(CilentManager.PropNeedVote);
            FindObjectOfType<VoteState>().InvokeShowVoteResult(v);
            HouseOwner.InitVote();//可有可无
            Debug.Log("延时3秒后开始新的一轮");
            Invoke("NewVoteTurn", 3f);
        }

        private void NewVoteTurn()
        {
            FindObjectOfType<VoteState>().HideVoteResult();
            //(房主)如果还有提案没投票，开始新一轮投票
            //FindObjectOfType<VoteState>().StartVoteRound();
            Debug.Log("剩余提案的个数" + ProposalManager.PropsofthisTurn.Count);
            if (ProposalManager.PropsofthisTurn.Count != 0)
            {
                if (CilentManager.playerdata.IsHouseOwner)
                    SendPropForVote(0);
            }
            else
            {
                Debug.Log("所有提案都投票结束");
            }
        }
        #endregion

        //将客户端玩家信息上传服务器
        public void SendPlayerMsg()
        {
            Debug.Log("Update:M" + CilentManager.playerdata.GetMoney + "  C" + CilentManager.playerdata.GetChip);
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

            CilentPlayer.Find("camp").GetComponent<Image>().sprite =
                TeemIcons[CilentManager.PDs[n].Number];
            Player1.Find("camp").GetComponent<Image>().sprite =
                TeemIcons[CilentManager.PDs[(n + 1) % 3].Number];
            Player2.Find("camp").GetComponent<Image>().sprite =
                TeemIcons[CilentManager.PDs[(n + 2) % 3].Number];

            Player1.Find("name").GetComponent<Text>().text =
                TomokinNet.OnlyName(CilentManager.PDs[(n + 1) % 3].PlayerName);
            Player1.Find("chip").GetComponent<Text>().text =
                string.Format("{0}", CilentManager.PDs[(n + 1) % 3].GetChip);

            Player2.Find("name").GetComponent<Text>().text =
                TomokinNet.OnlyName(CilentManager.PDs[(n + 2) % 3].PlayerName);
            Player2.Find("chip").GetComponent<Text>().text =
                string.Format("{0}", CilentManager.PDs[(n + 2) % 3].GetChip);

        }

        //提案通过，客户端更新协议书(和自己的手牌)!!!


        //购票减筹码（在之前要判断筹码）(监听)
        public void BuyExVote(bool b)
        {
            if (b)
            {
                if (CilentManager.playerdata.GetChip >= 3)
                {
                    CilentManager.playerdata.GetChip = -3;
                    CilentManager.playerdata.ExVote = true;
                    //上传玩家信息
                    UpdateUIMsg();
                    SendPlayerMsg();
                }
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
            prepare.bribeMessageSent += SendBrideMsg; //监听发送贿赂请求按钮
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
            Icons = TypeIcons;
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