using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour {

    private AudioSource audioSource;

    //chest sadrži slot za item
    public Item item;
    //animacija
    private Animator animator;

    private bool entered;
    private bool pickedUp;
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        entered = false;
        pickedUp = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Press 'E' to open chest.");
        entered = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && entered && !pickedUp && !EncounterController.instance.AreWeFighting())
        {
            //audio
            audioSource.time = 0.5f;
            audioSource.Play();
            audioSource.SetScheduledEndTime(AudioSettings.dspTime + (1.5f - 0.5f));

            animator.SetBool("openChest", true);
            //Rokni item u inventory
            Inventory.instance.Add(item);
            //ubijanje daljnje interakcije sa škrinjom
            pickedUp = true;
            PlayerStats.instance.toggleKeyFoundMessage(true);
            Debug.Log("Picked up " + item.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        entered = false;
    }

}
