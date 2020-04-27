using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tomokin
{
    public class BookManager : MonoBehaviour
    {
        public static BookManager Instance;
        public GameManager GM;
        public bool isReplace = false;      //替换模式
        public GameObject Bereplace;        //被替换的卡牌对象

        public void Add_Book_Prop(GameObject book)
        {
            ProposalManager.AddProp(null, book);
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
        /// <summary>
        /// 切换为替换模式（按钮）
        /// </summary>
        /// <param name="obj">协议书对象</param>
        public void RepalceMode(GameObject obj)
        {
            Bereplace = obj;
            isReplace = true;
        }

        private void Start()
        {
            Instance = this;
            GM = GameManager.Instance;
        }
    }
}