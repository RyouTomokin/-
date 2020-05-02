using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace C
{
    public class StepThreeActionData : ActionData
    {
        public StepThreeActionData(string action_owner_nickname, int step_num,bool isBuyOther)
        {
            this.action_owner_nickname = action_owner_nickname;
            this.step_num = step_num;
            this.isBuyOther = isBuyOther;
        }
        //是否购买额外一票
        public bool isBuyOther;
    }
}