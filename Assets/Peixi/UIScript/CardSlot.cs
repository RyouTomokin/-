using Peixi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    List<GameObject> locks = new List<GameObject>();
    // Start is called before the first frame update
    int round = 1;
    PrepareStateEvent prepare;
    RoundTipFrame roundTipFrame;
    void Start()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
            locks.Add(item.gameObject);
        }
        prepare = FindObjectOfType<PrepareStateEvent>();
        roundTipFrame = FindObjectOfType<RoundTipFrame>();

        prepare.onRoundStarted+=()=> SetSlotState(new int[] { 4, 5 }, new bool[] { true, true });

        roundTipFrame.RoundChange += (round) =>
        {
            if (round == 3)
            {
                SetSlotState(4, true);
            }

            if (round == 6)
            {
                SetSlotState(5, true);
            }
        };
    }
    /// <summary>
    /// 设置卡牌框的锁定状态
    /// </summary>
    /// <param name="n">从左到右为0~5</param>
    /// <param name="state">false-锁定</param>
    public void SetSlotState(int n,bool state)
    {
        locks[n].SetActive(state);
    }
    /// <summary>
    /// 设置卡牌框的锁定状态
    /// </summary>
    /// <param name="n">从左到右为0~5</param>
    /// <param name="state">false-锁定</param>
    public void SetSlotState(int[] n,bool[] state)
    {
        if (n.Length != state.Length)
        {
            throw new System.Exception("n.Length != state.Length");
        }
        else
        {
            for (int i = 0; i < n.Length; i++)
            {
                SetSlotState(n[i], state[i]);
            }
        }
    }
}
