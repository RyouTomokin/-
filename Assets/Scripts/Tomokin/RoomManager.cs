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

        public static PlayerGameData[] PD = new PlayerGameData[3];  //名字+ID

        private bool IsReady = false;

        public void JoinRoom(string name)
        {
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
            JoinRoom(CilentManager.PlayerName);
        }

        public void LeaveRoom(string name)
        {
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
            
            GameStartInit(TomokinNet.PlayersInRoom, true);
        }
        
        public void GameStartInit(List<string> names, bool ishouseowner, bool isNetGame = true)
        {
            foreach (var scene in GameScenes)
            {
                scene.SetActive(true);
            }
            GameManager GM = GameManager.Instance;
            //生成3个玩家的数据类
            for (int i = 0; i < names.Count; i++)
            {
                PD[i] = new PlayerGameData(names[i], i, ishouseowner);
                CilentManager.PDs[i] = PD[i];
                if (names[i] == CilentManager.PlayerName + CilentManager.PlayerID)
                {
                    CilentManager.playerdata = PD[i];
                    CilentManager.PlayerNum = i;
                }
            }
            GM.UpdateUIMsg();
            if (ishouseowner)
            {
                int[] n = HouseOwner.AgreementInit();
                if (isNetGame) Net.InitAgreement(n[0], n[1]);
                else//单机测试
                {
                    FindObjectOfType<BookManager>().Add_Card_To_Book(0, n[0]);
                    FindObjectOfType<BookManager>().Add_Card_To_Book(1, n[1]);
                }
            }
            //给本地玩家换牌
            GM.HCM.drop.value = GM.dropInRoom.value;
            GM.Roll();

            //进入准备阶段
            PrepareStateEvent prepare = FindObjectOfType<PrepareStateEvent>();
            Debug.Log(prepare);
            prepare.RoundStartInvoke();
            this.gameObject.SetActive(false);
        }
    }
}
