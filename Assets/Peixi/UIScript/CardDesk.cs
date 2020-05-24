using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tomokin;

namespace Peixi
{
    public class CardDesk : MonoBehaviour
    {
        VoteState voteState;
        [SerializeField]
        GameObject[] cardStyle = new GameObject[3];

        int voteRound = 0;

        private void OnEnable()
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

        void OnVoteRoundStart()
        {
            //Debug.Log("输入卡牌信息");
            Proposal prop = CilentManager.PropNeedVote;
            if (prop.HandCard != null)
            {
                //Debug.Log("手牌信息");
                cardStyle[0].SetActive(true);
                cardStyle[0].GetComponent<CardMsg>().card = prop.HandCard;
                GameObject CardAnim = cardStyle[0].transform.Find("CardAnim").gameObject;
                GameManager.InputCardMsg(cardStyle[0]);
            }
            else cardStyle[0].SetActive(false);

            if (prop.BookCard != null)
            {
                //Debug.Log("协议书信息");
                cardStyle[1].SetActive(true);
                cardStyle[1].GetComponent<CardMsg>().card = prop.BookCard;
                GameObject CardAnim = cardStyle[1].transform.Find("CardAnim").gameObject;
                GameManager.InputCardMsg(cardStyle[1]);
            }
            else cardStyle[1].SetActive(false);
        }

        void OnVoteRoundEnd()
        {
            voteRound = 0;
        }
    }
}

