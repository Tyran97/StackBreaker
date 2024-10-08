using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    // Unity panelinde obstacle'ları görebilmek için SerializeField yapıldı (Private olduğu için)
    [SerializeField]
    private Obstacle[] obstacles = null; // Obstacle atamak için bir array


    public void ShatterAllObstacles() 
    {
        if (transform.parent != null) // null kontrolü
        {
            transform.parent = null;
        }


        foreach (Obstacle item in obstacles) // Bütün obstacle'lar için shatter fonksiyonu çalıştır
        {
            item.Shatter();
        }

        StartCoroutine(GarbageCollector()); 


    }

    IEnumerator GarbageCollector() // Performans için parçalanan dosyaları arka planda yok etmemizi sağlayan fonksiyon
    {
        // Objeler shatter olduktan 1 saniye sonra yok eder.
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
