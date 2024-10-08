using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 cameraPos;
    private Transform player, win; // player ve win poziyonları 

    private float cameraOffset = 4f; // Kameranın player'ı nereye kadar takip edeceğini belirlemek için bir değer(nokta ile kamera arasındaki mesafe)

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform; // Player poziyonunun ataması
    }

    void Update()
    {
        if (win == null) // Henüz oluşmama olasılığını gidermek için(En son oluşan obje)
        {
            win = GameObject.Find("win(Clone)").GetComponent<Transform>(); // Win poziyonunun ataması
           
        }

        if(transform.position.y>player.position.y && transform.position.y > win.position.y + cameraOffset)
        {
            cameraPos = new Vector3(transform.position.x, player.position.y, transform.position.z); // Kamera ile player poziyonunu eşitler
            transform.position = new Vector3(transform.position.x, cameraPos.y, -5); // Kameranın başlangıç poziyonu

        }

    }
}
