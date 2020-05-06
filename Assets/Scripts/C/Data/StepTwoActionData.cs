using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace C
{
    public class StepTwoActionData : ActionData
    {
        public StepTwoActionData(string action_owner_nickname, int step_num, string action_tip,int hand_card,int agreement_id,int agreement_card,string owner_nickname)
        {
            this.action_owner_nickname = action_owner_nickname;
            this.step_num = step_num;
            this.action_tip = action_tip;
            this.hand_card = hand_card;
            this.agreement_id = agreement_id;
            this.agreement_card = agreement_card;
            this.owner_nickname = owner_nickname;
            this.poll = 0;
        }
        //行为标识 增加，删除，替换
        public string action_tip;
        //手牌卡牌
        public int hand_card;
        //协议书编号
        public int agreement_id;
        //协议卡牌
        public int agreement_card;
        //提出者名称
        public string owner_nickname;
        //投票数
        public float poll;
    }
}
