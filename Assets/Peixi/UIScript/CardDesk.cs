using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public class CardDesk : MonoBehaviour
    {
        VoteState voteState;
        [SerializeField]
        GameObject[] cardStyle = new GameObject[3];

        int voteRound = 0;
        private void Start()
        {
            voteState = FindObjectOfType<VoteState>();
            voteState.onVoteRoundStart += OnVoteRoundStart;
            voteState.onVoteRoundEnd += OnVoteRoundEnd;
        }

        void ShowCardStyle(int n)
        {
            for (int i = 0; i < 3; i++)
            {
                cardStyle[i].SetActive(false);
            }
            cardStyle[n].SetActive(true);
        }

        void OnVoteRoundStart(List<Bill> bills)
        {
            string action = bills[voteRound].action;
            if (action == "Add")
            {
                ShowCardStyle(2);
            }
            else if (action == "Delete")
            {
                ShowCardStyle(1);
            }
            else if (action == "Replace")
            {
                ShowCardStyle(0);
            }
            voteRound++;
        }

        void OnVoteRoundEnd()
        {
            voteRound = 0;
        }
    }
}

