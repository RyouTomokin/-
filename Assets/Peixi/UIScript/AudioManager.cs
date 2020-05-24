using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Peixi;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource bribed;
    PrepareStateEvent prepare;
    // Start is called before the first frame update
    void Start()
    {
        prepare = FindObjectOfType<PrepareStateEvent>();
        prepare.onRoundStarted += () =>
        {
            bgm.Play();
        };
        prepare.bribeMessageReceived += (int n) =>
        {
            bribed.Play();
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
