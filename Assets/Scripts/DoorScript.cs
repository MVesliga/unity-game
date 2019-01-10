using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private AudioSource audioSource;

    public GameObject waypoint1;
    public GameObject waypoint2;

    //kontroler za animaciju
    private Animator animator;
    //bool koji postane true kad smo u poziciji u kojoj možemo otvoriti vrata
    private bool entered;
    //služi za pronalaženje osi na kojoj su waypoint ispred i waypoint iza vrata povezani
    private string direction;

    private bool doorOpened;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        doorOpened = false;

        animator = GetComponentInChildren<Animator>();
        entered = false;

        //pronalaženje poveznice među waypointovima između kojih su vrata
        if (waypoint1.GetComponent<Waypoint>().left == waypoint2) { direction = "left"; }
        else if (waypoint1.GetComponent<Waypoint>().right == waypoint2) { direction = "right"; }
        else if (waypoint1.GetComponent<Waypoint>().up == waypoint2) { direction = "up"; }
        else { direction = "down"; }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Press 'E' to open door");
        entered = true;

        //kad uđemo u trigger od vrata gasi se link između waypointova, to nam onemogućuje prolazak kroz zatvorena vrata
        if (direction == "left") {
            waypoint1.GetComponent<Waypoint>().left = null;
            waypoint2.GetComponent<Waypoint>().right = null;
        }
        else if (direction == "right") {
            waypoint1.GetComponent<Waypoint>().right = null;
            waypoint2.GetComponent<Waypoint>().left = null;
        }
        else if (direction == "up") {
            waypoint1.GetComponent<Waypoint>().up = null;
            waypoint2.GetComponent<Waypoint>().down = null;
        }
        else {
            waypoint1.GetComponent<Waypoint>().down = null;
            waypoint2.GetComponent<Waypoint>().up = null;
        }
    }

    private void Update()
    {
        //Ako smo u triggeru i neko rokne "E"
        if (Input.GetButtonDown("Interact") && entered && !EncounterController.instance.AreWeFighting())
        {
            doorOpened = true;
            //audio
            audioSource.time = 1.5f;
            audioSource.Play();
            audioSource.SetScheduledEndTime(AudioSettings.dspTime + (2.5f-1.5f));

            //pokretanje animacije za otvaranje vrata
            animator.SetBool("close", false);
            animator.SetBool("open", true);

            //paljenje poveznice među waypointovima, tj sad možemo proći kroz vrata
            if (direction == "left")
            {
                waypoint1.GetComponent<Waypoint>().left = waypoint2;
                waypoint2.GetComponent<Waypoint>().right = waypoint1;
            }
            else if (direction == "right")
            {
                waypoint1.GetComponent<Waypoint>().right = waypoint2;
                waypoint2.GetComponent<Waypoint>().left = waypoint1;
            }
            else if (direction == "up")
            {
                waypoint1.GetComponent<Waypoint>().up = waypoint2;
                waypoint2.GetComponent<Waypoint>().down = waypoint1;
            }
            else
            {
                waypoint1.GetComponent<Waypoint>().down = waypoint2;
                waypoint2.GetComponent<Waypoint>().up = waypoint1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorOpened)
        {
            //audio
            audioSource.time = 5.0f;
            audioSource.PlayDelayed(0.3f);
            audioSource.SetScheduledEndTime(AudioSettings.dspTime + (6.0f - 5.0f));
            doorOpened = false;
        }

        // kad se maknemo iz triggera, zatvori vrata
        animator.SetBool("close", true);
        animator.SetBool("open", false);
        entered = false;
    }
}
