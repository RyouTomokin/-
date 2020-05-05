using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peixi;

public class Test : MonoBehaviour
{
    public Func<string> bribe = new Func<string>(BribePlayer);
    
    // Start is called before the first frame update
    void Start()
    {
       
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
