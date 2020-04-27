using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Peixi
{
    public class VoteFrame : MonoBehaviour
    {
        VoteState voteState;
        [SerializeField]
        Text billboard;
        [SerializeField]
        Text content;

        int voteRound = 0;
        private void Awake()
        {
            voteState = FindObjectOfType<VoteState>();
            voteState.onVoteRoundStart += OnVoteRoundStart;
            voteState.onVoteRoundEnd += OnVoteRoundEnd;
        }
        void OnVoteRoundStart(List<Bill> bills)
        {
            Utility.AcitveAllChildren(transform);
            billboard.text = "玩家" + bills[voteRound].name + "的提案是:";
            content.text = bills[voteRound].action + bills[voteRound].card1;
            voteRound++;
        }
        void OnVoteRoundEnd()
        {
            voteRound = 0;
            Utility.AcitveAllChildren(transform, false);
        }
        public void OnAgreeButtonPressed()
        {

        }
        public void OnDisagreeButtonPressed()
        {

        }
    }
}
    


