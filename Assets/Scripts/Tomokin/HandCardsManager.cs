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
        }

        /// <summary>
        /// 添加协议书（按钮）查看是否有空协议书，有则添加手牌到协议书
        /// </summary>
        /// <param name="hand">手牌</param>
        public void Add_Book(GameObject hand, bool isReplace)
        {
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
            book.GetComponent<CardMsg>().card = hand.GetComponent<CardMsg>().card;
            //book.GetComponent<Image>().sprite = book.GetComponent<CardMsg>().card.icon;
            int i = 0;
            foreach (var text in book.GetComponentsInChildren<Text>())
            {
                int v = book.GetComponent<CardMsg>().card.GetByNum(i);
                if (v > 0) text.text = "+" + v;
                else text.text = v.ToString();
                i++;
            }

            hand.SetActive(false);
            hand.GetComponent<CardMsg>().card = null;
            hand.transform.SetAsLastSibling();
        }
        public void Add_Book(GameObject hand)
        {
            Add_Book(hand, false);
        }

        private void Start()
        {
            Instance = this;
            GM = GameManager.Instance;
        }
    }
}