using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Peixi
{
    /// <summary>
    /// 管理玩家的信息显示
    /// </summary>
    public struct PlayerData
    {
        public string name;
        public int chip;
        public int coin;
    }
    public class PlayerInformation : MonoBehaviour
    {
        public static PlayerInformation instance;
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
        GameObject[] players;
        // Start is called before the first frame update
        void Start()
        {
            instance = this;
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
            Text chipLabel = players[0].transform.Find("chip").GetComponent<Text>();
            chipLabel.text = chip.ToString();
        }
        void OnBribeRequestResultReceived(bool m_result)
        {
            if (m_result)
            {
                gcoin -= 2;
                Text coinLabel = players[0].transform.Find("chip").GetComponent<Text>();
                coinLabel.text = gcoin.ToString();
            }
        }
        void OnAcceptBribeButtonPressed()
        {
            gcoin += 2;
            Text coinLabel = players[0].transform.Find("gcoin").GetComponent<Text>();
            coinLabel.text = gcoin.ToString();
        }
        /// <summary>
        /// 从服务器端更新所有玩家信息显示
        /// </summary>
        /// <param name="data">默认data[0]是本地玩家</param>
        public void UpdatePlayerData(List<PlayerData> data)
        {
            for (int i = 0; i < 3; i++)
            {
                Text name = players[i].transform.Find("name").GetComponent<Text>();
                Text chip = players[i].transform.Find("chip").GetComponent<Text>();
                Text gcoin = players[i].transform.Find("gcoin").GetComponent<Text>();
                name.text = data[i].name;
                chip.text = data[i].chip.ToString();
                gcoin.text = data[i].chip.ToString();
            }
            chip = data[0].chip;
            gcoin = data[0].coin;
        }
    }
}

