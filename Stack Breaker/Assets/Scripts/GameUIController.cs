using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public Image levelSlider;

    public GameObject settingBTN;
    public GameObject allBTN;

    public GameObject soundONBTN;
    public GameObject soundOFFBTN;
    public bool soundOnOffBo;

    public bool buttonSettingBo;
    
    public Material sliderMat;

    private PlayerController player;
    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject gameUI;

    void Start()
    {
        gameUI.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = "" + PlayerPrefs.GetInt("Level", 1);
        gameUI.transform.GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = "" + PlayerPrefs.GetInt("nLevel", 1);

        sliderMat = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        player = FindObjectOfType<PlayerController>();
        levelSlider.transform.GetComponent<Image>().color = sliderMat.color + Color.gray;

        levelSlider.color = sliderMat.color;
        
        soundONBTN.GetComponent<Button>().onClick.AddListener((() => SoundManager.instance.SoundOnOff()));
        soundOFFBTN.GetComponent<Button>().onClick.AddListener((() => SoundManager.instance.SoundOnOff()));

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ignoreUI() && player.playerstate == PlayerController.PlayerState.Prepare)
        {
                player.playerstate = PlayerController.PlayerState.Playing;
                homeUI.SetActive(false);
                gameUI.SetActive(true);
                
        }
        
        
        
        if (SoundManager.instance.sound)
        {
            soundONBTN.SetActive(true);
            soundOFFBTN.SetActive(false);
        }
        else
        {
            soundONBTN.SetActive(false);
            soundOFFBTN.SetActive(true);
        }
    }

    
    private bool ignoreUI()
    {
        
        PointerEventData pointerEventData=new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        
        List<RaycastResult> raycastResults=new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData,raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<IgnoreGameUI>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }
        
        Debug.Log("--->>"+raycastResults.Count);

        return raycastResults.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void settingShow()
    {
        buttonSettingBo = !buttonSettingBo;
        allBTN.SetActive(buttonSettingBo);
    }

  
    
    
}
