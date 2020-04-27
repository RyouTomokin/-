using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public class BribeMessageFrame : MonoBehaviour
    {
        PrepareStateEvent prepareState;
        Animation anim;
        
        private void Start()
        {
            prepareState = FindObjectOfType<PrepareStateEvent>();
            anim = GetComponent<Animation>();
            prepareState.bribeMessageReceived += OnBribeMessageReceived;
            //prepareState.approveBribe += OnApproveButtonPressed;
            //prepareState.rejectBribe += OnApproveButtonPressed;
        }

        void OnBribeMessageReceived()
        {
            Utility.AcitveAllChildren(transform, true);
            anim.Play();
            //print("弹出悄悄话信息框");
        }

        public void OnApproveButtonPressed()
        {
            prepareState.InvokeApproveBribe();
            Utility.AcitveAllChildren(transform, false);
        }
        
        public void OnRejectButtonPressed()
        {
            //print("reject bribe button pressed");
            prepareState.InvokeRejectBribe();
            Utility.AcitveAllChildren(transform, false);
        }
    }
}


