using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Authentication_UI : MonoBehaviour
{

    public GameObject m_AuthenticationPanel;
    //public GameObject m_RegisterPanel;
    //public GameObject m_Main;
    public GameObject S_Input_Mult_Question;

    //public PlayerInfo P_Info;

    Button m_LoginBtn;
    Button m_RegisterBtn;
    InputField m_UserName;
    InputField m_Password;

    string m_LoadPath = "AppPrefabs/UI/";

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start--in");
        m_LoginBtn = this.gameObject.transform.Find("Auth_Content_BG/Auth_Btn_Login").GetComponent<Button>();
        m_RegisterBtn = this.gameObject.transform.Find("Auth_Content_BG/Auth_Btn_T_Register").GetComponent<Button>();
        m_UserName = this.gameObject.transform.Find("Auth_Content_BG/Auth_Input_UserName").GetComponent<InputField>();
        m_Password = this.gameObject.transform.Find("Auth_Content_BG/Auth_Input_Password").GetComponent<InputField>();
        
        EventTriggerListener.Get(m_LoginBtn.gameObject).onClick = OnButtonClick;
        EventTriggerListener.Get(m_RegisterBtn.gameObject).onClick = OnButtonClick;
    }


    void OnButtonClick(GameObject pObj)
    {
        Debug.Log("in");
        if (pObj == m_LoginBtn.gameObject)
        {
            Debug.Log("click btn :" + pObj.name);
            Debug.Log("UserName = " + m_UserName.text + " ///  Password = " + m_Password.text);
            new GameSparks.Api.Requests.AuthenticationRequest()
                .SetUserName(m_UserName.text)
                .SetPassword(m_Password.text)
                .Send((response) => {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Authenticated...");
                    Debug.Log("Login in the next panel ");
                    Resources.Load(m_LoadPath + "");
                    //Close Auth Panel
                    //to Main menu

                    m_AuthenticationPanel.SetActive(false);
                        S_Input_Mult_Question.SetActive(true);
                    //m_Main.SetActive(true);
                    //load play data
                    //P_Info.Update_P_Info();
                    }
                else
                {

                    Debug.LogError("Error Authenticating Player...");
                }
            });
        }
        else if (pObj == m_RegisterBtn.gameObject)
        {
            //Close Auth Panel
            m_AuthenticationPanel.SetActive(false);

            //Open Reg Panel
            //m_RegisterPanel.SetActive(true);

        }
    }
}
