using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource playerAudioSource;
    
    public AudioClip[] BackgroundSounds;
    
    
    public AudioClip boatGlideAudioClip;
    public AudioSource boatGlideSource;

    public AudioClip CollectiblePicupClip;
    public AudioSource CollectiblePicupSource;

    public AudioClip[] ObstacleClip;
    public AudioSource ObstacleAudioSource;
    
    public AudioClip OofAudioClip;
    public AudioSource OofAudioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAudioSource.clip = BackgroundSounds[0];
        playerAudioSource.Play();

        boatGlideSource.clip = boatGlideAudioClip;
        boatGlideSource.loop = true;
        boatGlideSource.Play();

        
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCollectibleSound()
    {
        CollectiblePicupSource.clip = CollectiblePicupClip;
        CollectiblePicupSource.Play();
    }

    public void playObstacleSound()
    {
        int randomValue = (int) Mathf.Ceil(Random.value * 2);
        ObstacleAudioSource.clip = ObstacleClip[randomValue];
        ObstacleAudioSource.Play();
    }

    public void playDeadSound()
    {
        OofAudioSource.clip = OofAudioClip;
        OofAudioSource.Play();

    }


}
