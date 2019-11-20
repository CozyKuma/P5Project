using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ambientSounds : MonoBehaviour
{

    public float delayInSeconds;
    public AudioClip waterdrop;
    public bool keepPlaying = true;

    private float timer;

    private float floorTimer;
    private float randomDelay;
    
    AudioSource audioData;
    
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        
        StartCoroutine(SoundOut());

        audioData.loop = true;
        audioData.Play();
    }

    // Update is called once per frame
/*    void Update()
    {
        audioData.PlayOneShot(waterdrop, 1.0f);

        timer += Time.deltaTime;

        floorTimer = Mathf.Floor(timer);
        
        Debug.Log(Mathf.Floor(timer));
        Debug.Log(floorTimer % delayInSeconds == 0);
        
        if (floorTimer % delayInSeconds == 0)
        {
            audioData.PlayOneShot(waterdrop, 1.0f);
        }
    }*/

    IEnumerator SoundOut()
    {
        while (keepPlaying){
            randomDelay = delayInSeconds - (Random.Range(5.0f, 15.0f));
            audioData.PlayOneShot(waterdrop);  
            Debug.Log("ChOO-ChOO");
            yield return new WaitForSeconds(randomDelay);
        }
    }
}