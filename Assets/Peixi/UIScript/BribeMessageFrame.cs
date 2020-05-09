using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public class BribeMessageFrame : MonoBehaviour
    {
        PrepareStateEvent prepareState;
        Animation anim;
        string bribeTaker;
        public int playerNum;
        private void Start()
        {
            prepareState = FindObjectOfType<PrepareStateEvent>();
            anim = GetComponent<Animation>();
            Utility.AcitveAllChildren(transform,false);
            prepareState.bribeMessageReceived += OnBribeMessageReceived;
        }
        void OnBribeMessageReceived(string player)
        {

            bribeTaker = player;
            if (player== bribeTaker)
            {
                Utility.AcitveAllChildren(transform, true);
            }
            //anim.Play();
            //print("弹出悄悄话信息框");
        }
        public void OnApproveButtonPressed()
        {
            string m_name = Tomokin.CilentManager.playerdata.PlayerName;
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


