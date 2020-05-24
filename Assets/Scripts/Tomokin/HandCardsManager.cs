using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tomokin
{
    public class HandCardsManager : MonoBehaviour
    {
        public static HandCardsManager Instance;

        public List<GameObject> Cards;
        private GameManager GM;
        public Dropdown drop;

        public static class Ploy
        {
            public static int[] PJ = new int[] { 0, 1, 1, 2, 2 };
            public static int[] JH = new int[] { 0, 0, 1, 1, 2 };
            public static int[] WH = new int[] { 0, 0, 0, 1, 2 };
        }

        public void Add_Book_Prop(GameObject hand)
        {
            ProposalManager.AddProp(hand, -1);
            //FindObjectOfType<TextInputManager>().SendMsg(CilentManager.PlayerName + "添加添加协议的提案");
        }

        /// <summary>
        /// 添加协议书（按钮）查看是否有空协议书，有则添加手牌到协议书
        /// </summary>
        /// <param name="hand">手牌</param>
        public void Add_Book(CardData cd)
        {
            Debug.Log("Add_Book");
            GameObject book = null;
            foreach (GameObject b in BookManager.Books)
            {
                if (!b.activeSelf)
                {
                    book = b;
                    break;
                }
            }
            if (book == null)
            {
                Debug.Log("协议书已满");
                return;
            }
            //找到协议书
            book.SetActive(true);
            //book.transform.SetAsLastSibling();
            book.GetComponent<CardMsg>().card = cd;
            //book.GetComponent<Image>().sprite = book.GetComponent<CardMsg>().card.icon;

            GameManager.InputCardMsg(book);

            
        }

        public void Remove_HandCard(CardData cd)
        {
            foreach (var c in Cards)
            {
                CardData cd_in_c = c.GetComponent<CardMsg>().card;
                Debug.Log("cd_in_c:" + cd_in_c.Get_Order);
                if (cd_in_c == cd)
                {
                    GameObject hand = c;
                    hand.SetActive(false);
                    hand.GetComponent<CardMsg>().card = null;
                    hand.transform.SetAsLastSibling();
                    return;
                }
            }
            Debug.Log("未找到手牌");
        }

        private void Start()
        {
            Instance = this;
            GM = GameManager.Instance;
        }
    }
}