using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AdditionalTools {

    // Alat za povezivanje waypointova na mapi, dalje nepotreban, ponovno pokretanje će samo omogućiti hodanje kroz neke zidove

    [MenuItem("Proizvodno Postrojenje/ Connect the Waypoints")]
    public static void ConnectTheWaypoints()
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach (GameObject x in waypoints)
        {
            foreach (GameObject y in waypoints)
            {
                //left x<0
                if (x.transform.position.z == y.transform.position.z) {
                    if(x.transform.position.x == y.transform.position.x - 2)
                    x.GetComponent<Waypoint>().left = y;
                }
                
                //right x>-18
                if (x.transform.position.z == y.transform.position.z)
                {
                    if (x.transform.position.x == y.transform.position.x + 2)
                        x.GetComponent<Waypoint>().right = y;
                }
                
                //up z>-18
                if (x.transform.position.x == y.transform.position.x)
                {
                    if (x.transform.position.z == y.transform.position.z + 2)
                        x.GetComponent<Waypoint>().up = y;
                }
                
                //down z<0
                if (x.transform.position.x == y.transform.position.x)
                {
                    if (x.transform.position.z == y.transform.position.z - 2)
                        x.GetComponent<Waypoint>().down = y;
                }
            }

        }
    }
}
