using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Peixi
{
    public class VoteResult : MonoBehaviour
    {
        int voteRound = 0;
        VoteState voteState;
        Text negativeText;
        Text positiveText;
        Text resultText;
        // Start is called before the first frame update
        void Start()
        {
            voteState = FindObjectOfType<VoteState>();
            voteState.onShowVoteResult += ShowVoteResult;
            voteState.onRoundEnded += OnRoundEnd;
            negativeText = transform.Find("negativeTicket").GetComponent<Text>();
            positiveText = transform.Find("positiveTicket").GetComponent<Text>();
            resultText = transform.Find("result").GetComponent<Text>();
        }
        void ShowVoteResult(Vote result)
        {
            Utility.AcitveAllChildren(transform);
            //show result content
            negativeText.text = result.negativeVote.ToString();
            positiveText.text = result.positiveVote.ToString();
            if (result.negativeVote > result.positiveVote)
            {
                resultText.text = "提案未通过";
            }
            else
            {
                resultText.text = "提案通过";
            }

            IEnumerator DelayEndVoteRound()
            {
                yield return new WaitForSeconds(4);
                voteRound++;
                if (voteRound>3)
                {
                    //end vote state
                    voteState.RoundEndInvoke();
                    
                }
                else
                {
                    //continue voting
                    voteState.EndVoteRound();
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

