using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    bool tap , conqueror;
    float conquerorTime; 

    public GameObject fireShield;

    // Level progress barı için değişkenler
    public int currObjNum;
    public int totalObjNum;

    // Panelden eklemek için
    [SerializeField]
    AudioClip win, death, cDestroy, destroy, bounce; // Ses dosyaları için değişkenler
    
    //UI tanımları
    public Image ConquerorSlider;
    public GameObject ConquerorOBJ;
    public GameObject gameOverUI;
    public GameObject finishUI;

    public enum PlayerState // Oyun durumu
    {
        Prepare,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public PlayerState playerstate = PlayerState.Prepare; // Oyun durumunun ilklendirilmesi

    void Start()
    {

        totalObjNum = FindObjectsOfType<ObstacleController>().Length; // Seviyede ki toplam obstacle sayısını bulur

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Ball objesine tanımlanan rigidbody ataması
        currObjNum = 0;
       
    }

    void Update()
    {
        if (playerstate == PlayerState.Playing)
        {
            if (Input.GetMouseButtonDown(0)) // Ekrana dokunulduğunda tap değişkeni true olur
            {
                tap = true;
            }

            if (Input.GetMouseButtonUp(0)) // Ekrana dokunulmadığında tap değişkeni false olur
            {
                tap = false;
            }


            if (conqueror) // Conqueror true yani açık ise, conquererTime ' ı belli bir değerle azalt ve fireshield aktif değilse aktif et
            {
                conquerorTime -= Time.deltaTime * .35f;
                if (!fireShield.activeInHierarchy)
                {
                    fireShield.SetActive(true);
                }
            }
            else
            {
                if (fireShield.activeInHierarchy) // FireShield'ı kapat
                {
                    fireShield.SetActive(false);
                }

                if (tap)
                {
                    conquerorTime += Time.deltaTime * 0.8f; // Ekrana dokunulurken conquerorTime'ı arttır
                }
                else
                {
                    conquerorTime -= Time.deltaTime * 0.5f; // Ekrana dokunulmadığında conquerorTime'ı azalt
                }
            }


            if (conquerorTime >= 0.15f || ConquerorSlider.color == Color.red)
            {
                ConquerorOBJ.SetActive(true);
            }
            else
            {
                ConquerorOBJ.SetActive(false);
            }
            
            
            
            if (conquerorTime >= 1) // ConquerorTime 1'e eşit veya büyükse, 1'e sabitle , conqueror'ı aktif et
            {
                conquerorTime = 1;
                conqueror = true;
                //Debug.Log("conqueror");
                ConquerorSlider.color=Color.red;
            }
            else if (conquerorTime <= 0) // ConquerorTime 0'a eşit veya küçükse, 0'a sabitle , conqueror'ı kapat
            {
                conquerorTime = 0;
                conqueror = false;
                //Debug.Log("-----------");
                ConquerorSlider.color=Color.white;
            }

            if ( ConquerorOBJ.activeInHierarchy)
            {
                ConquerorSlider.fillAmount = conquerorTime / 1;
            }

           
            
            
            
        }

        if (playerstate == PlayerState.Finish) // Oyun durumu finish ise ekrana dokunulduğunda sonraki seviyeye geç
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<LevelSpawner>().NextLevel();
            }
        }

    }


    public void shatterObstacles()
    {
        // Conqueror aktifken 3 skor değilken 1 skor ekle
        if (conqueror) 
        {

            ScoreManager.sample.addScore(3);
        }
        else
        {
            ScoreManager.sample.addScore(1);
        }
        
    }



    private void FixedUpdate()
    {

        if (playerstate == PlayerState.Playing) // Oyun durumu playing ise
        {
            if (tap) // tap true ise rigidbody' e aşağı dogru hız ataması yapılır(y ekseni)
            {

                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0); //
                

            }
        }


    }

    private void OnCollisionEnter(Collision collision) // Collision(çarpışma) olduğunda
    {
        if (!tap) // tap false ise ve bir nesneye etkileşime girerse rigidbody 'e yukarı doğru hız ataması yapılır
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
        else
        {
            if (conqueror)
            {
                if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "plane") // Conqueror açık ise enemy veya plane taglı objeleri shatter edebiliyoruz(power up)
                {
                    // ObstacleController sınıfında yazdığımız shatter animasyon fonksiyonu çağırıyoruz
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles(); 
                    shatterObstacles(); // Skor fonksiyonu çalışır
                    SoundManager.instance.playAudio(cDestroy, 0.5f);  // cDestroy soundtrack çalışır
                    currObjNum++;
                }
               
            }
            else
            {
                if (collision.gameObject.tag == "enemy") // Objenin tagi(etiketi) enemy ise 
                {
                    // ObstacleController sınıfında yazdığımız shatter animasyon fonksiyonu çağırıyoruz ve çalıştırıyoruz
                    collision.transform.parent.GetComponent<ObstacleController>().ShatterAllObstacles();
                    shatterObstacles(); // Skor fonksiyonu çalışır
                    SoundManager.instance.playAudio(destroy, 0.5f); // destroy soundtrack çalışır
                    currObjNum++;

                }
                else if (collision.gameObject.tag == "plane") // Objenin tagi(etiketi) plane ise 
                {
                    // GameOverUI devreye girer, oyun durumu finish olur , Kinematic true olur(oyun durur) , Skor resetlenir, death soundtrack çalışır
                    // Debug.Log("GameOver");
                    gameOverUI.SetActive(true);
                    playerstate = PlayerState.Finish;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    ScoreManager.sample.ResetScore();
                    SoundManager.instance.playAudio(death, 0.5f);
                    gameOverUI.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "" + PlayerPrefs.GetInt("HighScore", 1);
                    gameOverUI.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + ScoreManager.sample.sayı;
                    PlayerPrefs.SetInt("Level", 0);
                    PlayerPrefs.SetInt("nLevel", 1);
                }
            }

            
            
          
           
        }  
        
        
       FindObjectOfType<GameUIController>().LevelSliderFill(currObjNum /(float)totalObjNum);
        

        if(collision.gameObject.tag=="Finish" && playerstate == PlayerState.Playing) // Tagi(etiketi) finish olan objeye(win) gelindiğinde ve oyun durumu playing ise
        {
            // Oyun durumunu finish yapar, win soundtrack çalısır, finishUI ekrana gelir, ekrana dokunulduğunda sonraki seviyeye geçer
            playerstate = PlayerState.Finish;
            SoundManager.instance.playAudio(win, 0.5f);
            finishUI.SetActive(true);
            finishUI.transform.GetChild(0).GetComponent<Text>().text = "Level"+ PlayerPrefs.GetInt("Level",1);           
        }


    }


    private void OnCollisionStay(Collision collision) // Collision(Çarpışma) devam ettiğinde
    {
        if (!tap || collision.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0); // Collision(çarpışma) olduğunda topa her zaman yukarı doğru hız verilir
            SoundManager.instance.playAudio(bounce, 0.5f); // Bounce soundtrack çalışır
            

        }
    }



}
