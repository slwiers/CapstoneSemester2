using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomAuido : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource source;

    public float minWaitBetweenPlays = 1f;
    public float maxWaitBetweenPlays = 5f;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(SoundPlayer());
    }

    private IEnumerator SoundPlayer()
    {
        float waitTime;
        while(true)
        {
            waitTime = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            yield return new WaitForSeconds(waitTime);
            PlaySound();
        }
    }

    private void PlaySound()
    {
        currentClip = audioClips[Random.Range(0, audioClips.Count)];
        source.clip = currentClip;
        source.Play();
    }
}
