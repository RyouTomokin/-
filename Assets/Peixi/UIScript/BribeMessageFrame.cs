using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tomokin;

namespace Peixi
{
    public class BribeMessageFrame : MonoBehaviour
    {
        PrepareStateEvent prepareState;
        Animation anim;
        string bribeTaker;
        public int playerNum;
        public Text contet_txt;
        private void Start()
        {
            prepareState = FindObjectOfType<PrepareStateEvent>();
            anim = GetComponent<Animation>();
            Utility.AcitveAllChildren(transform,false);
            prepareState.bribeMessageReceived += OnBribeMessageReceived;
            contet_txt = transform.Find("contet").GetComponent<Text>();
        }
        void OnBribeMessageReceived(int player)
        {
            int n = (CilentManager.PlayerNum + playerNum) % 3;
            if (n == player)
            {
                Utility.AcitveAllChildren(transform, true);
                bribeTaker = CilentManager.PDs[n].PlayerName;
                Debug.Log(bribeTaker+ "希望花费2G币与您达成私下和解");
                contet_txt.text = bribeTaker + "希望花费2G币与您达成私下和解。";
            }
            //anim.Play();
            //print("弹出悄悄话信息框");
        }
        public void OnApproveButtonPressed()
        {
            prepareState.InvokeApproveBribe(bribeTaker);
            Utility.AcitveAllChildren(transform, false);
        }
        public void OnRejectButtonPressed()
        {
            string m_name = Tomokin.CilentManager.playerdata.PlayerName;
            //print("reject bribe button pressed");
            prepareState.InvokeRejectBribe(bribeTaker);
            Utility.AcitveAllChildren(transform, false);
        }
    }
}


