using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiotest : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip testClip;

    void Start()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = testClip;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }
}
