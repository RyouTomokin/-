using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tomokin;
using Peixi;

public class ProposalSequence : MonoBehaviour
{
    public Text[] proposals;

    VoteState vote;
    private void Start()
    {
        Utility.AcitveAllChildren(transform, false);
        vote = FindObjectOfType<VoteState>();

        vote.onVoteRoundStart += () =>
        {
            Utility.AcitveAllChildren(transform, true);
            for (int i = 0; i < 3; i++)
            {
                string name = ProposalManager.PropsofthisTurn[i].Player.PlayerName;
                proposals[i].text = name;
            }
        };

        vote.onRoundEnded += () =>
        {
            Utility.AcitveAllChildren(transform, false);
        };
    }
}
