using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace C
{
    public class StepForthActionData : ActionData
    {
        public StepForthActionData(string action_owner_nickname,int step_num,string description)
        {
            this.action_owner_nickname = action_owner_nickname;
            this.step_num = step_num;
            this.description = description;
        }
        //描述
        public string description;
    }
}
