using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tomokin
{
    public class EnemyEvent : MonoBehaviour
    {
        [SerializeField]
        private GameObject MessageLabel;
        public static GameObject SelectedObj = null;
        /// <summary>
        /// 显示对手信息
        /// </summary>
        [System.Obsolete]
        //private void OnMouseDown()
        //{
        //    if (MessageLabel.active && SelectedObj == gameObject)
        //    {
        //        MessageLabel.SetActive(false);
        //        SelectedObj = null;
        //    }
        //    else
        //    {
        //        MessageLabel.SetActive(true);
        //        SelectedObj = gameObject;
        //        ShowMsg();
        //    }
        //}

        //public void ShowMsg()
        //{
        //    ActionData A_Data = SelectedObj.GetComponent<ActionData>();
        //    MessageLabel.transform.position = SelectedObj.transform.position + (Vector3.up * 2);
        //    MessageLabel.transform.rotation = SelectedObj.transform.rotation;
        //    Text txt = MessageLabel.GetComponentInChildren<Text>();
        //    txt.text = string.Format("钱袋 = {0}" +
        //        "\n筹码 = {1}" +
        //        "\n选票 = {2}",
        //        A_Data.P_Data.GetMoney, A_Data.P_Data.GetChip, A_Data.P_Data.GetVote);
        //    if (A_Data.GetBeBribed != null)
        //    {
        //        txt.text += string.Format("\n被{0}贿赂", A_Data.GetBeBribed.name);
        //    }
        //}

        public void Start()
        {
        }
    }
}