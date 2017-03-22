using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System.Threading;

public class Device_Auth : MonoBehaviour
{


    public GameObject S_Main;

    public Button m_LoginBtn;


    // Use this for initialization
    void Start()
    {
        EventTriggerListener.Get(this.gameObject).onClick = OnButtonClick;
    }


    //public PlayerInfo P_Info;

    // Use this for initialization
    void OnButtonClick(GameObject pObj)
    {
        Debug.Log("in");
        if (pObj == this.gameObject)
        {

            //Debug.Log("UserName = " + m_UserName.text + " ///  Password = " + m_Password.text);

            string Uni_id = SystemInfo.deviceUniqueIdentifier;
            string Sys_OS = SystemInfo.operatingSystem;

            new DeviceAuthenticationRequest().Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Device Authenticated...");
                    this.gameObject.SetActive(false);
                    S_Main.SetActive(true);
                }
                else
                {
                    Debug.Log("Error Authenticating Device...");
                }
            });


        }
    }
}
