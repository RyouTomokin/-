using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peixi;

public class Test : MonoBehaviour
{
    static Test instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        FindObjectOfType<PrepareStateEvent>().onRollCard += Test1;
    }
    void Test1()
    {
        print("roll card");
    }
    void Test1(string n)
    {
        print(222);
    }
}
