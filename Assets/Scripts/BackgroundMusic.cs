using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

    #region Singleton
    //Instancira sam sebe i održava se na životu kroz sve scene
    public static BackgroundMusic instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("More then one instance of EncounterController found!");
            Destroy(gameObject);
        }
    }
    #endregion

    public AudioSource audioSource;
    public AudioClip background;
    public AudioClip battle;

    private void Start()
    {
        audioSource.clip = background;
        audioSource.Play();
    }

    public void playBatlle() {
        audioSource.clip = battle;
        audioSource.Play();
    }

    public void playTheme() {
        audioSource.clip = background;
        audioSource.Play();
    }
}
