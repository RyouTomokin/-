using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peixi;
using Tomokin;

public class Test : MonoBehaviour
{

    public Func<string> bribe = new Func<string>(BribePlayer);
    // Start is called before the first frame update
    void Start()
    {
        //string n = Tomokin.CilentManager.playerdata.PlayerName;
    }
    static string BribePlayer()
    {
        print(11);
        return "Alice";
    }
    string Receive()
    {
        print(bribe());
        return null;
    }
}
