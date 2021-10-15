using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using System;
//using TMPro;

public class RouletteAPI : MonoBehaviour
{

    private readonly string basePokeURL = "https://pokeapi.co/api/v2/";
    private readonly string listURL = "https://pokeapi.co/api/v2/pokemon";

    public void StartGetDataCoroutine(int id, Action<string,Texture2D> callback)
    {
        StartCoroutine(GetPokemonAtIndex(id, callback));
    }
    IEnumerator GetPokemonAtIndex(int pokemonIndex, Action<string, Texture2D> callback)
    {

        //Debug.Log("GetPokemonAtIndex(): " + pokemonIndex);

        // Get Pokemon Info

        string pokemonURL = basePokeURL + "pokemon/" + pokemonIndex.ToString();
        // Example URL: https://pokeapi.co/api/v2/pokemon/151

        UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);

        yield return pokeInfoRequest.SendWebRequest();

        if (pokeInfoRequest.isNetworkError || pokeInfoRequest.isHttpError)
        {
            Debug.LogError(pokeInfoRequest.error);
            Debug.Log("GetPokemonAtIndex(): " + pokemonIndex);
            yield break;
        }


        JSONNode pokeInfo = JSON.Parse(pokeInfoRequest.downloadHandler.text);

        string pokeName = pokeInfo["name"];
        string pokeSpriteURL = pokeInfo["sprites"]["front_default"];


        // Get Pokemon Sprite

        UnityWebRequest pokeSpriteRequest = UnityWebRequestTexture.GetTexture(pokeSpriteURL);

        yield return pokeSpriteRequest.SendWebRequest();

        if (pokeSpriteRequest.isNetworkError || pokeSpriteRequest.isHttpError)
        {

            Debug.Log("GetSprite(): " + pokemonIndex);
            Debug.LogError(pokeSpriteRequest.error);
            yield break;
        }

        // Set UI Objects

        Texture2D pokeImage = DownloadHandlerTexture.GetContent(pokeSpriteRequest); ;

        pokeName = char.ToUpper(pokeName[0]) + pokeName.Substring(1);
        callback(pokeName, pokeImage);



    }




    #region Check if the character count if valid

    public void CheckIfValidIndex(int id,Action<bool> callback)
    {
        StartCoroutine(CheckPokemonAtIndex(id, callback));
    }

    IEnumerator CheckPokemonAtIndex(int id, Action<bool> callback)
    {

        //Debug.Log("GetPokemonAtIndex(): " + pokemonIndex);

        // Get Pokemon Info

        string pokemonURL = basePokeURL + "pokemon/" + id.ToString();
        // Example URL: https://pokeapi.co/api/v2/pokemon/151

        UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);

        yield return pokeInfoRequest.SendWebRequest();

        if (pokeInfoRequest.isNetworkError || pokeInfoRequest.isHttpError)
        {
            Debug.LogError(pokeInfoRequest.error);
            Debug.Log("GetPokemonAtIndex(): " + id);

            callback(false);
            yield break;
        }

        callback(true);
    }




    #endregion


    public void StartArrayCoroutine(Action<int> callback)
    {
        StartCoroutine(GetPokemonList(callback));
    }



    IEnumerator GetPokemonList(Action<int> callback)
    {
        string pokemonURL = listURL;
        //string pokemonURL = basePokeURL + "pokemon/" + 1;
        //string pokemonURL = "https://rickandmortyapi.com/api/character";


        UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);

        yield return pokeInfoRequest.SendWebRequest();

        if (pokeInfoRequest.isNetworkError || pokeInfoRequest.isHttpError)
        {
            Debug.LogError(pokeInfoRequest.error);
            yield break;
        }


        //JSONNode pokeInfo = JSON.Parse(pokeInfoRequest.downloadHandler.text);

        JSONNode pokeList = JSON.Parse(pokeInfoRequest.downloadHandler.text);

        //Debug.Log(pokeList.ToString());
   

        string pokeCount = pokeList["count"];
        //string pokeName = pokeList["name"];

        Debug.Log(pokeCount);
        //string pokeSpriteURL = pokeInfo["sprites"]["front_default"];



        callback(int.Parse(pokeCount));
        //callback(850);


    }


}
