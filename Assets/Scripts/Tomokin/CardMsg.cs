using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Tomokin
{
    /// <summary>
    /// 显示该协议书或者手牌的对应的菜单
    /// </summary>
    public class CardMsg : MonoBehaviour
    {
        private GameManager GM;
        private BookManager BM;
        private HandCardsManager HM;

        public GameObject obj;  //手牌的菜单栏
        public CardData card;

        private void Start()
        {
            GM = GameManager.Instance;
            if (card != null)
            {
                //obj.transform.parent.gameObject.GetComponent<Image>().sprite = card.icon;
                gameObject.GetComponent<Image>().sprite = card.icon;
                card.Get_IsInLib = false;
            }
        }
        //显示协议书的菜单
        public void Show()
        {
            GM.Show_Replace_Remove(obj);
        }
        //显示手牌的菜单（若为替换模式，则将手牌添加到协议书）
        public void ShowAdd()
        {
            //不是替换模式则显示菜单栏
            if (!BM.isReplace)
                GM.Show_Add(obj);
            else
            {
                //传递数据
                //GM.Player.GetComponent<ActionData>().Proposal[0] = gameObject;
                //GM.Player.GetComponent<ActionData>().Proposal[1] = BM.Bereplace;

                //Debug.Log("替换提案：" + BM.Bereplace.name + "为提案：" + gameObject.name);
                //BM.RemoveBook(BM.Bereplace, true);    //删除被替换的协议书
                //HM.Add_Book(gameObject, true);        //添加手牌到协议书
                ProposalManager.AddProp(gameObject, BM.Bereplace);
                BM.isReplace = false;
            }
        }


        private void Update()
        {
            if (GM == null)
                GM = GameManager.Instance;
            if (HM == null)
                HM = HandCardsManager.Instance;
            if (BM == null)
                BM = BookManager.Instance;
            //if (card != null)
            //{
            //    obj.transform.parent.gameObject.GetComponent<Image>().sprite = card.icon;
            //}
        }
    }
}