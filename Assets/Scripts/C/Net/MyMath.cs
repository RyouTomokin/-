using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C;
namespace C
{
    public static class MyMath
    {
        public static string IdGen()
        {
            int i = 0;
            string id = "";
            while(i < 16)
            {
                i++;
                id += Random.Range(0, 10).ToString();
            }
            Debug.Log(id);
            return id;
        }
    }
}

