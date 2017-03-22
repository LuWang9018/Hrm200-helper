using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using GameSparks.Core;


public class RegisterPanel_UI : MonoBehaviour
{

    public GameObject m_AuthenticationPanel;
    public GameObject m_RegisterPanel;
    public GameObject m_Main;

    //public PlayerInfo P_Info;

    InputField m_DisplayName;
    InputField m_UserName;
    InputField m_Password;

    Button m_RegisterBtn;
    Button m_T_Auth_Btn;

    //string m_LoadPath = "AppPrefabs/UI/";

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start--in");
        m_DisplayName = this.gameObject.transform.Find("Reg_Content_BG/Reg_Input_DisplayName").GetComponent<InputField>();
        m_UserName = this.gameObject.transform.Find("Reg_Content_BG/Reg_Input_UserName").GetComponent<InputField>();
        m_Password = this.gameObject.transform.Find("Reg_Content_BG/Reg_Input_Password").GetComponent<InputField>();
        m_RegisterBtn = this.gameObject.transform.Find("Reg_Content_BG/Reg_Btn_Register").GetComponent<Button>();
        m_T_Auth_Btn = this.gameObject.transform.Find("Reg_Content_BG/Reg_Btn_B_Login").GetComponent<Button>();

        EventTriggerListener.Get(m_T_Auth_Btn.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(m_RegisterBtn.gameObject).onClick = OnButtonClick;
    }


    void OnButtonClick(GameObject pObj)
    {
        Debug.Log("in");
        if (pObj == m_T_Auth_Btn.gameObject)
        {
            //Open Auth Panel
            m_AuthenticationPanel.SetActive(true);

            //Close Reg Panel
            m_RegisterPanel.SetActive(false);

        }
        else if (pObj == m_RegisterBtn.gameObject)
        {
            Debug.Log(" click btn : " + pObj.name);
            Debug.Log("UserName = " + m_UserName.text + " ///  Password = " + m_Password.text);

            new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(m_DisplayName.text)
            .SetPassword(m_Password.text)
            .SetUserName(m_UserName.text)
            .Send((response) => {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Registered…");

                    //Set init value
                    init_player();

                    //to Main menu
                    m_RegisterPanel.SetActive(false);
                    m_Main.SetActive(true);

                    //load play data
                    //P_Info.Update_P_Info();

                }
                else
                {
                    Debug.Log("Error Registering Player");
                }
            });
        }

    }

    void init_player()
    {
        GSRequestData jsonDataToSend = new GSRequestData();
        jsonDataToSend.AddNumber("win", 0);
        jsonDataToSend.AddNumber("lost", 0);

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("Player_Init")
            .SetEventAttribute("Game_W_L", jsonDataToSend)
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
    }

}
