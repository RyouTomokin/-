using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomokin
{
    public class ProposalManager
    {
        public static List<Proposal> Props_List = new List<Proposal>();
        private static Proposal TempProp;

        

        public static void AddProp(GameObject hand, GameObject book)
        {
            TempProp = new Proposal(hand, book);
            Props_List.Add(TempProp);
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

        public static void AgreeProp()
        {
            AgreeProp(Props_List[Props_List.Count - 1]);
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
            else if (prop.InBook == null)
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
        public GameObject InBook;
        public CardData HandCard;
        public CardData BookCard;
        //卡牌本身信息；是否通过；回合数

        public Proposal(GameObject h, GameObject b)
        {
            InHand = h;
            InBook = b;
            if (h != null) HandCard = h.GetComponent<CardMsg>().card;
            if (b != null) BookCard = b.GetComponent<CardMsg>().card;
        }
    }
}