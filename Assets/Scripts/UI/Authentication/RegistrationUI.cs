using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class RegistrationUI : MonoBehaviour
{
    [Header("Login")]
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField passwordConfirmation;

    public AuthenticationController authenticationController;



    public void Login()
    {
        authenticationController.ShowLoginPanel();
    }

    public void Register()
    {
        authenticationController.Register(email.text, password.text);
    }

}
