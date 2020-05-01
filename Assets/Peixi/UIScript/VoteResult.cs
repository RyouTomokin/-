using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Peixi
{
    public class VoteResult : MonoBehaviour
    {
        int voteRound = 0;
        VoteState vote;
        // Start is called before the first frame update
        void Start()
        {
            vote = FindObjectOfType<VoteState>();
            vote.onShowVoteResult += ShowVoteResult;
            vote.onRoundEnded += OnRoundEnd;
        }
        void ShowVoteResult()
        {
            Utility.AcitveAllChildren(transform);
            IEnumerator DelayEndVoteRound()
            {
                yield return new WaitForSeconds(4);
                voteRound++;
                if (voteRound>3)
                {
                    //end vote state
                    vote.RoundEndInvoke();
                    
                }
                else
                {
                    //continue voting
                    vote.EndVoteRound();
                    Utility.AcitveAllChildren(transform, false);
                }
            }
            StartCoroutine(DelayEndVoteRound());
        }
        void OnRoundEnd()
        {
            voteRound = 0;
        }
        void EndVoteRound()
        {

        }
    }
}

