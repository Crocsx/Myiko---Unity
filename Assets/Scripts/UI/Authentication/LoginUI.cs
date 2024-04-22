using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LoginUI: MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;

    public AuthenticationController authenticationController;


    public void Register() {
        authenticationController.ShowRegistrationPanel();
    }

    public void Login()
    {
        authenticationController.Login(email.text, password.text);
    }
}
