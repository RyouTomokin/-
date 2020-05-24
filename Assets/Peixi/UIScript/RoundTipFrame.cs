using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Peixi
{
    public class RoundTipFrame : MonoBehaviour
    {
        public Text roundText;
        public Text stateText;

        int round = -1;
        int state = -1;
        string[] stateName = { "准备阶段", "提案阶段", "协商阶段", "投票阶段", "结算阶段" };
        public int Round
        {
            set
            {
                if (value < 6)
                {
                    round = value;
                    roundText.text = round.ToString();
                }
            }
            get { return round; }
        }
        public int State
        {
            set
            {
                state = value;

                state = (state > 5) ? 0 : state;
                stateText.text = stateName[state];

            }
            get { return state; }
        }

        PrepareStateEvent prepare;
        ProposalStateEvent proposal;
        NegociateState negociate;
        VoteState vote;
        AccountState account;

        // Start is called before the first frame update
        void Start()
        {
            prepare = FindObjectOfType<PrepareStateEvent>();
            proposal = FindObjectOfType<ProposalStateEvent>();
            negociate = FindObjectOfType<NegociateState>();
            vote = FindObjectOfType<VoteState>();
            account = FindObjectOfType<AccountState>();

            prepare.onRoundStarted += () => State++;
            proposal.onRoundStarted += () => State++;
            negociate.onRoundStarted += () => State++;
            vote.onVoteRoundStart += () => State++;
            account.onRoundStart += (Score[] m) => State++;

            account.onRoundEnded += () => Round++;

            Round++;
        }
    }
}


