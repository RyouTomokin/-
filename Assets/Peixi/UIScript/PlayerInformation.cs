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
        //protected int gcoin = 2;
        protected string clientPlayer = "Player1";
        protected bool bribed;
        protected bool haveExTicket;

        //test code
        int clientNum = 0;
        List<PlayerGameData> datas = new List<PlayerGameData>();
        public int Chip
        {
            get { return chip; }
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
                int m_coin = Tomokin.CilentManager.playerdata.GetMoney;
                Text coinLabel = players[0].transform.Find("chip").GetComponent<Text>();
                coinLabel.text = m_coin.ToString();
            }
        }
        void OnAcceptBribeButtonPressed(string name)
        {
            int m_coin = Tomokin.CilentManager.playerdata.GetChip;
            Text coinLabel = players[0].transform.Find("gcoin").GetComponent<Text>();
            coinLabel.text = m_coin.ToString();
            bribed = true;
        }
        /// <summary>
        /// 服务器端更新所有玩家信息显示
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
        /// 服务器端更新所有玩家信息显示
        /// </summary>
        /// <param name="m_data"></param>
        public void UpdatePlayerData(List<PlayerGameData> m_data)
        {
            var temp = m_data[clientNum];
            //print(0 + m_data[0].PlayerName + m_data[0].Number);
            m_data.RemoveAt(clientNum);
         
            m_data.Insert(0,temp);
            //for (int i = 0; i < m_data.Count; i++)
            //{
            //    Debug.Log(i + m_data[i].PlayerName + m_data[i].Number);
            //}
            for (int i = 0; i < 3; i++)
            {
                Text name = players[i].transform.Find("name").GetComponent<Text>();
                Text chip = players[i].transform.Find("chip").GetComponent<Text>();
                Text coin = players[i].transform.Find("gcoin").GetComponent<Text>();

                name.text = m_data[i].PlayerName;
                chip.text = m_data[i].GetChip.ToString();
                coin.text = m_data[i].GetMoney.ToString();
            }
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
        public void GameStartInit()
        {
            
        }   
    }
   
}

