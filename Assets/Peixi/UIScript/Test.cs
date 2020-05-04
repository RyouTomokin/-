using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peixi;

public class Test : MonoBehaviour
{
    public Func<string> testFunc;
    static Test instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        testFunc += Test1;
        print(testFunc());
    }
    string Test1()
    {
        print("test well");
        return "Plyaer1";
    }

}
