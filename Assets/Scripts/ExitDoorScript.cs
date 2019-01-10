using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorScript : MonoBehaviour {

    public Item item;
    private AudioSource audioSource;
    public AudioClip locked;
    public AudioClip unlocked;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerStats.instance.toggleLockedDoorMessage(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Press 'E' to open door");
    }

    private void OnTriggerStay(Collider other)
    {
        //Ako imamo ključ u inventoryu, uništi ga i prebaci nas na sljedeći level
        if (Input.GetButtonDown("Interact"))
        {
            if (Inventory.instance.Contains(item))
            {
                audioSource.clip = unlocked;
                audioSource.Play();
                Inventory.instance.Remove(item);
                if (SceneManager.GetActiveScene().name == "Floor1")
                {
                    SceneManager.LoadScene("Floor2");
                }
                if (SceneManager.GetActiveScene().name == "Floor2")
                {
                    SceneManager.LoadScene("Floor3");
                }
                if (SceneManager.GetActiveScene().name == "Floor3")
                {
                    SceneManager.LoadScene("Floor4");
                }
                PlayerStats.instance.toggleLevelUp(true);
            }
            else
            {
                audioSource.clip = locked;
                audioSource.Play();
                PlayerStats.instance.toggleLockedDoorMessage(true);
                Debug.Log("You don't have a key!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("");
    }

    public void confirmDoor()
    {
        PlayerStats.instance.toggleLockedDoorMessage(false);
        Debug.Log("STISNEM CONTINUE!");
    }
}
