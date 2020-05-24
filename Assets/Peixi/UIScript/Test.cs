using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peixi;
using Tomokin;


interface ITest
{
    void DoTest();
}
public class Test : MonoBehaviour, ITest
{
    public void DoTest()
    {

        print("do something");
    }
}


