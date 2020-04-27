using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Peixi
{
    public class ProposalStateEvent : RoundState
    {
        PlayableDirector director;
        private void Start()
        {
            director = GetComponent<PlayableDirector>();
            onRoundStarted += OnRoundStart;
            onRoundEnded += OnRoundEnd;
        }
        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            print("开始提案阶段");
        }
        protected override void OnRoundEnd()
        {
            base.OnRoundEnd();
            print("结束提案阶段，等待其他玩家");
            director.Play();
            StartCoroutine(RoundInterval());
        }

        IEnumerator RoundInterval()
        {
            yield return new WaitForSeconds(2);
            FindObjectOfType<NegociateState>().RoundStartInvoke();
        }
    }
}
