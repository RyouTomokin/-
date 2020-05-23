using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tomokin;

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
        void OnVoteRoundStart()
        {
            //List<Bill> bills = voteState.PlayerBills;
            Proposal prop = CilentManager.PropNeedVote;
            if (prop != null)
            {
                Utility.AcitveAllChildren(transform);
                string name = TomokinNet.OnlyName(prop.Player.PlayerName);
                billboard.text = "玩家" + name + "的提案是:";
                string tip;
                if (prop.HandCard == null) tip = "删除";
                else if (prop.BookCard == null) tip = "添加";
                else tip = "替换";
                content.text = tip + "卡牌";
                voteRound++;
            }
            else
            {
                throw new System.Exception("没有初始化bill");
            }

        }
        void OnVoteRoundEnd()
        {
            voteRound = 0;
            Utility.AcitveAllChildren(transform, false);
        }
    }
}
    


