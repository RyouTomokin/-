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
            //Props_List.Add(TempProp);             //应当把这个回合的提案一起添加
        }

        public static void AgreeProp(Proposal prop)
        {
            BookManager BM = BookManager.Instance;
            HandCardsManager HM = HandCardsManager.Instance;
            if (prop.InBook != null)
            {
                BM.RemoveBook(prop.InBook);
            }
            if (prop.InHand != null)
            {
                HM.Add_Book(prop.InHand);
            }
        }
        #region
        //服务器生成提案列表（顺序为玩家的积分升序排列）
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
                if (BookManager.GetBookByNum(prop.InBook).GetComponent<CardMsg>().card == prop.BookCard)
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
            if (b != -1) BookCard = BookManager.GetBookByNum(b).GetComponent<CardMsg>().card;
            Player = GameManager.PD;
        }
    }
}