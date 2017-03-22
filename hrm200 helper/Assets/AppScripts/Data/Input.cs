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

public class A_question
{
    public int Chapter = 0;
    public int Question_Num = 0;
    public string The_Question = "";
    public string Answer_A = "";
    public string Answer_B = "";
    public string Answer_C = "";
    public string Answer_D = "";
    public string Answer_E = "";
    public string Real_Answer = "";
};

public class Input : MonoBehaviour
{

    public enum Procress_Section{ Q_num, Q_Q, Q_A, Q_right, Q_EX, Done }
    //Scenes
    public GameObject S_AuthenticationPanel;
    public GameObject S_Input_Confirm;
    public GameObject S_TF;

    //public PlayerInfo P_Info;

    public Button Input_Enter;
    public Button To_TF
        ;
    public InputField Input_Chapter;
    public InputField Input_Question;


    List<A_question> Question_List = new List<A_question>();
    // Use this for initialization
    void Start()
    {
        EventTriggerListener.Get(Input_Enter.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(To_TF.gameObject).onClick = OnButtonClick;
    }


    int Tmp_Chapter = 0;
    string Tmp_Question_Num = "";

    void OnButtonClick(GameObject pObj)
    {
        Debug.Log("in");
        if (pObj == Input_Enter.gameObject)
        {
            int Tmp_Chapter = System.Int32.Parse(Input_Chapter.text);
            Debug.Log("------------------" + Input_Chapter.text);
            Procress_Questions(Input_Question.text);
            //Procress_Questions(test1);
        }
        else if (pObj == To_TF.gameObject)
        {
            this.gameObject.SetActive(false);
            S_TF.gameObject.SetActive(true);
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
                            Question_List.Add(new A_question());
                            Debug.Log("Question Num Start: ");
                        }
                        Tmp_Question_Num = Tmp_Question_Num + New_Question[i];
                    }
                    else if (New_Question[i] == ')')
                    {
                        Question_List.Last().Question_Num = System.Int32.Parse(Tmp_Question_Num);
                        Debug.Log("Done: Question Number is " + Tmp_Question_Num);
                        Current = Procress_Section.Q_Q;
                    }
                    break;
                case Procress_Section.Q_Q:
                    
                    if(New_Question[i] != ' ')
                    {
                        int end_index;
                        string end_string = " " + Tmp_Question_Num + ") ";
                        Debug.Log("Question Start: ");
                        Debug.Log("--: Question End is " + end_string);

                        end_index = New_Question.IndexOf(end_string);

                        Question_List.Last().The_Question = New_Question.Substring(i, end_index - i);

                        Debug.Log("Done: Question is " + Question_List.Last().The_Question);

                        i = end_index + Tmp_Question_Num.Length + 1;

                        Current = Procress_Section.Q_A;
                    }
                    break;
                case Procress_Section.Q_A:
                    Debug.Log("Answer Start: ");
                    //A

                    if (New_Question[i] == ' ')
                    {
                        break;
                    }
                    int end_index2;
                    string end_string2 = "B)";

                    end_index2 = New_Question.IndexOf(end_string2);

                    Question_List.Last().Answer_A = New_Question.Substring(i, end_index2 - i - 1);

                    Debug.Log("Done: Answer A is " + Question_List.Last().Answer_A);

                    i = end_index2;

                    //B
                    end_string2 = "C)";

                    end_index2 = New_Question.IndexOf(end_string2);

                    Question_List.Last().Answer_B = New_Question.Substring(i, end_index2 - i - 1);

                    Debug.Log("Done: Answer B is " + Question_List.Last().Answer_B);

                    i = end_index2;

                    //C
                    end_string2 = "D)";

                    end_index2 = New_Question.IndexOf(end_string2);

                    Question_List.Last().Answer_C = New_Question.Substring(i, end_index2 - i - 1);

                    Debug.Log("Done: Answer C is " + Question_List.Last().Answer_C);

                    i = end_index2;


                    //D
                    end_string2 = "E)";

                    end_index2 = New_Question.IndexOf(end_string2);

                    Question_List.Last().Answer_D = New_Question.Substring(i, end_index2 - i - 1);

                    Debug.Log("Done: Answer D is " + Question_List.Last().Answer_D);

                    i = end_index2;

                    //E
                    end_string2 = "Answer:";

                    end_index2 = New_Question.IndexOf(end_string2);

                    Question_List.Last().Answer_E = New_Question.Substring(i, end_index2 - i - 1);

                    Debug.Log("Done: Answer E is " + Question_List.Last().Answer_E);


                    i = end_index2;


                    Current = Procress_Section.Q_right;
                    break;
                case Procress_Section.Q_right:
                    Debug.Log("Real Answer Start: ");

                    string ANSWER = "Answer:";
                    i = i + ANSWER.Length;

                    Question_List.Last().Real_Answer = "" + New_Question[i];
                    Debug.Log("Done: Real Answer is " + Question_List.Last().Real_Answer);

                    Debug.Log("!!!: Current point: " + New_Question[i]);
                    Current = Procress_Section.Q_EX;

                    break;
                case Procress_Section.Q_EX:
                    Debug.Log("EX Start: ");

                    Question_List.Last().Chapter = System.Int32.Parse(Input_Chapter.text);
                    Debug.Log("Chapter Num is: " + Question_List.Last().Chapter);
                    Current = Procress_Section.Done;


                    //set Confirm scene
                    Input_Question.text = "";
                    S_Input_Confirm.GetComponents<Input_Cho_Confirm>()[0].The_Question = Question_List.Last();

                    Tmp_Question_Num = "";

                    this.gameObject.SetActive(false);
                    S_Input_Confirm.SetActive(true);
                    break;
                case Procress_Section.Done:
                    break;
                
            }
            

        }
    }


}
