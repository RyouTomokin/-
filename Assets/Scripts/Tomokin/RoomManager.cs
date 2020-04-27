using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using C;
using Peixi;

namespace Tomokin
{
    public class RoomManager : MonoBehaviour
    {
        public GameObject[] PlayerMsg;      //显示玩家信息的面板
        public GameObject[] GameScenes;     //游戏的UI

        public static PlayerGameData[] PD = new PlayerGameData[3];

        private bool IsReady = false;

        public void JoinRoom(string name)
        {
            TomokinNet.PlayersInRoom.Add(name);
            foreach (var P in PlayerMsg)
            {
                if (!P.activeSelf)
                {
                    P.transform.SetAsLastSibling();
                    P.GetComponentInChildren<Text>().text = name;
                    P.SetActive(true);
                    break;
                }
            }
        }

        public void JoinRoom()
        {
            JoinRoom(GameManager.PlayerName);
        }

        public void LeaveRoom(string name)
        {
            TomokinNet.PlayersInRoom.Remove(name);
            foreach (var P in PlayerMsg)
            {
                if (P.GetComponentInChildren<Text>().text == name)
                {
                    P.transform.SetAsLastSibling();
                    P.GetComponentInChildren<Text>().text = "";
                    P.SetActive(false);
                    break;
                }
            }
        }

        public void Ready(Text txt)
        {
            if (IsReady) txt.text = "准备";
            else txt.text = "已准备";
            IsReady = !IsReady;
            if (!IsReady) return;
            //判断所有玩家都准备了
            this.gameObject.SetActive(false);
            foreach (var scene in GameScenes)
            {
                scene.SetActive(true);
            }
            GameStartInit(TomokinNet.PlayersInRoom, true);
        }
        
        public void GameStartInit(List<string> names, bool ishouseowner)
        {
            //生成3个玩家的数据类
            for (int i = 0; i < names.Count; i++)
            {
                PD[i] = new PlayerGameData(name, i, ishouseowner);
            }
            GameManager.

            //进入准备阶段
            FindObjectOfType<PrepareStateEvent>().RoundStartInvoke();
        }
    }
}
