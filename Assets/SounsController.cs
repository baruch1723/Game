using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SounsController : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundOnce()
    {
        _audioSource.clip = _audioClip;
        _audioSource.Play();
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
