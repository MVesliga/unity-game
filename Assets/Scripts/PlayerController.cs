using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//za hodanje i rotiranje
public class PlayerController : MonoBehaviour {

    private AudioSource audioSource;

    //trenutna točka na kojoj se lik nalazi
    public GameObject currentWaypoint;

    //predstavljat će točku na koju se možemo pomaknuti sa trenutne pozicije s obzirom na orijentaciju
    private GameObject targetWaypoint;

    //služe za spriječavanje istovremenog hodanja i rotiranja
    private bool executingMovement;
    private bool executingRotate;

    private bool fightingInProgress;

    //za smoothing hodanja, da se samo ne warpamo
    private float pathTraversed;

    //trenutna rotacija
    private float rotation;

    //za koliko stupnjeva se rotiramo
    private int degRotation = 90;

    public float speed = 2.5f;  
    // utječe na brzinu rotacije
    public float inTime = 0.6f;

    private void Start()
    {
        executingRotate = false;
        executingMovement = false;
        fightingInProgress = false;
        rotation = transform.eulerAngles.y;
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 2;
    }

    private void Update()
    {
        //stalno provjeravamo u kojem smo smjeru zarotirani
        rotation = transform.eulerAngles.y;

        if (Input.GetButtonDown("Forward") && !executingRotate && !executingMovement && !fightingInProgress && !PlayerStats.instance.enterName.activeSelf) {
            //bool FindTargetu služi samo da zna da li da traži waypoint ispred ili iza lika
            targetWaypoint = FindTarget(true);
            //resetiranje varijable za smoothing pokreta
            pathTraversed = 0f;
            //ako postoji targetWaypoint, započni kretanje prema njemu
            if (targetWaypoint != null) {
                executingMovement = true;
                //audio
                StartCoroutine(PlayFootsteps());
            }
        }

        //see above
        if (Input.GetButtonDown("Back") && !executingRotate && !executingMovement && !fightingInProgress && !PlayerStats.instance.enterName.activeSelf)
        {
            targetWaypoint = FindTarget(false);
            pathTraversed = 0f;
            if (targetWaypoint != null)
            {
                executingMovement = true;
                //audio
                StartCoroutine(PlayFootsteps());
            }
        }

        if (executingMovement)
        {
            // sumiramo u brzinu, kad dođe do jedan prešli smo cijeli put
            // Time.deltaTime služi da brzina ne zavisi o broju renderiranih frejmova
            pathTraversed += speed * Time.deltaTime;

            // Lerp je smoothing funkcija kojoj damo start end i inkrement
            // transform.position je pozicija lika
            transform.position = Vector3.Lerp(currentWaypoint.transform.position, targetWaypoint.transform.position, pathTraversed);
            
            // Ako smo se pomakli na destinaciju, target postaje current, omogući unos novog kretanja
            // i omogući rotaciju kamere
            if (transform.position == targetWaypoint.transform.position)
            {
                currentWaypoint = targetWaypoint;
                executingMovement = false;
                fightingInProgress = EncounterController.instance.RollForBattle();
            }
        }

        //hope this works
        if (fightingInProgress) {
            fightingInProgress = EncounterController.instance.AreWeFighting();
        }

        //rotacija
        if (Input.GetButtonDown("Right") && !executingRotate && !executingMovement && !fightingInProgress && !PlayerStats.instance.enterName.activeSelf)
        {
            StartCoroutine(PlayFootsteps());
            executingRotate = true;
            StartCoroutine(RotateMe(Vector3.up * degRotation, inTime, returnValue =>
            { executingRotate = returnValue; }));
        }

        if (Input.GetButtonDown("Left") && !executingRotate && !executingMovement && !fightingInProgress && !PlayerStats.instance.enterName.activeSelf)
        {
            StartCoroutine(PlayFootsteps());
            executingRotate = true;
            StartCoroutine(RotateMe(Vector3.up * -degRotation, inTime, returnValue =>
            { executingRotate = returnValue; }));

        }
    }

    private GameObject FindTarget(bool forward) {
        // po rotaciji uzima iz current waypointa link na idući waypoint te ga vraća u target waypoint
        if (rotation == 90)
        {
            if (forward)
            {
                return currentWaypoint.GetComponent<Waypoint>().left;
            }
            else
            {
                return currentWaypoint.GetComponent<Waypoint>().right;
            }
            
        }
        else if (rotation == 180)
        {
            if (forward)
            {
                return currentWaypoint.GetComponent<Waypoint>().up;
            }
            else
            {
                return currentWaypoint.GetComponent<Waypoint>().down;
            }
        }
        else if (rotation == 270)
        {
            if (forward)
            {
                return currentWaypoint.GetComponent<Waypoint>().right;
            }
            else
            {
                return currentWaypoint.GetComponent<Waypoint>().left;
            }
        }
        else
        {
            if (forward)
            {
                return currentWaypoint.GetComponent<Waypoint>().down;
            }
            else
            {
                return currentWaypoint.GetComponent<Waypoint>().up;
            }
        }
    }


    IEnumerator PlayFootsteps() {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.Play();
    }

    //korutina https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
    IEnumerator RotateMe(Vector3 byAngles, float inTime, System.Action<bool> callback)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        /* Quaternion.Slerp radi interpolaciju od a do b po vremenu t u intervalu [0,1]
           for petlja nikada neće dati točno 1, pa za završnu rotaciju još ga
           jednom pozivamo i dajemo mu točno 1*/
        transform.rotation = Quaternion.Slerp(fromAngle, toAngle, 1);
        // yield retun null čeka idući frame i onda nastavlja izvršavanje
        yield return null;
        callback(false);

    }
}
