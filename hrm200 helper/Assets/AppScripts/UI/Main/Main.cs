using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System;

public class Main : MonoBehaviour {

    //================Mata Data================
    int Width = Screen.width;
    int Heigth = Screen.height;
    public GameObject Charp_Pre;
    public GameObject Chapters;
    //================Scenes================
    public GameObject S_Chapter;
    
    
    //================Buttons================


    //================Inputs================



    void Start()
    {
        for (int i = 0, k = 1; i < 4; i++)
        {
            for (int j = 0; j < 5 && k <= 17; j++, k++)
            {
                GameObject tmp_pfb = Gen_Charp_Button(k);
                tmp_pfb.transform.localScale = new Vector3(1, 1, 1);
                tmp_pfb.transform.position = new Vector3(Width/2f, Heigth/2f, 0);                
                tmp_pfb.transform.position += new Vector3((j-2) * 270 * Width/ 2208, (i - 1) * -170 * Heigth / 1242, 0);
                EventTriggerListener.Get(tmp_pfb).onClick = OnButtonClick;
            }
        }
    }
    /*
    GameObject GenTails(int PlayerNum, string DisplayName, string Kind)
    {
        GameObject go = (GameObject)Instantiate(PlayerPrefab);
        go.transform.parent = Players.transform;
        go.name = "Player_" + PlayerNum;
        go.transform.FindChild("DisplayName").gameObject.GetComponent<Text>().text = DisplayName;
        go.transform.FindChild("Number").gameObject.GetComponent<Text>().text = PlayerNum.ToString();

        return go;
    }
    */

    GameObject Gen_Charp_Button(int charp_num)
    {
        GameObject go = (GameObject)Instantiate(Charp_Pre);
        go.transform.parent = Chapters.transform;
        go.name = "Chapter_" + charp_num;
        go.transform.FindChild("Text").gameObject.GetComponent<Text>().text = charp_num.ToString();
        return go;
    }

    void OnButtonClick(GameObject pObj)
    {
        string chooced_chapter = pObj.transform.FindChild("Text").gameObject.GetComponent<Text>().text;

        S_Chapter.GetComponent<Chapter>().Update_Data(chooced_chapter);

    }
}
