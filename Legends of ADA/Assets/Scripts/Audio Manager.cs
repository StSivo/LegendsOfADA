using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public AudioSource menuMusic;
    public AudioSource[] battleMusics;
    // Update is called once per frame
    void Update()
    {
        
    }
}
