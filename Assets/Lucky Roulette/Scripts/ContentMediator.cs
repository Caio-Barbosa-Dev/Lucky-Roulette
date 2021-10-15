using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContentMediator : MonoBehaviour
{
    RouletteAPI currentApi;

    public List<string> charNames = new List<string>();
    public List<Texture2D> charImages = new List<Texture2D>();
    int characterCount = 12;

    Action<List<string>> gotNames;
    Action<List<Texture2D>> gotImages;
    Action<int> returnCount;

    public int namesCounter = 0;

    
    public void InitializeComponents(Action<int> callback)
    {

        returnCount = callback;

        currentApi = GetComponent<RouletteAPI>();
        currentApi.StartArrayCoroutine(SetCompleteCharacterCount);
    }


    void SetCompleteCharacterCount(int count)
    {
        characterCount = count;
        currentApi.CheckIfValidIndex(count, ValidateCharacterCount);
    }


    void ValidateCharacterCount(bool valid)
    {
        if (!valid)
        {
            characterCount -= 75;
            currentApi.CheckIfValidIndex(characterCount, ValidateCharacterCount);
        }
        else
        {
            returnCount(characterCount);
        }
    }



    public void GetCharacterData(List<int> ids, Action<List<string>> callback, Action<List<Texture2D>> callback2)
    {

        gotNames = callback;
        gotImages = callback2;
        

        for (int i = 0; i < ids.Count; i++)
        {
            //Debug.Log("i: " + i);

            currentApi.StartGetDataCoroutine(ids[i], AddCharacterData);
        }

    }


   

    void AddCharacterData(string name, Texture2D image)
    {
        //Debug.Log("AddCharacterName: " + name);

        charNames.Add(name);
        charImages.Add(image);
        namesCounter++;

        if (namesCounter == 12)
        {
            gotNames(charNames);
            gotImages(charImages);
        }

    }






}
