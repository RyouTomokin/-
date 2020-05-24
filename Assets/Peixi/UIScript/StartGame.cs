using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void Tutorial()
    {
        SceneManager.LoadScene(1);
    }
    public void StartScene(int n)
    {
        SceneManager.LoadScene(n);
    }

}
