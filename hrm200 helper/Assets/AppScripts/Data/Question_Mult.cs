using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Core;

public class Question_Mult : MonoBehaviour
{

    //================Mata Data================
    List<A_question> Question_set = new List<A_question>();
    enum Cur_Mode { View, Test };
    Cur_Mode CurrentMode = Cur_Mode.Test;
    public string Current_Cpt = "";
    int Current_Range_Min = 0;
    int Current_Range_Max = 0;
    public string choosed_Range = "";
    public int current_viewed_question = 0;
    //================Scenes================
    public GameObject S_Chapter;

    //================Buttons================
    public GameObject Btn_Last;
    public GameObject Btn_Next;
    public GameObject Btn_GoBack;
    public GameObject Btn_SwitchMode;

    //================Inputs================
    public GameObject Inpute_Question;
    public GameObject Inpute_A;
    public GameObject Inpute_B;
    public GameObject Inpute_C;
    public GameObject Inpute_D;
    public GameObject Inpute_E;



    // Use this for initialization
    void Start()
    {
        EventTriggerListener.Get(Btn_Last).onClick = OnButtonClick;
        EventTriggerListener.Get(Btn_Next).onClick = OnButtonClick;
        EventTriggerListener.Get(Btn_GoBack).onClick = OnButtonClick;
        EventTriggerListener.Get(Btn_SwitchMode).onClick = OnButtonClick;
        EventTriggerListener.Get(Inpute_A).onClick = OnButtonClick;
        EventTriggerListener.Get(Inpute_B).onClick = OnButtonClick;
        EventTriggerListener.Get(Inpute_C).onClick = OnButtonClick;
        EventTriggerListener.Get(Inpute_D).onClick = OnButtonClick;
        EventTriggerListener.Get(Inpute_E).onClick = OnButtonClick;
    }

    public void PreSet(){
        Process_range(choosed_Range);
        Load_Questions(Current_Cpt, Current_Range_Min, Current_Range_Max );
    }

    void Set_question(int question_num)
    {

        if (question_num >= 0 && question_num < Question_set.Count())
        {

            Inpute_Question.GetComponent<InputField>().text =  Question_set[question_num].Question_Num + ") " + Question_set[question_num].The_Question;
            Inpute_A.transform.FindChild("Text").GetComponent<Text>().text = Question_set[question_num].Answer_A;
            Inpute_B.transform.FindChild("Text").GetComponent<Text>().text = Question_set[question_num].Answer_B;
            Inpute_C.transform.FindChild("Text").GetComponent<Text>().text = Question_set[question_num].Answer_C;
            Inpute_D.transform.FindChild("Text").GetComponent<Text>().text = Question_set[question_num].Answer_D;
            Inpute_E.transform.FindChild("Text").GetComponent<Text>().text = Question_set[question_num].Answer_E;

            if (CurrentMode == Cur_Mode.Test)
            {
                Inpute_A.GetComponent<Image>().color = Color.white;
                Inpute_B.GetComponent<Image>().color = Color.white;
                Inpute_C.GetComponent<Image>().color = Color.white;
                Inpute_D.GetComponent<Image>().color = Color.white;
                Inpute_E.GetComponent<Image>().color = Color.white;
            }
            else if (CurrentMode == Cur_Mode.View)
            {
                Inpute_A.GetComponent<Image>().color = Color.white;
                Inpute_B.GetComponent<Image>().color = Color.white;
                Inpute_C.GetComponent<Image>().color = Color.white;
                Inpute_D.GetComponent<Image>().color = Color.white;
                Inpute_E.GetComponent<Image>().color = Color.white;

                show_real_answer();
            }
        }
    }

    void Load_Questions(string Chapters, int Min, int Max)
    {
        new GameSparks.Api.Requests.LogEventRequest().SetEventKey("Get_Mult_Questions")
           .SetEventAttribute("Chapter", Chapters)
           .SetEventAttribute("Max", Max)
           .SetEventAttribute("Min", Min)
           .Send((response) =>
           {
               if (!response.HasErrors)
               {
                   List<GSData> Questions = response.ScriptData.GetGSDataList("Questions");
                   for (int i = 0; i < Questions.Count; i++)
                   {
                       A_question tmp_new_question = new A_question();

                       tmp_new_question.Question_Num = System.Int32.Parse(Questions[i].GetNumber("Question_Num").ToString());
                       tmp_new_question.Chapter = System.Int32.Parse(Current_Cpt);
                       tmp_new_question.The_Question = Questions[i].GetString("The_Question");
                       tmp_new_question.Answer_A = Questions[i].GetString("Answer_A");
                       tmp_new_question.Answer_B = Questions[i].GetString("Answer_B");
                       tmp_new_question.Answer_C = Questions[i].GetString("Answer_C");
                       tmp_new_question.Answer_D = Questions[i].GetString("Answer_D");
                       tmp_new_question.Answer_E = Questions[i].GetString("Answer_E");
                       tmp_new_question.Real_Answer = Questions[i].GetString("Real_Answer");

                       Question_set.Add(tmp_new_question);
              
                   }
                   Set_question(0);

               }
               else
               {
                   Debug.Log("Error Loading Player Data...");
               }
           });
    }

