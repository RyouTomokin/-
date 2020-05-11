using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Peixi
{
 
    public class AccountFrame : MonoBehaviour
    {
        [SerializeField]
        GameObject[] champions;
        AccountState accountState;

        private void Awake()
        {
            accountState = FindObjectOfType<AccountState>();
            accountState.onRoundStart += ShowPlayerScore;
            accountState.onRoundEnded += OnRoundEnd;
        }
        void OnRoundStart()
        {
            ActivateFrame(true);
        }
        void OnRoundEnd()
        {
            ActivateFrame(false);
        }
        void ShowPlayerScore(Score[] m_score)
        {
            ActivateFrame(true);
            for (int i = 0; i < champions.Length; i++)
            {
                Text[] texts = champions[i].GetComponentsInChildren<Text>();
                texts[0].text = m_score[i].name;
                texts[1].text = m_score[i].chipGain.ToString();
                texts[2].text = m_score[i].GcoinGain.ToString();
                texts[3].text = m_score[i].score.ToString();
            }
        }
        void ActivateFrame(bool active)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }
}

