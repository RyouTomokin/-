using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Peixi
{
    public class UseTicket : MonoBehaviour
    {
        VoteState vote;
        private void Start()
        {
            vote = FindObjectOfType<VoteState>();
            vote.onAgreeVoteSent += StartUseTicketFrame;
            vote.onDisagreeVoteSent += StartUseTicketFrame;
        }
        public void ConfirmUseTicket()
        {
            vote.InvokeUseTicket();
            Utility.AcitveAllChildren(transform, false);
        }
        public void CancelUseTicket()
        {
            Utility.AcitveAllChildren(transform, false);
        }
        void StartUseTicketFrame()
        {
            Utility.AcitveAllChildren(transform);
        }
    }
}

