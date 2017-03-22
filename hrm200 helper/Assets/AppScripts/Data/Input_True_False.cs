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

public class A_question_Mult
{
    public int Chapter = 0;
    public int Question_Num = 0;
    public string The_Question = "";
    public string Real_Answer = "";
};


public class Input_True_False : MonoBehaviour
{

    public enum Procress_Section { Q_num, Q_Q, Q_A, Q_right, Q_EX, Done }

    //Scenes
    //public GameObject S_AuthenticationPanel;
    public GameObject S_Input;

    //public PlayerInfo P_Info;

    public Button Input_True;
    public Button Input_False;
    public Button To_Mult;
    public InputField Input_Chapter;
    public InputField Input_Question;
    public InputField Input_Start_Num;

    List<A_question_Mult> Question_List = new List<A_question_Mult>();


    // Use this for initialization
    void Start()
    {
        EventTriggerListener.Get(Input_True.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(Input_False.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(To_Mult.gameObject).onClick = OnButtonClick;
    }

    int Tmp_Chapter = 0;
    int Tmp_Start_Num = 0;
    string Tmp_Question_Num = "";
    string Tmp_True_False;
    void OnButtonClick(GameObject pObj)
    {
        Debug.Log("in");
        if (pObj == Input_True.gameObject)
        {
            Tmp_True_False = "true";
            Tmp_Chapter = System.Int32.Parse(Input_Chapter.text);
            Tmp_Start_Num = Int32.Parse(Input_Start_Num.text);
            Procress_Questions(Input_Question.text);
            Input_Question.text = "";
        }
        else if(pObj == Input_False.gameObject)
        {
            Tmp_True_False = "false";
            Tmp_Chapter = System.Int32.Parse(Input_Chapter.text);
            Tmp_Start_Num = Int32.Parse(Input_Start_Num.text);
            Procress_Questions(Input_Question.text);
            Input_Question.text = "";
        }
        else if(pObj == To_Mult.gameObject)
        {
            this.gameObject.SetActive(false);
            S_Input.SetActive(true);

        }

    }



    void Procress_Questions(string Question)
    {
        Debug.Log("Start: ");

        string New_Question = Question.Replace(System.Environment.NewLine, "  ");

        Procress_Section Current = Procress_Section.Q_num;
        Question_List.Clear();
        Debug.Log("@@Question Get : " + New_Question);
        for (int i = 0; i < New_Question.Length; i++)
        {
            switch (Current)
            {
                //Question num
                case Procress_Section.Q_num:
                    if (char.IsNumber(New_Question[i]))
                    {
                        if (Tmp_Question_Num == "")
                        {
                            Question_List.Add(new A_question_Mult());
                            Debug.Log("Question Num Start: ");
                        }
                        Tmp_Question_Num = Tmp_Question_Num + New_Question[i];
                    }
                    else if (New_Question[i] == ')')
                    {
                        Question_List.Last().Question_Num = System.Int32.Parse(Tmp_Question_Num) - Tmp_Start_Num + 1;
                        Debug.Log("Done: Question Number is " + Tmp_Question_Num);
                        Current = Procress_Section.Q_Q;
                    }
                    break;
                case Procress_Section.Q_Q:

                    if (New_Question[i] != ' ')
                    {

                        int end_index;
                        string end_string = " " + Tmp_Question_Num + ") ";
                        Debug.Log("Question Start: ");
                        Debug.Log("--: Question End is " + end_string);

                        end_index = New_Question.IndexOf(end_string);

                        Question_List.Last().The_Question = New_Question.Substring(i, end_index - i);

                        Debug.Log("Done: Question is " + Question_List.Last().The_Question);

                        i = end_index + Tmp_Question_Num.Length + 1;

                        Current = Procress_Section.Q_right;

                    }
                    break;
                case Procress_Section.Q_right:
                    Debug.Log("Real Answer Start: ");

                    Question_List.Last().Real_Answer = Tmp_True_False;
                    Debug.Log("Done: Real Answer is " + Question_List.Last().Real_Answer);

                    Current = Procress_Section.Done;

                    new GameSparks.Api.Requests.LogEventRequest()
                     .SetEventKey("Add_TF_Question")
                     .SetEventAttribute("Chapter", Tmp_Chapter.ToString())
                     .SetEventAttribute("Question_Num", Question_List.Last().Question_Num)
                     .SetEventAttribute("The_Question", Question_List.Last().The_Question)
                     .SetEventAttribute("Real_Answer", Question_List.Last().Real_Answer)
                     .Send((response) => {
                         if (!response.HasErrors)
                         {
                             Debug.Log("Player Saved To GameSparks...");
                         }
                         else
                         {
                             Debug.Log("Error Saving Player Data...");
                         }
                     });
                    
                    break;

                case Procress_Section.Done:

                    Tmp_Start_Num = 0;
                    Tmp_Question_Num = "";
                    break;

            }
        }
    }
}
