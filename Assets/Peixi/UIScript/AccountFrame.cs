using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Peixi
{
    public struct Score
    {
        public string name;
        public int GcoinGain;
        public int chipGain;
    }
    public class AccountFrame : MonoBehaviour
    {
        [SerializeField]
        GameObject[] champions;
        AccountState accountState;

        List<Score> scores = new List<Score>();

        private void Start()
        {
            ActivateFrame(false);

            accountState = FindObjectOfType<AccountState>();
            accountState.onRoundStarted += OnRoundStart;
            accountState.onRoundEnded += OnRoundEnd;
            Test();
        }

        void OnRoundStart()
        {
            //print("AccountFrame connect well");
           ActivateFrame(true);
            ShowPlayerScore();
        }

        void OnRoundEnd()
        {
            ActivateFrame(false);
        }

        void ShowPlayerScore()
        {
            for (int i = 0; i < champions.Length; i++)
            {
                Text[] texts = champions[i].GetComponentsInChildren<Text>();
                //print(texts[0].transform.name);
                //print(texts[1].transform.name);
                //print(texts[2].transform.name);
                texts[0].text = scores[i].name;
                texts[1].text = scores[i].chipGain.ToString();
                texts[2].text = scores[i].GcoinGain.ToString();
            }
        }

        void ActivateFrame(bool active)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }

        void Test()
        {
            Score player1 = new Score();
            player1.name = "Player1";
            player1.chipGain = 1;
            player1.GcoinGain = 1;

            Score player2 = new Score();
            player2.name = "Player2";
            player2.chipGain = 2;
            player2.GcoinGain = 1;

            Score player3 = new Score();
            player3.name = "Player3";
            player3.chipGain = 3;
            player3.GcoinGain = -1;

            scores.Insert(0, player1);
            scores.Insert(1, player2);
            scores.Insert(2, player3);
        }

    }
}

