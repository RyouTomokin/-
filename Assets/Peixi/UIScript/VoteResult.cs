using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Peixi
{
    public class VoteResult : MonoBehaviour
    {
        VoteState vote;
        // Start is called before the first frame update
        void Start()
        {
            vote = FindObjectOfType<VoteState>();
            vote.onVoteResultReceived += ShowVoteResult;
        }
        void ShowVoteResult()
        {
            Utility.AcitveAllChildren(transform);
            IEnumerator DelayEndVoteRound()
            {
                yield return new WaitForSeconds(4);
                vote.RoundEndInvoke();
            }
            StartCoroutine(DelayEndVoteRound());
        }
    }
}

