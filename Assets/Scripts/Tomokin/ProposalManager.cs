using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomokin
{
    public class ProposalManager
    {
        public static List<Proposal> Props_List = new List<Proposal>();
        public static List<Proposal> PropsofthisTurn;   //接收这个回合的所有提案
        private static Proposal TempProp;
        public static void AddProp(GameObject hand, int book)
        {
            TempProp = new Proposal(hand, book);
            GameManager.Instance.SendMyProposal(TempProp);

            //Props_List.Add(TempProp);             //应当把这个回合的提案一起添加
        }

        public static void AgreeProp(Proposal prop)
        {
            BookManager BM = BookManager.Instance;
            HandCardsManager HM = HandCardsManager.Instance;
            if (prop.InBook != -1)
            {
                Debug.Log("删除卡牌");
                BM.RemoveBook(prop.InBook);
            }
            
            if (prop.HandCard != null)
            {
                Debug.Log("添加卡牌");
                if (BookManager.IsBookFull())
                {
                    Debug.Log("协议书已满");
                    return;
                }
                HM.Add_Book(prop.HandCard);
                Debug.Log(prop.Player.PlayerName + "  " + CilentManager.playerdata.PlayerName);
                //if (prop.Player == CilentManager.playerdata)
                //{
                //    HM.Remove_HandCard(prop.HandCard);
                //}
            }
        }

        public static string GetPropType(Proposal prop)
        {
            string tip;
            if (prop.InHand == null)
            {
                tip = "删除";
            }
            else if (prop.InBook == -1)
            {
                tip = "添加";
            }
            else
            {
                tip = "替换";
            }
            return tip;
        }

        #region
        public static void GetPropFromNet(int inbook, int handcard, int bookcard, string name)
        {
            foreach (var pd in CilentManager.PDs)
            {
                if (pd.PlayerName == name)
                {
                    CardData hc = null, bc = null;
                    if (handcard != -1) hc = GameManager.Instance.CardsInLibarary[handcard];
                    if (bookcard != -1) bc = GameManager.Instance.CardsInLibarary[bookcard];
                    Proposal prop = new Proposal(inbook, hc, bc, pd);
                    if (PropsofthisTurn == null)
                        PropsofthisTurn = new List<Proposal>();
                    PropsofthisTurn.Add(prop);
                    break;
                }
            }
        }
        //投票结果
        #endregion

        public static void AgreeProp()
        {
            AgreeProp(Props_List[Props_List.Count - 1]);
        }
        //判断某个提案是否有效
        public static bool IsValid(Proposal prop)
        {
            if (prop.InHand == null)
            {
                if (BookManager.Instance.GetBookByNum(prop.InBook).GetComponent<CardMsg>().card == prop.BookCard)
                    return true;
                else return false;
            }
            else if (prop.InBook == -1)
            {
                //如果协议书满了，返回false
                //if()
                return true;
            }
            else
            {
                return true;
            }
        }

        public static string PrintProp()
        {
            string msg = "提案:";
            Proposal prop = Props_List[Props_List.Count - 1];
            if (prop.InHand == null)
            {
                int id = prop.BookCard.Get_Order;
                msg += string.Format("删除协议书#{0}\n", id);
            }
            else if (prop.InBook == -1)
            {
                int id = prop.HandCard.Get_Order;
                msg += string.Format("添加手牌#{0}到协议书\n", id);
            }
            else
            {
                int id1 = prop.BookCard.Get_Order;
                int id2 = prop.HandCard.Get_Order;
                msg += string.Format("替换协议书#{0}为手牌#{1}", id1, id2);
            }
            return msg;
        }
    }
    public class Proposal
    {
        public GameObject InHand;
        public int InBook;
        public CardData HandCard;
        public CardData BookCard;
        public PlayerGameData Player;
        //是否通过;回合数

        public Proposal(GameObject h, int b)
        {
            InHand = h;
            InBook = b;
            if (h != null) HandCard = h.GetComponent<CardMsg>().card;
            else HandCard = null;
            if (b != -1) BookCard = BookManager.Instance.GetBookByNum(b).GetComponent<CardMsg>().card;
            else BookCard = null;
            Player = CilentManager.playerdata;
        }

        public Proposal(int ib, CardData hc, CardData bc, PlayerGameData pd)
        {
            InBook = ib;
            HandCard = hc;
            BookCard = bc;
            Player = pd;
        }
    }
}