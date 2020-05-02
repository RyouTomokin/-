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
                    ProposalManager.AddProp(null, i);
            }
        }

        public void Add_Card_To_Book(int pos, int card)
        {
            Books = BookCards;
            Books[pos].SetActive(true);
            Books[pos].GetComponent<CardMsg>().card = GM.CardsInLibarary[card];
            Books[pos].GetComponent<Image>().sprite = GM.CardsInLibarary[card].icon;
        }
        //通过协议书槽的序号找到协议书
        public static GameObject GetBookByNum(int n)
        {
            if (n < Books.Length)
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

        public static void LockChange()
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
            isReplace = true;
        }

        private void Start()
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