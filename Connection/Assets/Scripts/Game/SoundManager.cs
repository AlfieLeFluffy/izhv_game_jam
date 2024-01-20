using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [SerializeField] public AudioSource musicSource, effectSource, characterSource, movementSource;

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void playEffSound(AudioClip clip){
        effectSource.PlayOneShot(clip);
    }

    public void stopEffSound(){
        effectSource.Stop();
    }

    public void playCharSound(AudioClip clip){
        characterSource.PlayOneShot(clip);
    }

    public void stopCharSound(){
        characterSource.Stop();
    }

    public bool isPlayingCharacter(){
        return characterSource.isPlaying;
    }

    public void playMovSound(AudioClip clip){
        movementSource.PlayOneShot(clip);
    }

    public void stopMovSound(){
        movementSource.Stop();
    }

    public bool isPlayingMov(){
        return movementSource.isPlaying;
    }

}
