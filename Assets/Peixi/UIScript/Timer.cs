using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
namespace Peixi
{
    public class Timer : MonoBehaviour
    {
        Text timeText;
        Button button;
        Animation anim;
        public int duration = 60;
        int time;
        bool runningState = false;
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

        public int Time
        {
            get { return time;}
            set
            {
                if (value != time)
                {
                    time = value;
                    timeText.text = time.ToString();
                    if (time == 0)
                    {
                        OnbuttonPressed();
                    }
                }
            }
        }
        //public bool RunningState
        //{
        //    get { return runningState; }
        //    set
        //    {
        //        if (value != runningState)
        //        {
        //            OnbuttonPressed();
        //        }
        //    }
        //}

        public PrepareStateEvent prepare;
        public ProposalStateEvent proposal;
        public NegociateState negociate;
        public VoteState vote;
        public AccountState account;
        // Start is called before the first frame update
        //public RoundState round;
        void Start()
        {
            timeText = GetComponentInChildren<Text>();
            button = GetComponent<Button>();
            anim = GetComponent<Animation>();

            prepare.onRoundStarted += StartRound;
            proposal.onRoundStarted += StartRound;
            negociate.onRoundStarted += StartRound;
            vote.onVoteRoundStart += StartRound;
            account.onRoundStart += StartRound;

            StartRound();
        }
        void StartRound()
        {
            button.interactable = true;
            StartCoroutine(TimeCountDown());
            Time = duration;
        }
        void StartRound(Score[] scores)
        {
            roundState = RoundStateEnum.AccountState;
            StartRound();
        }
        /// <summary>
        /// End time count down 
        /// </summary>
        public void OnbuttonPressed()
        {
            button.interactable = false;
            StopAllCoroutines();
            Time = 0;
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
                    FindObjectOfType<NegociateState>().RoundEndInvoke();
                    roundState = RoundStateEnum.VoteState;
                    break;
                case RoundStateEnum.VoteState:
                    FindObjectOfType<VoteState>().RoundEndInvoke();
                    roundState = RoundStateEnum.AccountState;
                    break;
                case RoundStateEnum.AccountState:
                    account.RoundEndInvoke();
                    roundState = RoundStateEnum.PrepareState;
                    break;
                default:
                    break;
            }
        }
        IEnumerator TimeCountDown()
        {
            yield return new WaitForSeconds(1);
            Time -= 1;
            if (Time>0)
            {
                StartCoroutine(TimeCountDown());
            }
        }
    }
}