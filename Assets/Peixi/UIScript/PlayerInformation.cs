using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Peixi
{
    public class PlayerInformation : MonoBehaviour
    {
        protected int chip = 2;
        protected int gcoin = 2;
        protected string playerName = "Player1";
        public int Chip
        {
            get { return chip; }
        }
        public int Gcoin
        {
            get { return gcoin; }
        }
        PrepareStateEvent prepare;
        [SerializeField]
        GameObject clientPlayer;

        // Start is called before the first frame update
        void Start()
        {
            prepare = FindObjectOfType<PrepareStateEvent>();
            prepare.onRoundStarted += OnRoundStart;
            prepare.onRollCard += OnRollCard;
            prepare.approveBribe += OnAcceptBribeButtonPressed;
            prepare.bribeRequestResultReceived += OnBribeRequestResultReceived;
        }
        void OnRoundStart()
        {
            print("PlayerInformation connect well");
        }
        void OnRollCard()
        {
            chip -= 2;
            Text chipLabel = clientPlayer.transform.Find("chip").GetComponent<Text>();
            chipLabel.text = chip.ToString();
        }
        void OnBribeRequestResultReceived(bool m_result)
        {
            if (m_result)
            {
                gcoin -= 2;
                Text coinLabel = clientPlayer.transform.Find("chip").GetComponent<Text>();
                coinLabel.text = gcoin.ToString();
            }
        }
        void OnAcceptBribeButtonPressed()
        {
            gcoin += 2;
            Text coinLabel = clientPlayer.transform.Find("gcoin").GetComponent<Text>();
            coinLabel.text = gcoin.ToString();
        }
        public void UpdateServerData()
        {

        }
    }
}

