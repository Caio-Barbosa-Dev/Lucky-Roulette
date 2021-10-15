using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RickRoulette : MonoBehaviour
{
    #region Variables
    ContentMediator mediator;
    FinalScreenController finalScreenController;

    [Header("Roulette")]
    public Transform rouletteSpin;
    public GameObject displayPrefab;
    public List<Text> nameDisplays;
    Transform namesParent;
    Button rotateButton;
    Text nameText;


    [Header("Screens")]
    public GameObject rouletteScreen;
    public GameObject nameScreen;
    public CanvasGroup overlay;


    [Header("FinalScreen")]
    public List<Texture2D> sprites = new List<Texture2D>();
    public RawImage characterDisplay;



    public List<string> allNames = new List<string>();
    public List<string> currentNames = new List<string>();
    List<int> currentIds = new List<int>();
    int numberOfCharacters = 12;
    int selectedId; 
    #endregion


    #region Initialization

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }


    void Initialize()
    {
        GetStartingComponents();
        SetStartingComponents();
    }

    void GetStartingComponents()
    {
        
 
        mediator = GetComponent<ContentMediator>();
        mediator.InitializeComponents(SetCharacterCount);

        rotateButton = rouletteScreen.transform.Find("Bottom Bar/Button").GetComponent<Button>();
        nameText = nameScreen.transform.Find("Bar/Button/Text").GetComponent<Text>();
        characterDisplay = nameScreen.transform.Find("Image").GetComponent<RawImage>();

        finalScreenController = nameScreen.GetComponent<FinalScreenController>();
        finalScreenController.InitializeComponents(characterDisplay.transform);

        overlay.alpha = 1.0f;
        rotateButton.interactable = false;
        nameScreen.SetActive(false);
    }



    List<int> GenerateRandomIds(int numberOfCharacters)
    {
        List<int> allIds = new List<int>();
        List<int> ids = new List<int>();

        for (int i = 1; i <= numberOfCharacters; i++)
        {
            allIds.Add(i);
        }

        allIds.Shuffle();
        for (int i = 0; i < 12; i++)
        {
            ids.Add(allIds[i]);
        }

        return ids; 
    }

    void SetStartingComponents()
    {
        SetNameDisplays();


    }

    void SetNameDisplays()
    {
        //Transform namesParent;
        GameObject temp;
        Text text;
        int rot = 90;

        namesParent = rouletteSpin.Find("Names");

        for (int i = 0; i < 12; i++)
        {
            temp = GameObject.Instantiate(displayPrefab);
            temp.transform.SetParent(namesParent,false);

            temp.transform.localPosition = Vector3.zero;

            text = temp.GetComponent<Text>();
            nameDisplays.Add(text);
            //text.text = currentNames[i];

            temp.transform.rotation = Quaternion.Euler(0, 0, rot);
            rot += 30;
        }

    }

    #endregion



    #region CallBack results

    void SetCharacterCount(int count)
    {
        Debug.Log("SetCharacterCount() \ncount: " + count);

        numberOfCharacters = 12;

        if (count > 0)
        {
            numberOfCharacters = count;
        }

        currentIds = GenerateRandomIds(numberOfCharacters);
        mediator.GetCharacterData(currentIds, SetCharacterNames, SetCharacterImages);

    }

    void SetCharacterNames(List<string> names)
    {
        Debug.Log("SetCharacterNames()\nnames: " + names.Count);

        currentNames = names;

        for (int i = 0; i < currentNames.Count; i++)
        {
            nameDisplays[i].text = currentNames[i];
        }


        FadeInScreen();
    }

    void SetCharacterImages(List<Texture2D> images)
    {
        Debug.Log("SetCharacterImages()\nimages: " + images.Count);
        sprites = images;
    }

    #endregion


    #region Fade in
    void FadeInScreen()
    {
        Debug.Log("FadeInScreen()");

        overlay.alpha = 1.0f;
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        float increment, value;
        float delay;

       
        delay = 0.05f;
        increment = delay;
        value = 1.0f;
 


        while (value > 0)
        {
            value -= increment;
            overlay.alpha = value;
            yield return new WaitForSeconds(delay);
        }

        rotateButton.interactable = true;

    }



    #endregion


    #region Roulette Spin
    public void SpinButton()
    {
        rotateButton.interactable = false;

        int id;
        int angle = 0;
        int baseAngle, extraSpins;
        //add seed

        Random.InitState((int)System.DateTime.Now.Ticks);

        id = Random.Range(0, 12);
        selectedId = id;
        nameText.text = "You got a... "+ currentNames[id]+ "!!";
        characterDisplay.texture = sprites[id];

        baseAngle = 30 * id;

        extraSpins = id = Random.Range(3, 10);
        angle = baseAngle + extraSpins * 360;


        StartCoroutine(RotateRoulette(id, angle));
    }
    IEnumerator RotateRoulette(int id, int angle)
    {
        int currentAngle = 0;
        int increase = 2;
        float maxIncrease, minIncrease;
        float perc = 0;

        maxIncrease = 10;
        minIncrease = 1;


        while (currentAngle < angle)
        {

            perc = (float)currentAngle / angle;
            increase = (int)Mathf.Lerp(maxIncrease, minIncrease, perc);

            rouletteSpin.Rotate(new Vector3(0, 0, -increase));

            currentAngle += increase;
            yield return new WaitForSeconds(0.01f);
        }

        ToggleNameDisplays(false);
        nameDisplays[selectedId].gameObject.SetActive(true);


        yield return new WaitForSeconds(1.5f);
        LoadNextScreen();


    }


    void ToggleNameDisplays(bool active)
    {
        for (int i = 0; i < nameDisplays.Count; i++)
        {
            nameDisplays[i].gameObject.SetActive(active);
        }
    }


    #endregion

    public void BackButton()
    {
        ReloadRoulette();
    }



    void LoadNextScreen()
    {
        nameScreen.SetActive(true);
        namesParent.gameObject.SetActive(false);

        finalScreenController.ImagePopup();
    }

    void ReloadRoulette()
    {
        nameScreen.SetActive(false);
        rouletteSpin.rotation = Quaternion.Euler(Vector3.zero);
        rotateButton.interactable = true;

        ToggleNameDisplays(true);
        namesParent.gameObject.SetActive(true);

    }







}
