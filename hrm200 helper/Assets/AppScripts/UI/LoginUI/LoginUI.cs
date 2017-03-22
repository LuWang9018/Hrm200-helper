using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class LoginUI : MonoBehaviour {

    Button m_LoginBtn;
    Button m_RegisterBtn;
    InputField m_UserName;
    InputField m_Password;
    string m_LoadPath = "AppPrefabs/UI/";

	// Use this for initialization
	void Start () {
       m_LoginBtn =  this.gameObject.transform.Find("Content_BG/Auth_Btn_Login").GetComponent<Button>();
       m_RegisterBtn = this.gameObject.transform.Find("Content_BG/Btn_Register").GetComponent<Button>();
       m_UserName = this.gameObject.transform.Find("Content_BG/Input_UserName").GetComponent<InputField>();
       m_Password = this.gameObject.transform.Find("Content_BG/Input_Password").GetComponent<InputField>();

       EventTriggerListener.Get(m_LoginBtn.gameObject).onClick = OnButtonClick;
       EventTriggerListener.Get(m_RegisterBtn.gameObject).onClick = OnButtonClick;
	}
	

    void OnButtonClick(GameObject pObj) {        
            Debug.Log(" in  ");
            if (pObj == m_LoginBtn.gameObject) {
                Debug.Log(" click btn : " + pObj.name);
                Debug.Log("UserName = " + m_UserName.text +" ///  Password = " + m_Password.text);
                new GameSparks.Api.Requests.AuthenticationRequest().SetUserName("Test User 1 "+ m_UserName.text).SetPassword(m_Password.text).Send((response) => {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Player Authenticated...");
                        Debug.Log("Login in the next panel ");
                        Resources.Load(m_LoadPath+"");
                    }
                    else
                    {

                        Debug.LogError("Error Authenticating Player...");
                    }
                });
            }
            else if (pObj == m_RegisterBtn.gameObject){
                Debug.Log(" click btn : " + pObj.name);
                Debug.Log("UserName = " + m_UserName.text + " ///  Password = " + m_Password.text);
                new GameSparks.Api.Requests.RegistrationRequest()
                .SetDisplayName(m_UserName.text)
                .SetPassword(m_Password.text)
                .SetUserName("Test User 1 "+ m_UserName.text)
                .Send((response) => {
                    if (!response.HasErrors){
                        Debug.Log("Player Registered…");
                    }
                    else{
                        Debug.Log("Error Registering Player");
                    }
                });
        }
    }
}
