using Peixi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSimulation : MonoBehaviour
{
    Test client;
    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType<PrepareStateEvent>().bribeMessageSent += ReceiveBribe;
    }
    void ReceiveBribe(int num)
    {
        print("收到玩家" + num + "的贿赂消息");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
