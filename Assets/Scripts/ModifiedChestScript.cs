using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiedChestScript : MonoBehaviour {


    

    //Nothing of value here, despacito chest kontroler 


    
    private Animator animator;
    private AudioSource audioSource;
    private bool started = false;
    //s ovom forom skrinje i vrata ce se otvarati samo ako ih direktno gledamo
    private float yRotation;
    private float yRotationPlayer;

    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        yRotation = transform.rotation.eulerAngles.y + 90;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Chest in range.");
    }

    private void OnTriggerStay(Collider other)
    {
        yRotationPlayer = other.transform.GetChild(0).rotation.eulerAngles.y;
        if (other.tag == "Player" && yRotation == yRotationPlayer && !started)
        {
            animator.SetBool("openChest", true);
            audioSource.Play(0);
            started = true;
            Debug.Log("This is so sad Alexa...");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Chest out of range.");
    }
}
