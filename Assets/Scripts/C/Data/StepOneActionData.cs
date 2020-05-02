using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace C
{
    public class StepOneActionData : ActionData
    {
        public StepOneActionData(string action_owner_nickname, int step_num, string action_tip,string tar_player_nickname)
        {
            this.action_owner_nickname = action_owner_nickname;
            this.step_num = step_num;
            this.action_tip = action_tip;
            this.tar_player_nickname = tar_player_nickname;
        }
        //行为标识 贿赂，接受贿赂，更换手牌，啥都不做
        public string action_tip;
        public string tar_player_nickname;

    }
}
