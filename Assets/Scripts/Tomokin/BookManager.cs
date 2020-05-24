using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Tomokin
{
    public class BookManager : MonoBehaviour
    {
        public static BookManager Instance;
        public GameManager GM;
        public bool isReplace = false;      //替换模式
        public GameObject Bereplace;        //被替换的卡牌对象
        public int Bereplace_int;
        public GameObject[] BookCards;
        public static GameObject[] Books = new GameObject[6];
        public static List<GameObject> UnlockedBooks = new List<GameObject>();

        public void Add_Book_Prop(GameObject book)
        {
            for (int i = 0; i < 6; i++)
            {
                if (book == Books[i])
                {
                    ProposalManager.AddProp(null, i);
                    FindObjectOfType<TextInputManager>().SendMsg(CilentManager.PlayerName + "添加删除协议的提案");
                    break;
                }
            }
        }
        /// <summary>
        /// 添加到协议书
        /// </summary>
        /// <param name="pos">协议书槽</param>
        /// <param name="card">卡牌id</param>
        public void Add_Card_To_Book(int pos, int card)
        {
            Books = BookCards;
            Books[pos].SetActive(true);
            Books[pos].GetComponent<CardMsg>().card = GM.CardsInLibarary[card];
            //Books[pos].GetComponent<Image>().sprite = GM.CardsInLibarary[card].icon;
            int i = 0;
            foreach (var text in BookCards[pos].GetComponentsInChildren<Text>())
            {
                int v = BookCards[pos].GetComponent<CardMsg>().card.GetByNum(i);
                if (v > 0) text.text = "+" + v;
                else text.text = v.ToString();
                i++;
            }
        }
        //通过协议书槽的序号找到协议书
        public GameObject GetBookByNum(int n)
        {
            Debug.Log("n = " + n);
            Books = BookCards;
            if (n < Books.Length && n >= 0)
                return Books[n];
            else return null;
        }

        public static int GetNumByBook(GameObject o)
        {
            for (int i = 0; i < Books.Length; i++)
            {
                if (o == Books[i]) return i;
            }
            return -1;
        }
        //判断是否所有协议书都被使用了
        public static bool IsBookFull()
        {
            foreach (var b in UnlockedBooks)
            {
                if (!b.activeSelf) return true;
            }
            return false;
        }

        public static void UnlockChange()
        {
            if (Books.Length == 6)
            {
                UnlockedBooks.Clear();
                for (int i = 2; i < 6; i++)
                {
                    UnlockedBooks.Add(Books[i]);
                }
            }
        }

        /// <summary>
        /// 同意删除协议书的提案（按钮）今后会被调用
        /// </summary>
        /// <param name="obj">协议书对象</param>
        public void RemoveBook(GameObject obj, bool isReplace)
        {
            if (obj == null) return;
            obj.SetActive(false);
            obj.GetComponent<CardMsg>().card = null;
        }
        public void RemoveBook(GameObject obj)
        {
            RemoveBook(obj, false);
        }
        public void RemoveBook(int obj)
        {
            RemoveBook(GetBookByNum(obj), false);
        }
        /// <summary>
        /// 切换为替换模式（按钮）
        /// </summary>
        /// <param name="obj">协议书对象</param>
        public void RepalceMode(GameObject obj)
        {
            Bereplace = obj;
            Bereplace_int = GetNumByBook(obj);
            Debug.Log("替换模式启动");
            isReplace = true;
        }

        private void OnEnable()
        {
            Instance = this;
            GM = GameManager.Instance;
            Books = BookCards;
            if (Books.Length == 6)
            {
                for (int i = 0; i < 4; i++)
                {
                    UnlockedBooks.Add(Books[i]);
                }
            }

        }
    }
}