using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapScript : MonoBehaviour {

    //kontrola mini mapa
    public new Camera camera;
    public Transform player;
    private int mode;


    void LateUpdate () {
        //promjena moda 0 je cjela mapa 1 je lokalni prikaz
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mode == 0)
            {
                mode = 1;         
            }
            else if (mode == 1)
            {
                mode = 0;
            }
        }
        if (mode == 1)
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0);
            camera.orthographicSize = 10;
        }
        if(mode==0)
        {
            transform.position = new Vector3(0f,47f,0f);
            transform.rotation = Quaternion.Euler(90f, 0, 0);
            camera.orthographicSize = 25;
        }
    }
}
