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

public class Input_Cho_Confirm : MonoBehaviour {

    //Scripts
    public A_question The_Question;

    //Scenes
    public GameObject S_Input;

    //public PlayerInfo P_Info;

    public Button Input_Enter;
    public InputField Input_Chapter;
    public InputField Input_Question;
    public InputField Input_Ans_A;
    public InputField Input_Ans_B;
    public InputField Input_Ans_C;
    public InputField Input_Ans_D;
    public InputField Input_Ans_E;

    //vars
    string Tmp_real;

    // Use this for initialization
    void Start()
    {
        EventTriggerListener.Get(Input_Enter.gameObject).onClick = OnButtonClick;
    }

    int Tmp_Chapter = 0;
    string Tmp_Question_Num = "";


    void OnButtonClick(GameObject pObj)
    {
        Debug.Log("in");
        if (pObj == Input_Enter.gameObject)
        {
            new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("Add_Mult_Question")
            .SetEventAttribute("Chapter", The_Question.Chapter.ToString())
            .SetEventAttribute("Question_Num", The_Question.Question_Num)
            .SetEventAttribute("The_Question", The_Question.The_Question)
            .SetEventAttribute("Answer_A", Input_Ans_A.text)
            .SetEventAttribute("Answer_B", Input_Ans_B.text)
            .SetEventAttribute("Answer_C", Input_Ans_C.text)
            .SetEventAttribute("Answer_D", Input_Ans_D.text)
            .SetEventAttribute("Answer_E", Input_Ans_E.text)
            .SetEventAttribute("Real_Answer", The_Question.Real_Answer)
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

            this.gameObject.SetActive(false);
            S_Input.SetActive(true);
        }
    }

	void OnEnable()
    {
        Input_Chapter.text = The_Question.Chapter.ToString();
        Input_Question.text = The_Question.Question_Num.ToString() + ") " + The_Question.The_Question;
        Input_Ans_A.text = The_Question.Answer_A;
        Input_Ans_B.text = The_Question.Answer_B;
        Input_Ans_C.text = The_Question.Answer_C;
        Input_Ans_D.text = The_Question.Answer_D;
        Input_Ans_E.text = The_Question.Answer_E;

        GameObject.Find("Input_C_Answer_A").gameObject.GetComponent<Image>().color = new Color(1F, 1F, 1F);
        GameObject.Find("Input_C_Answer_B").gameObject.GetComponent<Image>().color = new Color(1F, 1F, 1F);
        GameObject.Find("Input_C_Answer_C").gameObject.GetComponent<Image>().color = new Color(1F, 1F, 1F);
        GameObject.Find("Input_C_Answer_D").gameObject.GetComponent<Image>().color = new Color(1F, 1F, 1F);
        GameObject.Find("Input_C_Answer_E").gameObject.GetComponent<Image>().color = new Color(1F, 1F, 1F);


        Tmp_real = "Input_C_Answer_" + The_Question.Real_Answer;
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!! :" + Tmp_real);
        GameObject Right_obj = GameObject.Find(Tmp_real).gameObject;
        Right_obj.GetComponent<Image>().color = new Color(1F, 1/255F*0.133F, 1/255F*0.102F);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
