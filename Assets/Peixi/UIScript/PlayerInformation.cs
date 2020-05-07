using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using Tomokin;

namespace Peixi
{
    /// <summary>
    /// 玩家信息
    /// </summary>
    public struct PlayerData
    {
        public string name;
        public int chip;
        public int coin;

        public PlayerData(string m_name,int m_chip,int m_coin)
        {
            name = m_name;
            chip = m_chip;
            coin = m_coin;
        }
        public void ChangeChip(int changeValue)
        {
            chip += changeValue;
        }
        public void ChangeCoin(int changeValue)
        {
            coin += changeValue;
        }
    }
   
    public class PlayerInformation : MonoBehaviour
    {
        [SerializeField]
        GameObject[] players;//3 players' UI gameobject
        Dictionary<string, PlayerData> playerData = new Dictionary<string, PlayerData>();

        public static PlayerInformation instance;
        protected int chip = 2;
        protected int gcoin = 2;
        protected string clientPlayer = "Player1";
        protected bool bribed;
        protected bool haveExTicket;
        public int Chip
        {
            get { return chip; }
        }
        public int Gcoin
        {
            get { return gcoin; }
        }
        public bool Bribed
        {
            get { return bribed; }
        }
        public bool HaveExTicket
        {
            get { return haveExTicket; }
        }
        PrepareStateEvent prepare;
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
        //initialize settings
        void OnRoundStart()
        {
            bribed = false;
            haveExTicket = false;
        }
        void OnRollCard()
        {
            playerData[clientPlayer].ChangeChip(-2);
            UpdateUI();
            //chip -= 2;
            //Text chipLabel = players[0].transform.Find("chip").GetComponent<Text>();
            //chipLabel.text = chip.ToString();
        }
        void OnBribeRequestResultReceived(string m_name, bool m_result)
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
            playerData[clientPlayer].ChangeCoin(2);
            UpdateUI();
            //gcoin += 2;
            //Text coinLabel = players[0].transform.Find("gcoin").GetComponent<Text>();
            //coinLabel.text = gcoin.ToString();
            //bribed = true;
        }
        /// <summary>
        /// 从服务器端更新所有玩家信息显示
        /// </summary>
        /// <param name="data">默认data[0]是本地玩家</param>
        public void UpdatePlayerData(List<PlayerData> m_data)
        {
            for (int i = 0; i < 3; i++)
            {
                //storge data
                playerData[m_data[i].name] = m_data[i];
                //updateUI
                UpdateUI();
            }
        }
        /// <summary>
        /// 从服务器端更新一位玩家信息显示
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="m_data"></param>
        public void UpdatePlayerDate(string m_playerName, PlayerData m_data)
        {
            playerData[m_playerName] = m_data;
            UpdateUI();
        }
        void UpdateUI()
        {
            for (int i = 0; i < 3; i++)
            {
                string name = players[i].transform.Find("name").GetComponent<Text>().text;
                Text chip = players[i].transform.Find("chip").GetComponent<Text>();
                Text gcoin = players[i].transform.Find("gcoin").GetComponent<Text>();

                chip.text = playerData[name].chip.ToString();
                gcoin.text = playerData[name].coin.ToString();
            }
        }
    }
   
}

