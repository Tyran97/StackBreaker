using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelSpawner : MonoBehaviour
{
    public GameObject[] obstacleModel;
    [HideInInspector]
    public GameObject[] obstaclePrefab = new GameObject[4];

    public GameObject winPrefab;

    private GameObject  temp1Obstacle,temp2Obstacle;

    public int level = 1, nLevel = 2, addNumber = 7 ;
    

    float obstacleNumber = 0;

    public Material plateMat, baseMat;
    public MeshRenderer playerMeshRenderer;
   

    void Awake()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        //PlayerPrefs.DeleteAll();

        randomObstaclegenerator();
        float randomNumber = Random.value;
        for (obstacleNumber = 0; obstacleNumber > -level -addNumber ; obstacleNumber -=0.5f)
        {

            if (level <= 20)
            {
                temp1Obstacle = Instantiate(obstaclePrefab[Random.Range(0, 2)]); // ilk 2 model
            }

            if (level > 20 && level<50)
            {
                temp1Obstacle = Instantiate(obstaclePrefab[Random.Range(1, 3)]); // 2. ve 3. model
            }

            if (level >= 50 && level <= 100)
            {
                temp1Obstacle = Instantiate(obstaclePrefab[Random.Range(2, 4)]); // 3. ve 4. model
            }

            if (level > 100)
            {
                temp1Obstacle = Instantiate(obstaclePrefab[Random.Range(3, 4)]); // 4. model ( 3 enemy 1 plain)
            }

            temp1Obstacle.transform.position = new Vector3(0, obstacleNumber - 0.01f, 0); // ilk objenin başlangıç poziyonu
            temp1Obstacle.transform.eulerAngles = new Vector3(0, obstacleNumber * 8, 0); // ilk objenin başlangıç rotasyonu



            if (Mathf.Abs(obstacleNumber)  >= level * .3f && Mathf.Abs(obstacleNumber) <= level * .6f) // Game logic
            {
                temp1Obstacle.transform.eulerAngles = new Vector3(0, obstacleNumber * 8, 0);
                temp1Obstacle.transform.eulerAngles += Vector3.up * 180;
            }else if (Mathf.Abs(obstacleNumber) > level* 0.8f)
            {
                temp1Obstacle.transform.eulerAngles = new Vector3(0, obstacleNumber * 8, 0);
                if (randomNumber > 0.75f)
                {
                    temp1Obstacle.transform.eulerAngles += Vector3.up * 180;
                }
                
            }
            

            temp1Obstacle.transform.parent = FindObjectOfType<RotateManager>().transform; // Parent ile temp1Obstacle objelerine rotasyon ekliyoruz(RotateManager script)
        }

        temp2Obstacle = Instantiate(winPrefab);
        temp2Obstacle.transform.position = new Vector3(0, obstacleNumber - 0.01f, 0); // Zemin objesi

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            plateMat.color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
            baseMat.color = plateMat.color + Color.gray;
            playerMeshRenderer.material.color = baseMat.color;
        }
    }

    public void randomObstaclegenerator()
    {
        int random = Random.Range(0,5);


        switch (random)
        {
            case 0: // Circle 
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i];
                }
                break;

            case 1: // Flower
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i+4];
                }
                break;
            case 2: // Hex
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 8];
                }
                break;
            case 3: // Spikes
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 12];
                }
                break;
            case 4: // Square
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 16];
                }
                break;

            default:
                break;
        }

    }


    public void NextLevel() // Sonraki seviye
    {
        // Aynı sahnede bir sonraki seviyeyi çağırır
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        PlayerPrefs.SetInt("nLevel", PlayerPrefs.GetInt("nLevel") + 1);
        SceneManager.LoadScene(0); 
    }

}
