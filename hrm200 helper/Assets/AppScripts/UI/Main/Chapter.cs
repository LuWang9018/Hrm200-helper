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

public class Chapter : MonoBehaviour
{

    //================Mata Data================
    float Width = Screen.width;
    float Heigth = Screen.height;
    string this_chapter = "";
    int TotalMultNum = 0;
    int TotalTFNum = 0;

    //================Pre Fab================
    public GameObject Charp_Pre;
    public GameObject QuestionSetMult;
    public GameObject QuestionTFSet;

    //================Scenes================
    public GameObject S_Main;
    public GameObject S_Question_Mult;
    public GameObject S_Question_TF;

    //================Buttons================
    public Button Go_Back;

    //================Inputs================

    //================Text================
    public Text Mult;
    public Text TF;
    public Text Title;

    void Start()
    {
        EventTriggerListener.Get(Go_Back.gameObject).onClick = OnButtonClick;
    }

    void Gen_Question_Select()
    {
        for (int j = 0, i = 0; j < 5 && i < TotalMultNum; j++, i +=20)
        {
            int EndNum = 0;
            if ((j + 1) * 20 <= TotalMultNum)
            {
                EndNum = (j + 1) * 20;
            }
            else
            {
                EndNum = TotalMultNum;
            }
            GameObject tmp_pfb = Gen_Mult_Button((j * 20 + 1), EndNum);
            tmp_pfb.transform.localPosition = new Vector3(0, 0, 0);
            tmp_pfb.transform.localScale = new Vector3(1, 1, 1);
            //tmp_pfb.transform.position = new Vector3(Width / 2f, Heigth / 2f, 0);
            tmp_pfb.transform.position += new Vector3((j - 2) * 270 * Width / 2208, 0, 0);
            EventTriggerListener.Get(tmp_pfb).onClick = OnButtonClick;
        }
        
        for (int j = 0, i = 0; j < 5 && i < TotalTFNum; j++, i += 20)
        {
            Debug.Log("J is: " + j + " I is : " + i + " TotalTFMun is : " + TotalTFNum );
            int EndNum = 0;
            if ((j + 1) * 20 <= TotalTFNum)
            {
                EndNum = (j + 1) * 20;
            }
            else
            {
                EndNum = TotalTFNum;
            }
            GameObject tmp_pfb = Gen_TF_Button((j * 20 + 1), EndNum);
            tmp_pfb.transform.localPosition = new Vector3(0, 0, 0);
            tmp_pfb.transform.localScale = new Vector3(1, 1, 1);
            //tmp_pfb.transform.position = new Vector3(Width / 2f, Heigth / 2f, 0);
            tmp_pfb.transform.position += new Vector3((j - 2) * 270 * Width / 2208, 0, 0);
            EventTriggerListener.Get(tmp_pfb).onClick = OnButtonClick;
        }
        
    }

    GameObject Gen_Mult_Button(int StartNum, int EndNum)
    {
        GameObject go = (GameObject)Instantiate(Charp_Pre);
        go.transform.parent = QuestionSetMult.transform;
        go.name = StartNum.ToString() + " - " + EndNum.ToString();
        go.transform.FindChild("Text").gameObject.GetComponent<Text>().text = go.name;
        return go;
    }

    GameObject Gen_TF_Button(int StartNum, int EndNum)
    {
        GameObject go = (GameObject)Instantiate(Charp_Pre);
        go.transform.parent = QuestionTFSet.transform;
        go.name = StartNum.ToString() + " - " + EndNum.ToString();
        go.transform.FindChild("Text").gameObject.GetComponent<Text>().text = go.name;
        return go;
    }

    void OnButtonClick(GameObject pObj)
    {
        string chooced_chapter = pObj.transform.FindChild("Text").gameObject.GetComponent<Text>().text;

        this.gameObject.SetActive(false);

        if (pObj == Go_Back.gameObject)
        {
            Debug.Log("Has " + QuestionTFSet.transform.childCount + " Children");
            for (var i = QuestionTFSet.transform.childCount - 1; i >= 0; i--)
            {
                Debug.Log("I =  " + i);

                // objectA is not the attached GameObject, so you can do all your checks with it.
                var objectA = QuestionTFSet.transform.GetChild(i);
                Destroy(objectA.gameObject);
                // Optionally destroy the objectA if not longer needed
            }

            for (var i = QuestionSetMult.transform.childCount - 1; i >= 0; i--)
            {
                // objectA is not the attached GameObject, so you can do all your checks with it.
                var objectA = QuestionSetMult.transform.GetChild(i);
                Destroy(objectA.gameObject);
                // Optionally destroy the objectA if not longer needed
            }

            this.gameObject.SetActive(false);
            S_Main.gameObject.SetActive(true);
        }
        else if (pObj.transform.parent.name == "QuestionSetMult")
        {
            S_Question_Mult.GetComponent<Question_Mult>().choosed_Range = chooced_chapter;
            S_Question_Mult.GetComponent<Question_Mult>().Current_Cpt = this_chapter;
            S_Question_Mult.GetComponent<Question_Mult>().current_viewed_question = 0;
            S_Question_Mult.GetComponent<Question_Mult>().PreSet();
            S_Question_Mult.SetActive(true);
        }
        else if(pObj.transform.parent.name == "QuestionTFSet")
        {
            S_Question_TF.GetComponent<Question_TF>().choosed_Range = chooced_chapter;
            S_Question_TF.GetComponent<Question_TF>().Current_Cpt = this_chapter;
            S_Question_TF.GetComponent<Question_TF>().current_viewed_question = 0;
            S_Question_TF.GetComponent<Question_TF>().PreSet();

            S_Question_TF.SetActive(true);
        }

    }

    public void Update_Data(string chooced_chapter)
    {
        Title.text = "Chapter " + chooced_chapter;
        this_chapter = chooced_chapter;
        new GameSparks.Api.Requests.LogEventRequest().SetEventKey("Get_Mult_Nums")
          .SetEventAttribute("Chapter", chooced_chapter)
          .Send((response) =>
          {
              if (!response.HasErrors)
              {
                  int Mult_Num = System.Int32.Parse(response.ScriptData.GetNumber("ChapterNum").ToString());

                  print("--Chapter has " + response.ScriptData.GetNumber("ChapterNum").ToString() + " questions.");


                  new GameSparks.Api.Requests.LogEventRequest().SetEventKey("Get_TF_Num")
                             .SetEventAttribute("Chapter", chooced_chapter)
                             .Send((response1) =>
                             {
                                 if (!response1.HasErrors)
                                 {
                                     int TF_Num = System.Int32.Parse(response1.ScriptData.GetNumber("ChapterNum").ToString());
                                     print("--Chapter has " + response1.ScriptData.GetNumber("ChapterNum").ToString() + " questions.");

                                     

                                     TotalMultNum = Mult_Num;
                                     TotalTFNum = TF_Num;
                                     Gen_Question_Select();

                                     Mult.text = "Total of " + Mult_Num.ToString() + " MULTIPLE CHOICE";

                                     TF.text = "Total of " + TF_Num.ToString() + " True False";
                                 }
                                 else
                                 {
                                     Debug.Log("Error Loading Player Data...");
                                 }
                             });


                  this.gameObject.SetActive(true);
                  S_Main.gameObject.SetActive(false);
              }
              else
              {
                  Debug.Log("Error Loading Player Data...");
              }
          });
    }
}

