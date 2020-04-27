using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public class AccountState : RoundState
    {
        [SerializeField]
        protected GameObject accountFrame;

        private void Start()
        {
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }
        protected override void OnRoundStart()
        {
            print("开始结算阶段");
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束结算阶段，等待其他玩家");
            StartCoroutine(RoundInterval());
            accountFrame.SetActive(false);
        }
        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            FindObjectOfType<PrepareStateEvent>().RoundStartInvoke();
        }
    }
}

