using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
namespace Peixi
{
    public class Timer : MonoBehaviour
    {
        Text timerNumber;
        Button button;
        Animation anim;
        public int duration = 60;
        int time;
        public PlayableDirector directer;

        private enum RoundStateEnum
        {
            PrepareState,
            ProposalState,
            NegociateState,
            VoteState,
            AccountState
        }

        RoundStateEnum roundState = RoundStateEnum.PrepareState;
        // Start is called before the first frame update

        public RoundState round;
        void Start()
        {
            timerNumber = GetComponentInChildren<Text>();
            button = GetComponent<Button>();
            anim = GetComponent<Animation>();


            FindObjectOfType<PrepareStateEvent>().onRoundStarted += OnRoundStart;
            FindObjectOfType<ProposalStateEvent>().onRoundStarted += OnRoundStart;
            FindObjectOfType<NegociateState>().onRoundStarted += OnRoundStart;
            //FindObjectOfType<VoteState>().onRoundStarted += OnRoundStart;
            FindObjectOfType<AccountState>().onRoundStarted += OnRoundStart;

        }

        void OnRoundStart()
        {
            //print("reset end button");
            button.interactable = true;
        }
        void ResetTimer()
        {
            time = duration;
            button.interactable = true;
            StartCoroutine(TimeCountDown());
        }
        public void OnbuttonPressed()
        {
            //print("End button pressed");
            button.interactable = false;
            //anim.Play();
            //button.interactable = false;
            //timerNumber.text = "00";
            switch (roundState)
            {
                case RoundStateEnum.PrepareState:
                    FindObjectOfType<PrepareStateEvent>().RoundEndInvoke();
                    roundState = RoundStateEnum.ProposalState;
                    break;
                case RoundStateEnum.ProposalState:
                    FindObjectOfType<ProposalStateEvent>().RoundEndInvoke();
                    roundState = RoundStateEnum.NegociateState;
                    break;
                case RoundStateEnum.NegociateState:
                    FindObjectOfType<VoteState>().RoundEndInvoke();
                    roundState = RoundStateEnum.VoteState;
                    break;
                case RoundStateEnum.VoteState:
                    FindObjectOfType<AccountState>().RoundEndInvoke();
                    roundState = RoundStateEnum.AccountState;
                    break;
                case RoundStateEnum.AccountState:
                    FindObjectOfType<PrepareStateEvent>().RoundEndInvoke();
                    roundState = RoundStateEnum.PrepareState;
                    break;
                default:
                    break;
            }
        }
        IEnumerator TimeCountDown()
        {
            timerNumber.text = time.ToString();
            yield return new WaitForSeconds(1);
            time -= 1;
            if (time <= 0)
            {
                OnbuttonPressed();
            }
            else
            {
                StartCoroutine(TimeCountDown());
            }
        }
    }
}