    void Process_range(string a_range)
    {
        string tmp_first = "";
        string tmp_second = "";
        for (int i = 0, j = 0; i < a_range.Length; i++)
        {
            if (j == 0)
            {
                if (a_range[i] != '-' && a_range[i] != ' ')
                {
                    tmp_first += a_range[i];
                }
                else if (a_range[i] == '-')
                {
                    j++;
                }
            }
            else if (j == 1 && a_range[i] != ' ')
            {
                tmp_second += a_range[i];
            }
        }
        Current_Range_Min = System.Int32.Parse(tmp_first);
        Current_Range_Max = System.Int32.Parse(tmp_second);
    }

    void OnButtonClick(GameObject pObj)
    {
        if (pObj == Btn_Next)
        {
            current_viewed_question++;
            if (current_viewed_question >= Question_set.Count())
            {
                current_viewed_question = Question_set.Count() - 1;
            }

            Set_question(current_viewed_question);
        }
        else if (pObj == Btn_Last)
        {
            current_viewed_question--;
            if (current_viewed_question < 0)
            {
                current_viewed_question = 0;
            }

            Set_question(current_viewed_question);
        }
        else if (pObj == Btn_SwitchMode)
        {
            if (CurrentMode == Cur_Mode.View)
            {
                CurrentMode = Cur_Mode.Test;
                Btn_SwitchMode.transform.Find("Text").gameObject.GetComponent<Text>().text = "Switch\n to\n View\n Mode\n";
                Inpute_A.GetComponent<Image>().color = Color.white;
                Inpute_B.GetComponent<Image>().color = Color.white;
                Inpute_C.GetComponent<Image>().color = Color.white;
                Inpute_D.GetComponent<Image>().color = Color.white;
                Inpute_E.GetComponent<Image>().color = Color.white;

                Inpute_A.GetComponent<Button>().interactable = true;
                Inpute_B.GetComponent<Button>().interactable = true;
                Inpute_C.GetComponent<Button>().interactable = true;
                Inpute_D.GetComponent<Button>().interactable = true;
                Inpute_E.GetComponent<Button>().interactable = true;

            }
            else if (CurrentMode == Cur_Mode.Test)
            {
                CurrentMode = Cur_Mode.View;
                Btn_SwitchMode.transform.Find("Text").gameObject.GetComponent<Text>().text = "Switch\n to\n Test\n Mode\n";
                Inpute_A.GetComponent<Image>().color = Color.white;
                Inpute_B.GetComponent<Image>().color = Color.white;
                Inpute_C.GetComponent<Image>().color = Color.white;
                Inpute_D.GetComponent<Image>().color = Color.white;
                Inpute_E.GetComponent<Image>().color = Color.white;

                Inpute_A.GetComponent<Button>().interactable = false;
                Inpute_B.GetComponent<Button>().interactable = false;
                Inpute_C.GetComponent<Button>().interactable = false;
                Inpute_D.GetComponent<Button>().interactable = false;
                Inpute_E.GetComponent<Button>().interactable = false;

                show_real_answer();
            }
        }
        else if (pObj == Inpute_A)
        {
            if (Question_set[current_viewed_question].Real_Answer != "A")
            {
                Inpute_A.GetComponent<Image>().color = Color.red;
            }
            show_real_answer();
        }
        else if (pObj == Inpute_B)
        {
            if (Question_set[current_viewed_question].Real_Answer != "B")
            {
                Inpute_B.GetComponent<Image>().color = Color.red;
            }
            show_real_answer();
        }
        else if (pObj == Inpute_C)
        {
            if (Question_set[current_viewed_question].Real_Answer != "C")
            {
                Inpute_C.GetComponent<Image>().color = Color.red;
            }
            show_real_answer();
        }
        else if (pObj == Inpute_D)
        {
            if (Question_set[current_viewed_question].Real_Answer != "D")
            {
                Inpute_D.GetComponent<Image>().color = Color.red;
            }
            show_real_answer();
        }
        else if (pObj == Inpute_E)
        {
            if (Question_set[current_viewed_question].Real_Answer != "E")
            {
                Inpute_E.GetComponent<Image>().color = Color.red;
            }
            show_real_answer();
        }
        else if (pObj == Btn_GoBack)
        {
            Question_set.Clear();
            CurrentMode = Cur_Mode.Test;
            Current_Cpt = "";
            Current_Range_Min = 0;
            Current_Range_Max = 0;
            choosed_Range = "";
            current_viewed_question = 0;

            Inpute_Question.GetComponent<InputField>().text = "";
            Inpute_A.transform.FindChild("Text").GetComponent<Text>().text = "";
            Inpute_B.transform.FindChild("Text").GetComponent<Text>().text = "";
            Inpute_C.transform.FindChild("Text").GetComponent<Text>().text = "";
            Inpute_D.transform.FindChild("Text").GetComponent<Text>().text = "";
            Inpute_E.transform.FindChild("Text").GetComponent<Text>().text = "";

            this.gameObject.SetActive(false);
            S_Chapter.SetActive(true);
        }
    }

    void show_real_answer()
    {
        switch (Question_set[current_viewed_question].Real_Answer)
        {
            case "A":
                Inpute_A.GetComponent<Image>().color = Color.green;
                break;
            case "B":
                Inpute_B.GetComponent<Image>().color = Color.green;
                break;
            case "C":
                Inpute_C.GetComponent<Image>().color = Color.green;
                break;
            case "D":
                Inpute_D.GetComponent<Image>().color = Color.green;
                break;
            case "E":
                Inpute_E.GetComponent<Image>().color = Color.green;
                break;

            default:
                break;
        }
    }
}
